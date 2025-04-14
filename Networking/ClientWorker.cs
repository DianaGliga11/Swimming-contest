using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using log4net;
using Networking.Request;
using Networking.Response;

namespace Networking
{

    public class ClientWorker : IMainObserver
    {
        private IContestServices server;
        private TcpClient connection;
        private NetworkStream stream;
        private readonly JsonSerializerOptions jsonOptions;
        private volatile bool connected;
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientWorker));


        public ClientWorker(IContestServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                connected = true;
                jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = false};
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
        
        public virtual void Run()
        {
            while (connected)
            {
                try
                {
                    string requestJson = ReadJsonFromStream();
                    if (string.IsNullOrWhiteSpace(requestJson))
                    {
                        continue;
                    }

                    IRequest request = DeserializeRequest(requestJson);
                    
                    IResponse response = HandleRequest(request);
                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception in Run: " + ex.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    log.Error("Exception in Run: " + ex.StackTrace);
                }
            }
        }

        private IResponse HandleRequest(IRequest request)
        {
            if (request is LoginRequest loginRequest)
            {
                log.Info("Login request ... ");

                if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return new ErrorResponse("Username and password are required");
                }

                try
                {
                    lock (server)
                    {
                        User user = server.Login(loginRequest.Username, loginRequest.Password, this);
                        return new OkResponse(user);
                    }
                }
                catch (Exception exception)
                {
                    connected = false;
                    return new ErrorResponse(exception.Message);
                }
            }
            
            if (request is LogoutRequest logoutRequest)
            {
                log.Info("Logout request");
                try
                {
                    lock (server)
                    {
                        server.Logout(logoutRequest.user, this);
                    }

                    connected = false;
                    return new OkResponse(null);
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }
            
            if (request is CreateParticipantRequest createParticipantRequest)
            {
                log.Info("Create participant request");
                try
                {
                    lock (server)
                    {
                        server.saveParticipant(createParticipantRequest.participant);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }

            if (request is CreateEventEntriesRequest createEventEntriesRequest)
            {
                log.Info("Create events request");
                try
                {
                    lock (server)
                    {
                        server.saveEventsEntries(createEventEntriesRequest.eventEntries);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }
            
            if (request is GetAllParticipantsRequest)
            {
                log.Info("Get all participants");
                try
                {
                    lock (server)
                    {
                        var result = server.GetAllParticipants();
                        return new AllParticipantsResponse(result);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }

            if (request is GetAllEventsRequest getAllEventsRequest)
            {
                log.Info("Get all events");
                try
                {
                    lock (server)
                    {
                        var result = server.GetAllEvents();
                        return new AllEventsResponse(result);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }

            if (request is GetEventsWithParticipantsCountRequest getEventsWithParticipantsCountRequest)
            {
                log.Info("Get events with participant count");
                try
                {
                    lock (server)
                    {
                        var result = server.GetEventsWithParticipantsCount();
                        return new EventsWithParticipantsCountResponse(result);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse(exception.Message);
                }
            }

            return null;
        }


        private string ReadJsonFromStream()
        {
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            string json = reader.ReadLine();
            log.Info("Received JSON: " + json);
            return json;
        }
       
        private IRequest DeserializeRequest(string json)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                string type = document.RootElement.GetProperty("Type").GetString();
                JsonElement payload = document.RootElement.GetProperty("Payload");

                return type switch
                {
                    nameof(LoginRequest) => payload.Deserialize<LoginRequest>(jsonOptions),
                    nameof(LogoutRequest) => payload.Deserialize<LogoutRequest>(jsonOptions),
                    nameof(CreateParticipantRequest) => payload.Deserialize<CreateParticipantRequest>(jsonOptions),
                    nameof(CreateEventEntriesRequest) => payload.Deserialize<CreateEventEntriesRequest>(jsonOptions),
                    nameof(GetAllParticipantsRequest) => payload.Deserialize<GetAllParticipantsRequest>(jsonOptions),
                    nameof(GetAllEventsRequest) => payload.Deserialize<GetAllEventsRequest>(jsonOptions),
                    nameof(GetEventsWithParticipantsCountRequest) => payload.Deserialize<GetEventsWithParticipantsCountRequest>(jsonOptions),
                    _ => throw new InvalidOperationException("Unknown request type: " + type)
                };
            }
            catch (Exception ex)
            {
                log.Error("Failed to deserialize request: " + ex.Message);
                throw;
            }
        }

        
        private void SendResponse(IResponse response)
        {
            var wrapper = new
            {
                Type = response.GetType().Name,
                Payload = response
            };

            string json = JsonSerializer.Serialize(wrapper, jsonOptions) + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

        
        public void ParticipantAdded(Participant participant)
        {
            log.Info("Participant added: " + participant.Name);
            try
            {
                SendResponse(new NewParticipantResponse(participant));
            }
            catch (Exception ex)
            {
                log.Error("Exception in ParticipantAdded: " + ex.StackTrace);
            }
        }

        public void EventEvntriesAdded(List<EventDTO> events)
        {
            log.Info("Events added: " + events.Count);
            try
            {
                SendResponse(new UpdatedEventsResponse(events));
            }
            catch (Exception ex)
            {
                log.Error("Exception in EventEvntriesAdded: " + ex.StackTrace);
            }
        }
    }
}