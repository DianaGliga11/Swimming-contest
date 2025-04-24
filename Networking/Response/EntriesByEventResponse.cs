using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;
using Networking.Request;

namespace Networking.Response
{
    public class EntriesByEventResponse : IResponse
    {
        [JsonPropertyName("eventEntries")]
        private IEnumerable<Office> EventEntries{get;set;}

        [JsonConstructor]
        public EntriesByEventResponse()
        {
        }

        public EntriesByEventResponse(IEnumerable<Office> eventEntries)
        {
            this.EventEntries = eventEntries;
        }

    }
}