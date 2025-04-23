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
            while (!finished)
            {
                log.Info("Client is running...");
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

                // now env.Type tells us what kind of response
                try
                {
                    if (env.Type == nameof(UpdatedEventsResponse))
                    {
                        var update = env.Payload.Deserialize<UpdatedEventsResponse>(jsonOptions);
                        HandleUpdate(update);
                    }
                    else
                    {
                        // concrete non-update response
                        IResponse resp = env.Type switch
                        {
                            nameof(OkResponse)    => env.Payload.Deserialize<OkResponse>(jsonOptions),
                            nameof(ErrorResponse) => env.Payload.Deserialize<ErrorResponse>(jsonOptions),
                            // … add other non-update response types here …
                            _ => throw new Exception($"Unknown response type: {env.Type}")
                        };

                        lock (responses)
                            responses.Enqueue(resp);
                        waitHandle.Set();
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to materialize '{env.Type}': {ex.Message}");
                }
            }

            finished = true;
        }


        private void HandleUpdate(UpdateResponse update)
        {
            switch (update)
            {
                case NewParticipantResponse newParticipantResponse:
                    client.ParticipantAdded(newParticipantResponse.participant);
                    break;
                case UpdatedEventsResponse updatedEventsResponse:
                    client.EventEvntriesAdded(updatedEventsResponse.events);
                    break;
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

        public List<Participant> GetAllParticipants()
        {
            SendRequest(new GetAllParticipantsRequest());
            IResponse response = ReadResponse();
            if (response is ErrorResponse errorResponse)
            {
                throw new Exception(errorResponse.message);
            }
            AllParticipantsResponse allParticipantsResponse = (AllParticipantsResponse)response;
            return allParticipantsResponse.participants;
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
            SendRequest(new CreateEventEntriesRequest(newEntry));
        }

        public void saveParticipant(Participant participant)
        {
            SendRequest(new CreateParticipantRequest(participant));
        }
    }
}