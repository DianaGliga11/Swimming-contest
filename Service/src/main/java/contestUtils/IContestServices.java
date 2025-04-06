package contestUtils;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import org.example.*;

import java.util.Collection;
import java.util.List;

public interface IContestServices {
    User login(String userName, String password, IMainObserver client) throws Exception;
    void logout(User user, IMainObserver client) throws Exception;
    Collection<EventDTO> getEventsWithParticipantsCount()throws EntityRepoException;
    Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws EntityRepoException;
    void saveEventEntry(List<Office> offices) throws EntityRepoException;
    Collection<Participant> findAllParticipants() throws EntityRepoException;
    //Collection<Event> findAllEvents() throws EntityRepoException;

    void saveParticipant(Participant participant) throws EntityRepoException;
}
