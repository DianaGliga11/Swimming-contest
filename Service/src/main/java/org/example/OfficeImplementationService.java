package org.example;

import java.util.List;
import java.util.Map;

public class OfficeImplementationService implements OfficeService {
    private final OfficeDBRepository officeDBRepository;

    public OfficeImplementationService(final OfficeDBRepository officeDBRepository) {
        this.officeDBRepository = officeDBRepository;
    }

    @Override
    public void add(Office entity) throws EntityRepoException {
        officeDBRepository.add(entity);
    }

    @Override
    public void remove(Long id) throws EntityRepoException {
        officeDBRepository.remove(id);
    }

    @Override
    public void update(Long id, Office entity) throws EntityRepoException {
        officeDBRepository.update(id, entity);
    }

    @Override
    public List<Office> getAll() throws EntityRepoException {
        return officeDBRepository.getAll();
    }

    @Override
    public Office findById(Long id) throws EntityRepoException {
        return officeDBRepository.findById(id);
    }

//    @Override
//    public List<Participant> findParticipantsByEvent(Long eventId) {
//        return officeDBRepository.findParticipantsByEvent(eventId);
//    }
//
//    @Override
//    public Map<Event, Integer> getEventsWithParticipantsCount() {
//        return officeDBRepository.getEventsWithParticipantsCount();
//    }
//
//    @Override
//    public void registerParticipantToEvents(Long participantId, List<Long> eventIds) {
//        officeDBRepository.registerParticipantToEvents(participantId, eventIds);
//    }
}
