using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Networking.Response;
using Service;
using log4net;
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
            lock (syncLock)
            {
                waitHandle.WaitOne();
                lock (responses)
                {
                    return responses.Dequeue();
                }
            }
        }

        private void SendRequest(IRequest request)
        {
            lock (syncLock)
            {
                var wrapper = new
                {
                    Type = request.GetType().Name,
                    Payload = request
                };
                string json = JsonSerializer.Serialize(wrapper) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                stream.Write(data, 0, data.Length);
            }
        }
        private void StartReader()
        {
            Thread threadWorker = new Thread(Run);
            threadWorker.Start();
        }

        public void Run()
        {
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            while (!finished)
            {
                try
                {
                    string json = reader.ReadLine();
                    if (!string.IsNullOrEmpty(json))
                    {
                        IResponse response = null;
                        try
                        {
                            response = DeserializeResponse(json);
                        }
                        catch (Exception ex)
                        {
                            log.Error($"Deserialization failed: {ex.Message}");
                            continue; // Salt peste răspuns invalid
                        }

                        if (response is UpdateResponse updateResponse)
                        {
                            HandleUpdate(updateResponse);
                        }
                        else
                        {
                            lock (responses)
                            {
                                responses.Enqueue(response);
                            }

                            waitHandle.Set();
                        }
                    }
                }
                catch (IOException ioex)
                {
                    log.Error("Connection closed: " + ioex.Message);
                    finished = true;
                }
                catch (Exception ex)
                {
                    log.Error("Reading error: " + ex.Message);
                }
            }
        }


        private IResponse DeserializeResponse(string json)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                var root = document.RootElement;

                if (!root.TryGetProperty("Type", out var typeProp) || typeProp.ValueKind != JsonValueKind.String)
                {
                    throw new Exception("Invalid or missing 'Type' property");
                }

                string type = typeProp.GetString();
                if (!root.TryGetProperty("Payload", out var payload))
                {
                    throw new Exception("Missing 'Payload' property");
                }

                return type switch
                {
                    nameof(OkResponse) => JsonSerializer.Deserialize<OkResponse>(payload.GetRawText()),
                    nameof(ErrorResponse) => JsonSerializer.Deserialize<ErrorResponse>(payload.GetRawText()),
                    nameof(NewParticipantResponse) => JsonSerializer.Deserialize<NewParticipantResponse>(payload.GetRawText()),
                    // ... adaugă toate celelalte tipuri de răspuns
                    _ => throw new Exception($"Unknown response type: {type}")
                };
            }
            catch (Exception ex)
            {
                log.Error($"Failed to deserialize response. JSON: {json}, Error: {ex.Message}");
                throw;
            }
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