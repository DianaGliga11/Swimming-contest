using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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

namespace Networking.ProtocolBuffers;

public class ProtocolBufferWorker : IMainObserver
{
    private readonly IContestServices _server;
    private readonly TcpClient _connection;
    private NetworkStream _stream;
    private volatile bool _connected;
    private static readonly ILog Log = LogManager.GetLogger(typeof(ClientWorker));

    public ProtocolBufferWorker(IContestServices server, TcpClient connection)
    {
        _server = server;
        _connection = connection;

        try
        {
            _stream = _connection.GetStream();
            _connected = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

    public virtual void Run()
    {
        while (_connected)
        {
            try
            {
                SwimmingContestRequest request = SwimmingContestRequest.Parser.ParseDelimitedFrom(_stream);
                SwimmingContestResponse response = HandleRequest(request);

                if (response != null)
                {
                    SendResponse(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        try
        {
            _stream.Close();
            _connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

    private SwimmingContestResponse HandleRequest(SwimmingContestRequest request)
    {
        try
        {
            switch (request.Type)
            {
                case SwimmingContestRequest.Types.Type.Login:
                    Log.Debug("Login request...");
                    string username = ProtocolBuilderUtils.GetUsername(request);
                    string password = ProtocolBuilderUtils.GetPassword(request);
                    try
                    {
                       User user = _server.Login(username, password, this);
                       return ProtocolBuilderUtils.CreateOkResponse(user);
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.Logout:
                    Log.Debug("Logout request...");
                    User deconncectedUser = ProtocolBuilderUtils.GetUser(request);
                    try
                    {
                        _server.Logout(deconncectedUser, this);
                        _connected = false;
                        return ProtocolBuilderUtils.CreateOkResponse(deconncectedUser);
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.CreateParticipant:
                    Log.Debug("Create participant request...");
                    Participant participant = ProtocolBuilderUtils.GetParticipant(request);
                    try
                    {
                        _server.SaveParticipant(participant, this);
                        return ProtocolBuilderUtils.CreateNewParticipantResponse(participant);
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.GetAllParticipants:
                    Log.Debug("Get all participants request...");
                    try
                    {
                        List<Participant> participants = _server.GetAllParticipants();
                        return ProtocolBuilderUtils.CreateAllParticipantsResponse(participants);
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.GetAllEvents:
                    Log.Debug("Get all events request...");
                    try
                    {
                        List<Event> events = _server.GetAllEvents();
                        return ProtocolBuilderUtils.CreateAllEventsResponse(events);
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.GetEventsWithParticipantsCount:
                    Log.Debug("Get events with count request...");
                    try
                    {
                        List<EventDTO> events = _server.GetEventsWithParticipantsCount();
                        return ProtocolBuilderUtils.CreateEventsWithParticipantsCountResponse(events);
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.GetParticipantsForEventWithCount:
                    Log.Debug("Get participants for event request...");
                    try
                    {
                        long eventId = request.EventId;
                        List<ParticipantDTO> participants = _server.GetParticipantsForEventWithCount(eventId);
                        return ProtocolBuilderUtils.CreateParticipantsForEventWithCountResponse(participants);
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case SwimmingContestRequest.Types.Type.CreateEventEntry:
                    Log.Debug("Create event entries request...");
                    List<Office> eventEntries = ProtocolBuilderUtils.GetEventEntries(request);
                    try
                    {
                        _server.SaveEventsEntries(eventEntries);
                        return ProtocolBuilderUtils.CreateUpdatedEventsResponse(
                            _server.GetEventsWithParticipantsCount());
                    }catch(Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                default:
                    throw new InvalidOperationException($"Unsupported request type: {request.Type}");
            }
        }
        catch (Exception ex)
        {
            Log.Error("HandleRequest error", ex);
            return ProtocolBuilderUtils.CreateErrorResponse(ex.Message);
        }
    }

    private void SendResponse(SwimmingContestResponse response)
    {
        lock (_stream)
        {
            response.WriteDelimitedTo(_stream);
            _stream.Flush();
            Log.Debug($"Sent response of type: {response.Type}");
        }
    }
    public void ParticipantAdded(Participant participant)
    {
        Log.Debug($"Observer: participant added {participant.Name}");
        SendResponse(ProtocolBuilderUtils.CreateNewParticipantResponse(participant));
    }

    public void EventEvntriesAdded(List<EventDTO> updatedEvents)
    {
        Log.Debug($"Observer: events updated ({updatedEvents.Count})");
        SendResponse(ProtocolBuilderUtils.CreateUpdatedEventsResponse(updatedEvents));
    }
    
}