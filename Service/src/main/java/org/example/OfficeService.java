package org.example;

import java.util.List;
import java.util.Map;

public interface OfficeService extends Service<Long, Office> {
    public List<Participant> findParticipantsByEvent(Long eventId);
    public Map<Event, Integer> getEventsWithParticipantsCount();
    public void registerParticipantToEvents(Long participantId, List<Long> eventIds);
}
