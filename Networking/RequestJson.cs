using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking
{

    public class RequestJson
    {
        [JsonPropertyName("type")] public RequestType Type { get; set; }

        [JsonPropertyName("username")] public string? Username { get; set; }

        [JsonPropertyName("password")] public string? Password { get; set; }

        [JsonPropertyName("user")] public User? User { get; set; }

        [JsonPropertyName("participant")] public Participant? Participant { get; set; }

        [JsonPropertyName("eventEntries")] public List<Office>? EventEntries { get; set; }

        [JsonPropertyName("eventId")] public long? EventId { get; set; }

        public RequestJson()
        {
        }
    }
}