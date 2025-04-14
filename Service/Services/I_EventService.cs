using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Service;

public interface I_EventService : I_Service<long, Event>
{
    IEnumerable<EventDTO> getEventsWithParticipantsCount();
    void saveEventEntry(Office office);
    IEnumerable<ParticipantDTO> getParticipantsForEventWithCount(long eventId);
}