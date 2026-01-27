package response;

import DTO.EventDTO;

public class NewEventEntryResponse implements UpdateResponse{
    public final EventDTO eventDTO;

    public NewEventEntryResponse(EventDTO eventDTO) {
        this.eventDTO = eventDTO;
    }

    public EventDTO getEventDTO() {
        return eventDTO;
    }
}
