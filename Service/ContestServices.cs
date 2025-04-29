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
        IMainObserver localClients = loggedClients[user.UserName];
        if (loggedClients == null)
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
        // Salvare în baza de date
        foreach (Office entry in newEntries)
        {
            eventService.saveEventEntry(entry);
        }
    
        // Obține datele actualizate
        var updatedEvents = GetEventsWithParticipantsCount();
    
        // Notifică TOȚI clienții
        NotifyAllClientsAboutEvents(updatedEvents);
    }

    private void NotifyAllClientsAboutEvents(List<EventDTO> events)
    {
        List<string> clientsToRemove = new();

        foreach (var entry in loggedClients)
        {
            var username = entry.Key;
            var observer = entry.Value;

            try
            {
                observer.EventEvntriesAdded(events);
            }
            catch (Exception ex)
            {
                log.Error($"Error notifying client '{username}': {ex.Message}");
                clientsToRemove.Add(username);
            }
        }

        // Scoate clienții care au dat eroare
        foreach (var user in clientsToRemove)
        {
            loggedClients.Remove(user);
        }
    }
    public void saveParticipant(List<Participant> participants)
    {
        // Salvați doar ultimul participant (dacă asta e comportamentul dorit)
        Participant newParticipant = participants.Last();
        participantService.add(newParticipant);
    
        // Obțineți lista actualizată de participanți
        List<Participant> updatedParticipants = GetAllParticipants();
    
        // Notificați TOȚI clienții conectați
        NotifyAllClients(updatedParticipants);
    }

    private void NotifyAllClients(List<Participant> participants)
    {
        List<IMainObserver> failedClients = new List<IMainObserver>();
    
        foreach (var clientEntry in loggedClients)
        {
            try
            {
                clientEntry.Value.ParticipantAdded(participants);
            }
            catch (Exception e)
            {
                log.Error($"Failed to notify client {clientEntry.Key}: {e.Message}");
                failedClients.Add(clientEntry.Value);
            }
        }
    
        // Ștergeți clienții care nu mai răspund
        foreach (var failedClient in failedClients)
        {
            loggedClients.Remove(loggedClients.FirstOrDefault(x => x.Value == failedClient).Key);
        }
    }
}
