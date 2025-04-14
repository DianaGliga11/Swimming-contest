package request;

public class GetParticipantsForEventWithCountRequest implements Request {
    private final Long eventId;

    public GetParticipantsForEventWithCountRequest(long eventId) {
        this.eventId = eventId;
    }

    public Long getEventId() {
        return eventId;
    }
}
