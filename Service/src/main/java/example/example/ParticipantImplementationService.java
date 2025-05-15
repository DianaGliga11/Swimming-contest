package example.example;

import example.model.Participant;
import example.repo.EntityRepoException;
import example.repo.ParticipantRepository;

import java.util.List;
import java.util.Optional;

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

    @Override
    public Optional<Participant> getParticipantByData(Participant participant) {
        return participantRepository.getParticipantByData(participant);
    }
}
