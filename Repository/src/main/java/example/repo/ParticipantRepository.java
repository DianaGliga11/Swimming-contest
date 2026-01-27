package example.repo;

import example.model.Participant;

import java.util.Optional;

public interface ParticipantRepository extends Repository<Participant> {
    Optional<Participant> getParticipantByData(final Participant participant);
}
