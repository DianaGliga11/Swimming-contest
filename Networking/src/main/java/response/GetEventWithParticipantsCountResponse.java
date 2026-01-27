package response;

import DTO.EventDTO;

import java.util.Collection;

public class GetEventWithParticipantsCountResponse implements Response{
    private final Collection<EventDTO> events;

    public GetEventWithParticipantsCountResponse(Collection<EventDTO> events) {
        this.events = events;
    }

    public Collection<EventDTO> getEvents() {
        return events;
    }
}
