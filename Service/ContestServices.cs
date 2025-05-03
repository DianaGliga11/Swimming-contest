using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using log4net;

namespace Service;

public class ContestServices : IContestServices
{
    private readonly I_UserService userService;
    private readonly I_ParticipantService participantService;
    private readonly I_EventService eventService;
    private readonly IDictionary<string, IMainObserver> loggedClients;
    private static readonly ILog log = LogManager.GetLogger(typeof(ContestServices));
    
    public ContestServices(I_UserService userService, I_ParticipantService participantService, I_EventService eventService)
    {
        this.eventService = eventService;
        this.participantService = participantService;
        this.userService = userService;
        loggedClients = new Dictionary<string, IMainObserver>();
    }

    public User Login(string username, string password, IMainObserver client)
    {
        User? foundUser = userService.getLogin(username, password);
        if (foundUser != null)
        {
            if (loggedClients.ContainsKey(foundUser.UserName))
            {
                log.Error("User already log in");
                throw new Exception("User already logged in.");
            }
            loggedClients[foundUser.UserName] = client;
            return foundUser;
        }
        else
        {
            log.Error("Authentication failed");
            throw new Exception("Authentication failed.");
        }
    }

    public void Logout(User user, IMainObserver client)
    {
        if (!loggedClients.TryGetValue(user.UserName, out var localClient))
        {
            log.Error("User is not logged in");
            throw new Exception("User is not logged in.");
        }

        loggedClients.Remove(user.UserName);
    }

    public List<EventDTO> GetEventsWithParticipantsCount()
    {
        return eventService.getEventsWithParticipantsCount().ToList();
    }

    public List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId)
    {
        return eventService.getParticipantsForEventWithCount(eventId).ToList();
    }

    public List<Participant> GetAllParticipants()
    {
        return participantService.getAll().ToList();
    }

    public List<Event> GetAllEvents()
    {
        return eventService.getAll().ToList();
    }

    public void saveEventsEntries(List<Office> newEntries)
    {
        foreach (Office entry in newEntries)
        {
            eventService.saveEventEntry(entry);
        }

        var updatedEvents=GetEventsWithParticipantsCount();
        foreach (IMainObserver client in loggedClients.Values)
        {
           client.EventEvntriesAdded(updatedEvents);
        }
    }
    public void saveParticipant(Participant participant, IMainObserver sender)
    {
        participantService.add(participant);
        foreach (IMainObserver client in loggedClients.Values)
        {
            if (client != sender)
            {
                Task.Run(() => client.ParticipantAdded(participant));
            }
        }
    }
}
