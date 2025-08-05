using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Service;

public interface IMainObserver
{
       void ParticipantAdded(Participant participant);
       void EventEvntriesAdded(List<EventDTO> events);
}