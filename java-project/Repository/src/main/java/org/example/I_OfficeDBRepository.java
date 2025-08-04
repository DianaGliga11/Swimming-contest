package org.example;

import java.util.ArrayList;
import java.util.List;

public interface I_OfficeDBRepository extends I_Repository<Office>{
    List<Office> offices = new ArrayList<>();
    boolean findParticipantByEvent(Event event);

}
