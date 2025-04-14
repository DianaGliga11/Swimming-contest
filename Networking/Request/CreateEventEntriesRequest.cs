using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class CreateEventEntriesRequest : IRequest
    {
        public List<Office> eventEntries{get;set;}

        public CreateEventEntriesRequest()
        {
        }

        public CreateEventEntriesRequest(List<Office> eventEntries)
        {
            this.eventEntries = eventEntries;
        }
    }
}