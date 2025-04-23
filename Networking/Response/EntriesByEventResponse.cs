using mpp_proiect_csharp_DianaGliga11.Model;
using Networking.Request;

namespace Networking.Response
{
    public class EntriesByEventResponse : IResponse
    {
        private IEnumerable<Office> eventEntries{get;set;}

        public EntriesByEventResponse()
        {
        }

        public EntriesByEventResponse(IEnumerable<Office> eventEntries)
        {
            this.eventEntries = eventEntries;
        }

    }
}