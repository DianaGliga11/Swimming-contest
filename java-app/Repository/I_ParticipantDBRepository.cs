using System.Collections.Generic;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_ParticipantDBRepository : I_Repository<Participant>
{
    Participant   GetParticipantsByData(Participant participant);
}
}