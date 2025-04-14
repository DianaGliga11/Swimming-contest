using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class AllEventsResponse : IResponse
    {
        public List<Event> events{ get; set; }

        public AllEventsResponse()
        {
        }

        public AllEventsResponse(List<Event> events)
        {
            this.events = events;
        }
    }
}