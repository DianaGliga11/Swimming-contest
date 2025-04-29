using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Networking.Response;
using Service;
using log4net;
using Networking.Networking;
using Networking.Request;

namespace Networking
{

    public class ServicesProxy : IContestServices
    {
        private readonly string host;
        private readonly int port;
        private IMainObserver client;
        private NetworkStream stream;
        private TcpClient connection;
        private readonly Queue<IResponse> responses;
        private volatile bool finished;
        private EventWaitHandle waitHandle;
        private static readonly ILog log = LogManager.GetLogger(typeof(ServicesProxy));
        private readonly object syncLock = new();

        static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        public ServicesProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<IResponse>();
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                finished = false;
                waitHandle = new AutoResetEvent(false);
                StartReader();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            
        }
        private IResponse ReadResponse()
        {
            log.Debug("Waiting for response...");
            waitHandle.WaitOne();  // așteaptă până când un răspuns e pus în coadă

            lock (responses)
            {
                if (responses.Count > 0)
                {
                    IResponse resp = responses.Dequeue();
                    log.Debug($"Returning response from queue: {resp.GetType().Name}");
                    return resp;
                }
            }

            log.Warn("No response in queue after waitHandle set.");
            return new ErrorResponse("No response received.");
        }

        private void SendRequest(IRequest request)
        {
            string payloadJson = JsonSerializer.Serialize(request, request.GetType(), jsonOptions);
            var payloadElement = JsonDocument.Parse(payloadJson).RootElement;

            // wrap into envelope
            var envelope = new {
                type = request.GetType().Name,
                payload = payloadElement
            };
            log.Info($"Sending request: {envelope}");
            string envelopeJson = JsonSerializer.Serialize(envelope, jsonOptions) + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(envelopeJson);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }


        private void StartReader()
        {
            Thread threadWorker = new Thread(Run);
            threadWorker.Start();
        }

public void Run()
{
    using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
    log.Info("Client is running...");

    while (!finished)
    {
        string line;
        try
        {
            line = reader.ReadLine();
        }
        catch (IOException ioex)
        {
            log.Error("Connection closed: " + ioex.Message);
            break;
        }

        if (string.IsNullOrWhiteSpace(line))
            continue;

        log.Debug($"Raw incoming: {line}");
        JsonEnvelope env;
        try
        {
            env = JsonSerializer.Deserialize<JsonEnvelope>(line, jsonOptions);
        }
        catch (Exception ex)
        {
            log.Error($"Invalid envelope, skipping: {ex.Message}");
            continue;
        }

        try
        {
            // verificăm dacă este un update
            if (env.Type == nameof(UpdatedEventsResponse) || env.Type == nameof(UpdatedParticipantsResponse))
            {
                UpdateResponse update = env.Type switch
                {
                    nameof(UpdatedEventsResponse) => env.Payload.Deserialize<UpdatedEventsResponse>(jsonOptions),
                    nameof(UpdatedParticipantsResponse) => env.Payload.Deserialize<UpdatedParticipantsResponse>(jsonOptions),
                    _ => throw new Exception($"Unknown update type: {env.Type}")
                };

                HandleUpdate(update);
            }
            else
            {
                // este răspuns la o cerere normală
                IResponse resp = env.Type switch
                {
                    nameof(OkResponse) => env.Payload.Deserialize<OkResponse>(jsonOptions),
                    nameof(ErrorResponse) => env.Payload.Deserialize<ErrorResponse>(jsonOptions),
                    nameof(AllEventsResponse) => env.Payload.Deserialize<AllEventsResponse>(jsonOptions),
                    nameof(AllParticipantsResponse) => env.Payload.Deserialize<AllParticipantsResponse>(jsonOptions),
                    nameof(EventsWithParticipantsCountResponse) => env.Payload.Deserialize<EventsWithParticipantsCountResponse>(jsonOptions),
                    nameof(EntriesByEventResponse) => env.Payload.Deserialize<EntriesByEventResponse>(jsonOptions),
                    nameof(GetParticipantsForEventWithCountResponse) => env.Payload.Deserialize<GetParticipantsForEventWithCountResponse>(jsonOptions),
                    // ATENȚIE: adaugă NewParticipantResponse aici DOAR dacă îl folosești și ca răspuns la cerere!
                    //nameof(NewParticipantResponse) => env.Payload.Deserialize<NewParticipantResponse>(jsonOptions),
                    _ => throw new Exception($"Unknown response type: {env.Type}")
                };

                lock (responses)
                    responses.Enqueue(resp);
                waitHandle.Set();
            }
        }
        catch (Exception ex)
        {
            log.Error($"Error processing response of type '{env.Type}': {ex.Message}");
        }
    }

    finished = true;
}



