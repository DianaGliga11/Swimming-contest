package request;

import example.example.Event;

public class CreateEventRequest implements Request {
    private final Event event;

    public CreateEventRequest(Event event) {
        this.event = event;
    }

    public Event getEvent() {
        return event;
    }
}
