package contestUtils;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import example.example.Event;
import example.example.Office;
import example.example.Participant;
import example.example.User;

import java.util.Collection;
import java.util.List;

public interface IContestServices {
    User login(String userName, String password, IMainObserver client) throws Exception;

    void logout(User user, IMainObserver client) throws Exception;

    Collection<EventDTO> getEventsWithParticipantsCount() throws Exception;

    Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws Exception;

    void saveEventEntry(List<Office> offices) throws Exception;

    Collection<Participant> findAllParticipants() throws Exception;

    void saveParticipant(Participant participant) throws Exception;

    Collection<Event> findAllEvents() throws Exception;

    void saveEvent(Event event) throws Exception;

}
