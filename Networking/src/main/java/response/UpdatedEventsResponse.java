package response;

import DTO.EventDTO;

import java.util.List;

public class UpdatedEventsResponse implements UpdateResponse{
    private final List<EventDTO> eventsDTO;

    public UpdatedEventsResponse(List<EventDTO> eventsDTO) {
        this.eventsDTO = eventsDTO;
    }

    public List<EventDTO> getEventsDTO() {
        return eventsDTO;
    }
}
