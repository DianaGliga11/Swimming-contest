using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using log4net;


namespace Networking
{
    public class ServicesProxy : IContestServices
    {
        private readonly string _host;
        private readonly int _port;
        private IMainObserver _clientObserver;
        private TcpClient _connection;
        private NetworkStream _stream;
        private readonly Queue<ResponseJson> _responseQueue = new();
        private volatile bool _finished;
        private readonly AutoResetEvent _responseEvent = new(false);
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServicesProxy));

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: true)
            }        };

        public ServicesProxy(string host, int port)
        {
            this._host = host;
            this._port = port;
        }

        private void EnsureConnected()
        {
            if (_connection != null) return;
            _connection = new TcpClient(_host, _port);
            _stream = _connection.GetStream();
            _finished = false;
            new Thread(RunReader) { IsBackground = true }.Start();
        }

        private void RunReader()
        {
            using var reader = new StreamReader(_stream, Encoding.UTF8, leaveOpen: true);
            while (!_finished)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var msg = JsonSerializer.Deserialize<ResponseJson>(line, JsonOptions);
                Log.Debug($"[proxy reader] ← {line}");
                if (msg.Type == ResponseType.UPDATED_EVENTS || msg.Type == ResponseType.NEW_PARTICIPANT)
                {
                    HandleUpdate(msg);
                }
                else
                {
                    lock (_responseQueue)
                        _responseQueue.Enqueue(msg);
                    _responseEvent.Set();
                }
            }
        }

        private ResponseJson ReadResponse()
        {
            _responseEvent.WaitOne();
            lock (_responseQueue)
            {
                if (_responseQueue.Count > 0)
                    return _responseQueue.Dequeue();
            }

            throw new Exception("No response in queue");
        }

        private void SendRequest(RequestJson req)
        {
            EnsureConnected();
            var json = JsonSerializer.Serialize(req, JsonOptions) + "\n";
            Log.Debug($"[proxy] → {json}");
            var buf = Encoding.UTF8.GetBytes(json);
            _stream.Write(buf, 0, buf.Length);
            _stream.Flush();
        }

        private void HandleUpdate(ResponseJson msg)
        {
            try
            {
                if (msg.Type == ResponseType.UPDATED_EVENTS && msg.Events != null)
                {
                    _clientObserver?.EventEvntriesAdded(msg.Events);
                    Log.Debug($"Received update: Type={msg.Type}, EventCount={msg.Events.Count}");
                }
                else if (msg.Type == ResponseType.NEW_PARTICIPANT && msg.Participant != null)
                    _clientObserver?.ParticipantAdded(msg.Participant);
            }
            catch (Exception ex)
            {
                Log.Error("error in HandleUpdate", ex);
            }
        }

        public User Login(string username, string password, IMainObserver client)
        {
            EnsureConnected();
            _clientObserver = client;
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
            _finished = true;
            _stream.Close();
            _connection.Close();
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
        public void SaveEventsEntries(List<Office> newEntries)
        {
            var req = JsonProtocolUtils.CreateCreateEventEntriesRequest(newEntries); 
            Task.Run(()=>SendRequest(req));
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ERROR)
                throw new Exception(resp.Error);
        }

        public void SaveParticipant(Participant participant, IMainObserver sender)
        {
            var req = JsonProtocolUtils.CreateCreateParticipantRequest(participant);
            Task.Run(()=>SendRequest(req));
            var resp = ReadResponse();
            if (resp.Type == ResponseType.ERROR)
                throw new Exception(resp.Error);
        }
    }
}