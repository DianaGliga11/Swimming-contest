package org.example;

import java.util.ArrayList;
import java.util.List;

public interface I_ParticipantDBRepository extends I_Repository<Participant>{
    List<Participant> participants = new ArrayList<>();
}
