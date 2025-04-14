using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    public class UpdatedEventsResponse : UpdateResponse
    {
        public List<EventDTO> events{get;set;}

        public UpdatedEventsResponse()
        {
        }

        public UpdatedEventsResponse(List<EventDTO> events)
        {
            this.events = events;
        }
    }
}