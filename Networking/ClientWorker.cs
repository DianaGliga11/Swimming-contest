using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using Networking;

namespace Networking
{
    public class ClientWorker : IMainObserver
    {
        private readonly IContestServices _server;
        private readonly TcpClient _connection;
        private NetworkStream _stream;
        private volatile bool _connected;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientWorker));

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public ClientWorker(IContestServices server, TcpClient connection)
        {
            this._server = server;
            this._connection = connection;
            try
            {
                _stream = connection.GetStream();
                _connected = true;
            }
            catch (Exception e)
            {
                Log.Error("Constructor error", e);
            }
        }

        public void Run()
        {
            using var reader = new StreamReader(_stream, Encoding.UTF8);
            while (_connected)
            {
                try
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    Log.Debug($"Received JSON request: {line}");
                    var request = JsonSerializer.Deserialize<RequestJson>(line, JsonOptions);
                    var response = HandleRequest(request);
                    if (response != null)
                        SendResponse(response);
                }
                catch (Exception e)
                {
                    Log.Error("Run error", e);
                }
                Thread.Sleep(100);
            }

            try
            {
                _stream.Close();
                _connection.Close();
            }
            catch (Exception e)
            {
                Log.Error("Error closing connection", e);
            }
        }

        private ResponseJson HandleRequest(RequestJson request)
        {
            try
            {
                switch (request.Type)
                {
                    case RequestType.LOGIN:
                        Log.Debug("Login request...");
                        var usr = request.User;
                        _server.Login(usr.UserName, usr.Password, this);
                        return JsonProtocolUtils.CreateOkResponse(usr);

                    case RequestType.LOGOUT:
                        Log.Debug("Logout request...");
                        _server.Logout(request.User, this);
                        _connected = false;
                        return JsonProtocolUtils.CreateOkResponse();

                    case RequestType.CREATE_PARTICIPANT:
                        Log.Debug("Create participant request...");
                        _server.saveParticipant(request.Participant, this);
                        return JsonProtocolUtils.CreateNewParticipantResponse(request.Participant);

                    case RequestType.GET_ALL_PARTICIPANTS:
                        Log.Debug("Get all participants request...");
                        var parts = _server.GetAllParticipants();
                        return JsonProtocolUtils.CreateAllParticipantsResponse(parts);

                    case RequestType.GET_ALL_EVENTS:
                        Log.Debug("Get all events request...");
                        var evs = _server.GetAllEvents();
                        return JsonProtocolUtils.CreateAllEventsResponse(evs);

                    case RequestType.GET_EVENTS_WITH_PARTICIPANTS_COUNT:
                        Log.Debug("Get events with count request...");
                        var evCount = _server.GetEventsWithParticipantsCount();
                        return JsonProtocolUtils.CreateEventsWithParticipantsCountResponse(evCount);

                    case RequestType.GET_PARTICIPANTS_FOR_EVENT_WITH_COUNT:
                        Log.Debug("Get participants for event request...");
                        var pForEv = _server.GetParticipantsForEventWithCount(request.EventId.Value);
                        return JsonProtocolUtils.CreateGetParticipantsForEventWithCountResponse(pForEv);

                    case RequestType.CREATE_EVENT_ENTRIES:
                        Log.Debug("Create event entries request...");
                        _server.saveEventsEntries(request.EventEntries);
                        return JsonProtocolUtils.CreateOkResponse();

                    default:
                        throw new InvalidOperationException($"Unsupported request type: {request.Type}");
                }
            }
            catch (Exception ex)
            {
                Log.Error("HandleRequest error", ex);
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }

        private void SendResponse(ResponseJson response)
        {
            var json = JsonSerializer.Serialize(response, JsonOptions);
            Log.Debug($"Sending response: {json}");
            lock (_stream)
            {
                var data = Encoding.UTF8.GetBytes(json + "\n");
                _stream.Write(data, 0, data.Length);
                _stream.Flush();
            }
        }
        public void ParticipantAdded(Participant participant)
        {
            Log.Debug($"Observer: participant added {participant.Name}");
            SendResponse(JsonProtocolUtils.CreateNewParticipantResponse(participant));
        }

        public void EventEvntriesAdded(List<EventDTO> updatedEvents)
        {
            Log.Debug($"Observer: events updated ({updatedEvents.Count})");
            SendResponse(JsonProtocolUtils.CreateUpdatedEventsResponse(updatedEvents));
        }
    }
}
