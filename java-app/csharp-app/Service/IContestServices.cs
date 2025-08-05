using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Service;

public interface IContestServices
{
    User Login(string username, string password, IMainObserver client);
    void Logout(User user, IMainObserver client);
    List<EventDTO> GetEventsWithParticipantsCount();
    List<ParticipantDTO> GetParticipantsForEventWithCount(long eventId);
    List<Participant> GetAllParticipants();
    List<Event> GetAllEvents();
    void SaveEventsEntries(List<Office> newEntry);
    void SaveParticipant(Participant participant, IMainObserver sender);
    
}