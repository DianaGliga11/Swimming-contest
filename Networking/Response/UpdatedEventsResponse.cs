using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    public class UpdatedEventsResponse : UpdateResponse
    {
        [JsonPropertyName("events")]
        public List<EventDTO> Events{get;set;}

        [JsonConstructor]
        public UpdatedEventsResponse()
        {
        }

        public UpdatedEventsResponse(List<EventDTO> events)
        {
            this.Events = events;
        }
    }
}