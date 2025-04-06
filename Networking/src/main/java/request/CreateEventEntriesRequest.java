package request;


import org.example.Office;

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
