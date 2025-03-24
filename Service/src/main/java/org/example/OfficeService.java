package org.example;

import java.util.List;
import java.util.Map;

public interface OfficeService extends Service<Long, Office> {
    Map<Event, Integer> getEventParticipants();
}
