using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class CreateEventEntriesRequest : IRequest
    {
        [JsonPropertyName("eventEntries")]
        public List<Office> EventEntries{get;set;} = null!;

        [JsonConstructor]
        public CreateEventEntriesRequest()
        {
        }

        public CreateEventEntriesRequest(List<Office> eventEntries)
        {
            this.EventEntries = eventEntries;
        }
    }
}