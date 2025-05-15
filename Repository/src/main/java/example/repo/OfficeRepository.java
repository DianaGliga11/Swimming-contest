package example.repo;

import example.model.Office;
import example.model.Participant;

import java.util.Collection;

public interface OfficeRepository extends Repository<Office> {
    Collection<Office> getEntriesByEvent(Long eventID);

    void deleteByIDs(Long participantID, Long eventID);

    Collection<Participant> findParticipantsByEvent(Long eventId);

    int countEventsForParticipant(Long participantId) throws EntityRepoException;
}
