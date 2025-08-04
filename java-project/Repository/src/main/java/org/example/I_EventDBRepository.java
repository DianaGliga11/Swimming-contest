package org.example;

import java.util.ArrayList;
import java.util.List;

public interface I_EventDBRepository extends I_Repository<Event> {
    List<Event> events = new ArrayList<>();
}
