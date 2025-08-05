using System;
using System.Net.Sockets;
using Google.Protobuf;
using log4net;
using Org.Example.Protocolbuffers;
using Service;

using Participant = mpp_proiect_csharp_DianaGliga11.Model.Participant;
using User = mpp_proiect_csharp_DianaGliga11.Model.User;
using Event = mpp_proiect_csharp_DianaGliga11.Model.Event;
using EventDTO = mpp_proiect_csharp_DianaGliga11.Model.DTO.EventDTO;
using ParticipantDTO = mpp_proiect_csharp_DianaGliga11.Model.DTO.ParticipantDTO;
using Office = mpp_proiect_csharp_DianaGliga11.Model.Office;

namespace Networking.ProtocolBuffers
{
    public class ProtocolBufferServicesProxy : IContestServices
    {

        private readonly string _host;
        private readonly int _port;
        private IMainObserver _clientObserver;
        private TcpClient _connection;
        private NetworkStream _stream;
        private readonly Queue<SwimmingContestResponse> _responseQueue = new();
        private volatile bool _finished;

        private EventWaitHandle _waitHandle;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ServicesProxy));

        public ProtocolBufferServicesProxy(string host, int port)
        {
            _host = host;
            _port = port;
            _waitHandle = new AutoResetEvent(false);
            _responseQueue = new Queue<SwimmingContestResponse>();
        }

        private void EnsureConnected()
        {
            if (_connection?.Connected == true) return;

            try
            {
                _connection = new TcpClient(_host, _port);
                _stream = _connection.GetStream();
                _finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
                Log.Info($"Connected to {_host}:{_port}");
            }
            catch (Exception ex)
            {
                Log.Error($"Connection failed: {ex.Message}");
                throw;
            }
        }


        private void RunReader()
        {
            while (!_finished)
            {
                try
                {
                    SwimmingContestResponse response = SwimmingContestResponse.Parser.ParseDelimitedFrom(_stream);
                    Log.Info("Response received: " + response);
                    if (IsUpdateResponse(response))
                    {
                        HandleUpdate(response);
                    }
                    else
                    {
                        lock (_responseQueue)
                        {
                            _responseQueue.Enqueue(response);
                        }

                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error in Run Client");
                }
            }
        }

        private bool IsUpdateResponse(SwimmingContestResponse response)
        {
            return response.Type == SwimmingContestResponse.Types.Type.NewParticipant
                   || response.Type == SwimmingContestResponse.Types.Type.UpdatedEvents
                   || response.Type == SwimmingContestResponse.Types.Type.NewEvent;
        }

        private SwimmingContestResponse ReadResponse()
        {
            SwimmingContestResponse response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (_responseQueue)
                {
                    response = _responseQueue.Dequeue();
                }
            }
            catch (Exception e)
            {
                Log.Error("Error while reading response");
            }

            return response;
        }

        private void SendRequest(SwimmingContestRequest req)
        {
            try
            {
                Log.Debug($"Sending {req.Type} request");
                req.WriteDelimitedTo(_stream);
                _stream.Flush();
            }
            catch (Exception ex)
            {
                Log.Error($"SendRequest failed: {ex.Message}");
                CloseConnection();
                throw;
            }
        }
        private void HandleUpdate(SwimmingContestResponse msg)
        {
            if (IsUpdateResponse(msg))
            {
                try
                {
                    switch (msg.Type)
                    {
                        case SwimmingContestResponse.Types.Type.NewParticipant:
                            _clientObserver.ParticipantAdded(ProtocolBuilderUtils.GetParticipant(msg));
                            break;
                        case SwimmingContestResponse.Types.Type.NewEvent:
                            break;
                        case SwimmingContestResponse.Types.Type.UpdatedEvents:
                            _clientObserver.EventEvntriesAdded(ProtocolBuilderUtils.GetEventDTOs(msg));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        
        public User Login(string username, string password, IMainObserver client)
        {
            try
            {
                EnsureConnected();
                _clientObserver = client;

                var request = new SwimmingContestRequest
                {
                    Type = SwimmingContestRequest.Types.Type.Login,
                    UserName = username,
                    Password = password
                };

                Log.Debug($"Sending login request for {username}");
                SendRequest(request);
            
                var response = ReadResponse();
                Log.Debug($"Received response type: {response?.Type}");

                if (response == null)
                    throw new Exception("Null response from server");

                if (response.Type == SwimmingContestResponse.Types.Type.Error)
                    throw new Exception(response.ErrorMessage);

                if (response.User == null)
                    throw new Exception("User data missing in response");

                return new User(response.User.UserName, response.User.Password) 
                { 
                    Id = response.User.Id 
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Login failed: {ex.Message}");
                CloseConnection();
                throw;
            }
        }

        private void CloseConnection()
        {
            _finished = true;
            try
            {
                _stream.Close();
                _connection.Close();
                _waitHandle.Close();
            }
            catch (Exception e)
            {
                Log.Error("Error while closing connection");
            }
        }

        private void StartReader()
        {
            Thread worker = new Thread(RunReader);
            worker.Start();
        }

        public void Logout(User user, IMainObserver client)
        {
            SwimmingContestRequest request = ProtocolBuilderUtils.CreateLogoutRequest(user);
            SendRequest(request);

            SwimmingContestResponse response = ReadResponse();
            CloseConnection();

            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while logging out");
            }
        }

        public List<EventDTO> GetEventsWithParticipantsCount()
        {
            SwimmingContestRequest request
                = ProtocolBuilderUtils.CreateGetEventsWithParticipantsCountRequest();
            SendRequest(request);

            SwimmingContestResponse response = ReadResponse();
            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while getting events with participant count");
            }

            return ProtocolBuilderUtils.GetEventDTOs(response);
        }

        public List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId)
        {
            SwimmingContestRequest request = ProtocolBuilderUtils.CreateGetParticipantsForEventWithCountRequest(eventId);
            SendRequest(request);
            SwimmingContestResponse response = ReadResponse();
            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while getting events with participant count");
            }
            return ProtocolBuilderUtils.GetParticipantDTOs(response);
        }

        public List<Participant> GetAllParticipants()
        {
            SwimmingContestRequest request = ProtocolBuilderUtils.CreateGetAllParticipantsRequest();
            SendRequest(request);

            SwimmingContestResponse response = ReadResponse();
            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while getting participants");
            }

            return ProtocolBuilderUtils.GetParticipants(response);
        }

        public List<Event> GetAllEvents()
        {
            SwimmingContestRequest request = ProtocolBuilderUtils.CreateGetAllEventsRequest();
            SendRequest(request);

            SwimmingContestResponse response = ReadResponse();
            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while getting participants");
            }

            return ProtocolBuilderUtils.GetEvents(response);
        }

        public void SaveEventsEntries(List<Office> newEntries)
        {
            SwimmingContestRequest request = ProtocolBuilderUtils.CreateCreateEventEntryRequest(newEntries);
            SendRequest(request);
            SwimmingContestResponse response = ReadResponse();
            if (response.Type == SwimmingContestResponse.Types.Type.Error)
            {
                Log.Error("Error while saving events entries");
            }
        }

        public void SaveParticipant(Participant participant, IMainObserver sender)
        {
            try
            {
                var request = ProtocolBuilderUtils.CreateCreateParticipantRequest(participant);
                SendRequest(request);
                var response = ReadResponse();
            
                if (response.Type == SwimmingContestResponse.Types.Type.Error)
                    throw new Exception(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error($"SaveParticipant failed: {ex.Message}");
                throw;
            }
        }

    }
}