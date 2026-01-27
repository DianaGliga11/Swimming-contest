package example.example;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import example.repo.EntityRepoException;
import example.repo.EventRepository;
import example.repo.OfficeRepository;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class EventImplementationService implements EventService {
    private final EventRepository eventRepository;
    private final OfficeRepository officeRepository;

    public EventImplementationService(final EventRepository eventRepository, final OfficeRepository officeRepository) {
        this.eventRepository = eventRepository;
        this.officeRepository = officeRepository;
    }

    @Override
    public void add(Event entity) throws EntityRepoException {
        eventRepository.add(entity);
    }

    @Override
    public void remove(Long id) throws EntityRepoException {
        eventRepository.remove(id);
    }

    @Override
    public void update(Long id, Event entity) throws EntityRepoException {
        eventRepository.update(id, entity);
    }

    @Override
    public List<Event> getAll() throws EntityRepoException {
        return eventRepository.getAll();
    }

    @Override
    public Event findById(Long id) throws EntityRepoException {
        return eventRepository.findById(id);
    }

    @Override
    public Collection<Office> getEntriesByEvent(Long eventID) {
        return List.of();
    }

    @Override
    public Collection<EventDTO> getEventsWithParticipantsCount() throws EntityRepoException {
        Collection<EventDTO> result = new ArrayList<>();
        Collection<Event> events = eventRepository.getAll();
        for (Event event : events) {
            Collection<Office> entries = officeRepository.getEntriesByEvent(event.getId());
            long uniqueParticipants = entries.stream()
                    .map(Office::getParticipant)
                    .distinct()
                    .count();
            result.add(new EventDTO(
                    event.getStyle(), event.getDistance(), (int) uniqueParticipants));
        }
        return result;
    }

    @Override
    public void saveEventEntry(Office office) throws EntityRepoException {
        officeRepository.add(office);
    }

    @Override
    public void deleteByIDs(Long participantID, Long eventID) {
        officeRepository.deleteByIDs(participantID, eventID);
    }

    @Override
    public Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws EntityRepoException {
        Collection<Participant> participants = officeRepository.findParticipantsByEvent(eventId);
        List<ParticipantDTO> result = new ArrayList<>();
        for (Participant p : participants) {
            int eventCount = officeRepository.countEventsForParticipant(p.getId());
            result.add(new ParticipantDTO(
                    p.getName(),
                    p.getAge(),
                    eventCount
            ));
        }
        return result;
    }
}
