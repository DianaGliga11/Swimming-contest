using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using log4net;

namespace Service;

public class ContestServices : IContestServices
{
    private readonly I_UserService _userService;
    private readonly I_ParticipantService _participantService;
    private readonly I_EventService _eventService;
    private readonly IDictionary<string, IMainObserver> _loggedClients;
    private static readonly ILog Log = LogManager.GetLogger(typeof(ContestServices));

    public ContestServices(I_UserService userService, I_ParticipantService participantService, I_EventService eventService)
    {
        this._eventService = eventService;
        this._participantService = participantService;
        this._userService = userService;
        _loggedClients = new Dictionary<string, IMainObserver>();
    }

    public User Login(string username, string password, IMainObserver client)
    {
        User? foundUser = _userService.getLogin(username, password);
        if (foundUser != null)
        {
            if (_loggedClients.ContainsKey(foundUser.UserName))
            {
                Log.Error("User already log in");
                throw new Exception("User already logged in.");
            }
            _loggedClients[foundUser.UserName] = client;
            return foundUser;
        }
        else
        {
            Log.Error("Authentication failed");
            throw new Exception("Authentication failed.");
        }
    }

    public void Logout(User user, IMainObserver client)
    {
        if (!_loggedClients.TryGetValue(user.UserName, out var localClient))
        {
            Log.Error("User is not logged in");
            throw new Exception("User is not logged in.");
        }

        _loggedClients.Remove(user.UserName);
    }

    public List<EventDTO> GetEventsWithParticipantsCount()
    {
        return _eventService.getEventsWithParticipantsCount().ToList();
    }

    public List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId)
    {
        return _eventService.getParticipantsForEventWithCount(eventId).ToList();
    }

    public List<Participant> GetAllParticipants()
    {
        return _participantService.getAll().ToList();
    }

    public List<Event> GetAllEvents()
    {
        return _eventService.getAll().ToList();
    }

    public void SaveEventsEntries(List<Office> newEntries)
    {
        foreach (Office entry in newEntries)
        {
            _eventService.saveEventEntry(entry);
        }

        var updatedEvents= GetEventsWithParticipantsCount();
        foreach (IMainObserver client in _loggedClients.Values)
        {
           client.EventEvntriesAdded(updatedEvents);
        }
    }
    public void SaveParticipant(Participant participant, IMainObserver sender)
    {
        _participantService.add(participant);
        foreach (IMainObserver client in _loggedClients.Values)
        {
            if (client != sender)
            {
               client.ParticipantAdded(participant);
            }
        }
    }
}