        private void HandleUpdate(UpdateResponse update)
        {
            try
            {
                if (update is UpdatedEventsResponse updatedEventsResponse)
                {
                    log.Info($"Received updated events: {updatedEventsResponse.Events?.Count ?? 0}");
                    if (updatedEventsResponse.Events != null) client?.EventEvntriesAdded(updatedEventsResponse.Events);
                }
                else if (update is NewParticipantResponse newParticipantResponse)
                {
                    log.Info($"Received new participant: {newParticipantResponse.Participant}");

                    client?.ParticipantAdded(newParticipantResponse.Participant); 
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error handling update: {ex.Message}");
            }
        }


        private void CloseConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                waitHandle.Close();
                client = null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        public User Login(string username, string password, IMainObserver client)
        {
            this.client = client;
            var request = new LoginRequest(username, password); 
            SendRequest(request);
            IResponse response = ReadResponse();
            if (response is OkResponse okResponse)
            {
                this.client = client;
                log.Debug("Login response parsed successfully!");
                return okResponse.user;
            }

            if (response is ErrorResponse errorResponse)
            {
                ErrorResponse error = (ErrorResponse)response;
                CloseConnection();
                throw new Exception(errorResponse.message);
            }

            return null;
        }

        public void Logout(User user, IMainObserver client)
        {
            SendRequest(new LogoutRequest(user));
            IResponse response = ReadResponse();
            CloseConnection();
            if (response is ErrorResponse errorResponse)
            {
                ErrorResponse error = (ErrorResponse)response;
                throw new Exception(errorResponse.message);
            }
        }

        public List<EventDTO> GetEventsWithParticipantsCount()
        {
            SendRequest(new GetEventsWithParticipantsCountRequest());
            IResponse response = ReadResponse();
            if (response is ErrorResponse errorResponse)
            {
                throw new Exception(errorResponse.message);
            }
            EventsWithParticipantsCountResponse eventsWithParticipantsCountResponse = (EventsWithParticipantsCountResponse)response;
            return eventsWithParticipantsCountResponse.events;
        }

        public List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId)
        {
            SendRequest(new GetParticipantsForEventWithCountRequest(eventId));
            IResponse response = ReadResponse();
            if (response is ErrorResponse errorResponse)
            {
                throw new Exception(errorResponse.message);
            }
            GetParticipantsForEventWithCountResponse getParticipantsForEventWithCountResponse = (GetParticipantsForEventWithCountResponse)response;
            return getParticipantsForEventWithCountResponse.participants;
        }

        public List<Participant> GetAllParticipants()
        {
            SendRequest(new GetAllParticipantsRequest());
            IResponse response = ReadResponse();
    
            if (response is ErrorResponse errorResponse)
            {
                throw new Exception(errorResponse.message);
            }
            else if (response is AllParticipantsResponse allParticipantsResponse) // Verificare corectă
            {
                return allParticipantsResponse.Participants;
            }
            else
            {
                throw new Exception($"Unexpected response type: {response?.GetType().Name}");
            }
        }

        public List<Event> GetAllEvents()
        {
            SendRequest(new GetAllEventsRequest());
            IResponse response = ReadResponse();
            if (response is ErrorResponse errorResponse)
            {
                throw new Exception(errorResponse.message);
            }
            AllEventsResponse allEventsResponse = (AllEventsResponse)response;
            return allEventsResponse.events;
        }

        public void saveEventsEntries(List<Office> newEntry)
        {
            try
            {
                SendRequest(new CreateEventEntriesRequest(newEntry));
                IResponse response = ReadResponse();

                if (response is ErrorResponse errorResponse)
                {
                    throw new Exception(errorResponse.message);
                }
                else if (response is UpdatedEventsResponse updatedEventsResponse)
                {
                    // Process update on a separate thread to avoid deadlock
                    Task.Run(() => HandleUpdate(updatedEventsResponse));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in saveEventsEntries: {ex.Message}");
                throw;
            }
        }
        public void saveParticipant(Participant participant)
        {
            try
            {
                SendRequest(new CreateParticipantRequest(participant));
                IResponse response = ReadResponse();

                // Gestionați corect tipurile de răspuns așteptate
                if (response is ErrorResponse errorResponse)
                {
                    throw new Exception(errorResponse.message);
                }
                else if (response is UpdatedParticipantsResponse updatedParticipantsResponse)
                {
                    // Participantul a fost adăugat cu succes
                    log.Info($"Participant added: {updatedParticipantsResponse.Participants?.Count ?? 0}");
                    Task.Run(() => HandleUpdate(updatedParticipantsResponse));
                }
                else
                {
                    throw new Exception($"Unexpected response type: {response.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in saveParticipants: {ex.Message}");
                throw;
            }
        }
    }
}