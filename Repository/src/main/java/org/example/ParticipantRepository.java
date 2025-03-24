package org.example;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public interface ParticipantRepository extends Repository<Participant> {
    Optional<Participant> getParticipantByData(final Participant participant);
}
