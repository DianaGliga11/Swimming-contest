using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace Service;

public class EventService : I_EventService
{
    private I_EventDBRepository eventRepository;
    private I_OfficeDBRepository officeRepository;

    public EventService(I_EventDBRepository eventRepository, I_OfficeDBRepository officeRepository)
    {
        this.eventRepository = eventRepository;
        this.officeRepository = officeRepository;
    }

    public Event findByID(long id)
    {
        return eventRepository.findById(id);
    }

    public IEnumerable<Event> getAll()
    {
        return eventRepository.getAll();
    }

    public void add(Event entity)
    {
        eventRepository.Add(entity);
    }

    public void delete(long id)
    {
        eventRepository.Remove(id);
    }

    public void update(long id, Event entity)
    {
        eventRepository.Update(id, entity);
    }

    public IEnumerable<EventDTO> getEventsWithParticipantsCount()
    {
        IList<EventDTO> result = new List<EventDTO>();
        IEnumerable<Event> events = eventRepository.getAll();
        foreach (var e in events)
        {
            result.Add(new EventDTO(e.Style, e.Distance, officeRepository.getEntriesByEvent(e.Id).Count()));
        }
        return result;
    }

    public void saveEventEntry(Office office)
    {
        officeRepository.Add(office);
    }

    public IEnumerable<ParticipantDTO> getParticipantsForEventWithCount(long eventId)
    {
        IEnumerable<Participant> participants = officeRepository.findParticipantsByEvent(eventId);
        IList<ParticipantDTO> result = new List<ParticipantDTO>();
        foreach (var participant in participants)
        {
            int eventCount = officeRepository.countEventsForParticipant(participant.Id);
            result.Add(new ParticipantDTO(participant.Name, participant.Age, eventCount));
        }
        return result;
    }
}