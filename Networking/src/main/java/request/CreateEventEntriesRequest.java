package request;


import example.model.Office;

import java.util.List;

public class CreateEventEntriesRequest implements Request {
    private final List<Office> eventEntries;

    public CreateEventEntriesRequest(List<Office> eventEntries) {
        this.eventEntries = eventEntries;
    }

    public List<Office> getEventEntries() {
        return eventEntries;
    }
}
