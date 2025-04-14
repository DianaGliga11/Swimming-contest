using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    public class EventsWithParticipantsCountResponse : IResponse
    {
        public List<EventDTO> events{ get; set; }

        public EventsWithParticipantsCountResponse()
        {
        }

        public EventsWithParticipantsCountResponse(List<EventDTO> events)
        {
            this.events = events;
        }

    }
}