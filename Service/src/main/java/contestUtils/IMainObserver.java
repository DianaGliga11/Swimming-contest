package contestUtils;

import DTO.EventDTO;
import example.model.Participant;

import java.util.List;

public interface IMainObserver {
    void participantAdded(Participant participant) throws Exception;

    void newRegistration(List<EventDTO> events) throws Exception;

    void eventAdded(EventDTO event) throws Exception;
}
