package org.example;

import java.util.List;

public class ParticipantImplementationService implements ParticipantService {
    private final ParticipantRepository participantRepository;

    public ParticipantImplementationService(final ParticipantRepository participantRepository) {
        this.participantRepository = participantRepository;
    }

    @Override
    public void add(Participant entity) throws EntityRepoException {
        participantRepository.add(entity);
    }

    @Override
    public void remove(Long id) throws EntityRepoException {
        participantRepository.remove(id);
    }

    @Override
    public void update(Long id, Participant entity) throws EntityRepoException {
        participantRepository.update(id, entity);
    }

    @Override
    public List<Participant> getAll() throws EntityRepoException {
        return participantRepository.getAll();
    }

    @Override
    public Participant findById(Long id) throws EntityRepoException {
        return participantRepository.findById(id);
    }
}
