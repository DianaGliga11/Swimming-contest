package response;

import example.model.Event;

import java.util.Collection;

public class AllEventsResponse implements Response {
    private final Collection<Event> events;

    public AllEventsResponse(Collection<Event> events) {
        this.events = events;
    }

    public Collection<Event> getEvents() {
        return events;
    }
}
