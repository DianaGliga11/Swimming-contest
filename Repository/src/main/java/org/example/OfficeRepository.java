package org.example;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public interface OfficeRepository extends Repository<Office> {
    //List<Office> offices = new ArrayList<>();
    //List<Map<String,Object>> findParticipantsByEvent(Event event) throws EntityRepoException;
    public Map<Event, Integer> getEventsWithParticipantsCount();
    public List<Participant> findParticipantsByEvent(Long eventId);
    public void registerParticipantToEvents(Long participantId, List<Long> eventIds);
}
