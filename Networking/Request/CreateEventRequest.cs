using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{

    public class CreateEventRequest
    {
        public Event ev { get; set; }

        public CreateEventRequest()
        {
        }

        public CreateEventRequest(Event ev)
        {
            this.ev = ev;
        }
    }
}