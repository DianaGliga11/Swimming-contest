using System.Text.Json.Serialization;

namespace Networking.Request
{

    public class GetParticipantsForEventWithCountRequest: IRequest
    {
        [JsonPropertyName("eventId")]
        public long EventId { get; set; }

        [JsonConstructor]
        public GetParticipantsForEventWithCountRequest()
        {

        }

        public GetParticipantsForEventWithCountRequest(long eventId)
        {
            EventId = eventId;
        }
}
}