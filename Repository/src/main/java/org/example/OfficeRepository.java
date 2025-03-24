package org.example;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;

public interface OfficeRepository extends Repository<Office> {
    Collection<Office> getEntriesByEvent(Long eventID);
    void deleteByIDs(Long participantID, Long eventID);
}
