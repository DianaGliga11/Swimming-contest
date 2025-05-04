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
        private IMainObserver clientObserver;
        private TcpClient connection;
        private NetworkStream stream;
        private Queue<ResponseJson> responseQueue = new();
        private volatile bool finished;
        private AutoResetEvent responseEvent = new(false);
        private static readonly ILog log = LogManager.GetLogger(typeof(ServicesProxy));

        static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: true)
            }        };

        public ServicesProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        private void EnsureConnected()
        {
            if (connection != null) return;
            connection = new TcpClient(host, port);
            stream = connection.GetStream();
            finished = false;
            new Thread(RunReader) { IsBackground = true }.Start();
        }

        private void RunReader()
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            while (!finished)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var msg = JsonSerializer.Deserialize<ResponseJson>(line, jsonOptions);
                log.Debug($"[proxy reader] ← {line}");
                if (msg.Type == ResponseType.UPDATED_EVENTS || msg.Type == ResponseType.NEW_PARTICIPANT)
                {
                    HandleUpdate(msg);
                }
                else
                {
                    lock (responseQueue)
                        responseQueue.Enqueue(msg);
                    responseEvent.Set();
                }
            }
        }

        private ResponseJson ReadResponse()
        {
            responseEvent.WaitOne();
            lock (responseQueue)
            {
                if (responseQueue.Count > 0)
                    return responseQueue.Dequeue();
            }

            throw new Exception("No response in queue");
        }

        private void SendRequest(RequestJson req)
        {
            EnsureConnected();
            var json = JsonSerializer.Serialize(req, jsonOptions) + "\n";
            log.Debug($"[proxy] → {json}");
            var buf = Encoding.UTF8.GetBytes(json);
            stream.Write(buf, 0, buf.Length);
            stream.Flush();
        }

        private void HandleUpdate(ResponseJson msg)
        {
            try
            {
                if (msg.Type == ResponseType.UPDATED_EVENTS && msg.Events != null)
                {
                    clientObserver?.EventEvntriesAdded(msg.Events);
                    log.Debug($"Received update: Type={msg.Type}, EventCount={msg.Events.Count}");
                }
                else if (msg.Type == ResponseType.NEW_PARTICIPANT && msg.Participant != null)
                    clientObserver?.ParticipantAdded(msg.Participant);
            }
            catch (Exception ex)
            {
                log.Error("error in HandleUpdate", ex);
            }
        }

        public User Login(string username, string password, IMainObserver client)
        {
            EnsureConnected();
            clientObserver = client;
            var req = JsonProtocolUtils.CreateLoginRequest(username, password);
            SendRequest(req);
            var resp = ReadResponse();
            if (resp.Type == ResponseType.OK)
                return resp.User!;
            throw new Exception(resp.Error ?? "Login failed");
        }

        public void Logout(User user, IMainObserver client)
        {
            var req = JsonProtocolUtils.CreateLogoutRequest(user);
            SendRequest(req);
            var resp = ReadResponse();
            finished = true;
            stream.Close();
            connection.Close();
            if (resp.Type == ResponseType.ERROR)
                throw new Exception(resp.Error);
        }

        public List<EventDTO> GetEventsWithParticipantsCount()
        {
            var req = JsonProtocolUtils.CreateGetEventsWithParticipantsCountRequest();
            SendRequest(req);
            var resp = ReadResponse();
            if (resp.Type == ResponseType.EVENTS_WITH_PARTICIPANTS_COUNT)
                return resp.Events ?? new List<EventDTO>();
            throw new Exception(resp.Error);
        }

        public List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId)
        {
            var req = JsonProtocolUtils.CreateGetParticipantsForEventWithCountRequest(eventId);
            SendRequest(req);
            var resp = ReadResponse();
            if (resp.Type == ResponseType.GET_PARTICIPANTS_FOR_EVENT_WITH_COUNT)
                return resp.Participants ?? new List<ParticipantDTO>();
            throw new Exception(resp.Error);
        }

        public List<Participant> GetAllParticipants()
        {
            var req = JsonProtocolUtils.CreateGetAllParticipantsRequest();
            SendRequest(req);
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ALL_PARTICIPANTS)
                return resp.ParticipantsRaw ?? new List<Participant>();
            throw new Exception(resp.Error);
        }

        public List<Event> GetAllEvents()
        {
            var req = JsonProtocolUtils.CreateGetAllEventsRequest();
            SendRequest(req);
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ALL_EVENTS)
                return resp.EventsRaw ?? new List<Event>();
            throw new Exception(resp.Error);
        }
        public void saveEventsEntries(List<Office> newEntries)
        {
            var req = JsonProtocolUtils.CreateCreateEventEntriesRequest(newEntries); 
            Task.Run(()=>SendRequest(req));
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ERROR)
                throw new Exception(resp.Error);
        }

        public void saveParticipant(Participant participant, IMainObserver sender)
        {
            var req = JsonProtocolUtils.CreateCreateParticipantRequest(participant);
            Task.Run(()=>SendRequest(req));
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ERROR)
                throw new Exception(resp.Error);
        }
    }
}