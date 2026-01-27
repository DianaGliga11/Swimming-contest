package example.example;

import example.model.Participant;

import java.util.Optional;

public interface ParticipantService extends Service<Long, Participant> {
    Optional<Participant> getParticipantByData(Participant participant);
}
