package example.example;

import java.util.Optional;

public interface ParticipantRepository extends Repository<Participant> {
    Optional<Participant> getParticipantByData(final Participant participant);
}
