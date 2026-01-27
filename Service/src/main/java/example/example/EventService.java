package example.example;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import example.model.Event;
import example.model.Office;
import example.repo.EntityRepoException;

import java.util.Collection;

public interface EventService extends Service<Long, Event> {
    Collection<Office> getEntriesByEvent(Long eventID);

    Collection<EventDTO> getEventsWithParticipantsCount() throws EntityRepoException;

    void saveEventEntry(Office office) throws EntityRepoException;

    void deleteByIDs(Long participantID, Long eventID) throws EntityRepoException;

    Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws EntityRepoException;
}
