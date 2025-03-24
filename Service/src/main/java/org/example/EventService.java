package org.example;

import DTO.EventDTO;

import java.util.Collection;
import java.util.Optional;

public interface EventService extends Service<Long, Event> {
   Collection<Office> getEntriesByEvent(Long eventID);
   Collection<EventDTO> getEventsWithParticipantsCount() throws EntityRepoException;
   void saveEventEntry(Office office) throws EntityRepoException;
   void deleteByIDs(Long participantID, Long eventID) throws EntityRepoException;
}
