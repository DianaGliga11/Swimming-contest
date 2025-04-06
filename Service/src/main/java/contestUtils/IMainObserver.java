package contestUtils;

import DTO.EventDTO;
import org.example.Event;
import org.example.Participant;

import java.util.List;

public interface IMainObserver {
    void participantAdded(Participant participant);
    void newRegistration(List<EventDTO> events);

    void eventAdded(EventDTO event);
}
