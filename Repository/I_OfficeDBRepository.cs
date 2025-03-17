using System.Collections.Generic;
using mpp_proiect_csharp_DianaGliga11.Model;

public class I_Repositoty<T>
{
}

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_OfficeDBRepository : I_Repository<Office>
{
    //IEnumerable<Office> Offices{get; }
    //bool findParticipantByEvent(Event e);
}
}