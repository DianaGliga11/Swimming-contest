using System.Collections;
using System.Collections.Generic;
using mpp_proiect_csharp_DianaGliga11.Model;

public class I_Repositoty<T>
{
}

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_OfficeDBRepository : I_Repository<Office>
{
    IEnumerable<Office> getEntriesByEvent(long eventID);
    IEnumerable<Participant> findParticipantsByEvent(long eventID);
    
    void deleteByIDs(long participantIDs, long eventID);
   
    int countEventsForParticipant(long participantId);
}
}