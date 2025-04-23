using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using log4net;
using Networking.Networking;
using Networking.Request;
using Networking.Response;

namespace Networking
{

    public class ClientWorker : IMainObserver
    {
        private IContestServices server;
        private TcpClient connection;
        private NetworkStream stream;
        //private readonly JsonSerializerOptions jsonOptions;
        private volatile bool connected;
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientWorker));

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter() }
        };

        public ClientWorker(IContestServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                connected = true;
                //jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = false};
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
        
        public void Run()
        {
            try
            {
                log.Info("Client Worker running...");
                using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                while (connected)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // parse envelope
                    JsonEnvelope env = JsonSerializer.Deserialize<JsonEnvelope>(line, jsonOptions);

                    // dispatch to concrete IRequest
                    IRequest request = env.Type switch
                    {
                        nameof(LoginRequest) => env.Payload.Deserialize<LoginRequest>(jsonOptions),
                        nameof(LogoutRequest) => env.Payload.Deserialize<LogoutRequest>(jsonOptions),
                        nameof(GetAllEventsRequest) => env.Payload.Deserialize<GetAllEventsRequest>(jsonOptions), 
                        nameof(GetAllParticipantsRequest) => env.Payload.Deserialize<GetAllParticipantsRequest>(jsonOptions),
                        nameof(GetEventsWithParticipantsCountRequest) => env.Payload.Deserialize<GetEventsWithParticipantsCountRequest>(jsonOptions),
                        nameof(CreateParticipantRequest) => env.Payload.Deserialize<CreateParticipantRequest>(jsonOptions),
                        nameof(CreateEventEntriesRequest) => env.Payload.Deserialize<CreateEventEntriesRequest>(jsonOptions),
                        nameof(CreateEventRequest) => env.Payload.Deserialize<CreateEventRequest>(jsonOptions),
                        nameof(GetParticipantsForEventWithCountRequest) => env.Payload.Deserialize<GetParticipantsForEventWithCountRequest>(jsonOptions),
                        _ => throw new Exception($"Unknown request type: {env.Type}")
                    };

                    // handle it
                    IResponse response = HandleRequest(request);

                    // send back in the same envelope form
                    string payloadJson = JsonSerializer.Serialize(response, response.GetType(), jsonOptions);
                    var payloadElement = JsonDocument.Parse(payloadJson).RootElement;
                    var outEnv = new { type = response.GetType().Name, payload = payloadElement };
                    string outLine = JsonSerializer.Serialize(outEnv, jsonOptions) + "\n";
                    byte[] outBytes = Encoding.UTF8.GetBytes(outLine);
                    stream.Write(outBytes, 0, outBytes.Length);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                log.Error("ClientWorker.Run: " + e.Message);
            }
            finally
            {
                log.Info("Client Worker closing connection...");
                stream?.Close();
                connection?.Close();
            }
        }
        
        private IResponse HandleRequest(IRequest request)
        {
            try
            {
                log.Info($"Handling request of type: {request.GetType().Name}");

                if (request is LoginRequest loginRequest)
                {
                    log.Info("Processing login request...");
            
                    if (string.IsNullOrEmpty(loginRequest.Username) || 
                        string.IsNullOrEmpty(loginRequest.Password))
                    {
                        return new ErrorResponse("Username and password are required");
                    }

                    lock (server)
                    {
                        User user = server.Login(loginRequest.Username, loginRequest.Password, this);
                        if (user == null)
                        {
                            return new ErrorResponse("Authentication failed");
                        }
                
                        // Asigură-te că obiectul User este complet populat
                        log.Debug($"User to return: {JsonSerializer.Serialize(user)}");
                        return new OkResponse(user);
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
            }
            catch (Exception ex)
            {
                log.Error("Request handling failed", ex);
                return new ErrorResponse(ex.Message);
            }

            return new ErrorResponse("Unsupported request type");
        }
 

        private void SendResponse(IResponse response)
        {
            var envelope = new {
                Type    = response.GetType().Name,
                Payload = response
            };
            string json = JsonSerializer.Serialize(envelope, jsonOptions) + "\n";
            log.Debug($"Sending response: {json}");
            byte[] data = Encoding.UTF8.GetBytes(json);
            stream.Write(data, 0, data.Length);
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