package response;

import DTO.EventDTO;
import org.example.Event;

public class NewEventResponse implements UpdateResponse{
    private final EventDTO event;

    public NewEventResponse(EventDTO event) {
        this.event = event;
    }

    public EventDTO getEvent() {
        return event;
    }
}
