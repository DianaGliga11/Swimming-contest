using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking
{

    public class ResponseJson
    {
        [JsonPropertyName("type")]
        public ResponseType Type { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("participant")]
        public Participant? Participant { get; set; }

        [JsonPropertyName("events")]
        public List<EventDTO>? Events { get; set; }

        [JsonPropertyName("participants")]
        public List<ParticipantDTO>? Participants { get; set; }

        // Pentru trimitere Eventuri raw (ex: AllEvents)
        [JsonPropertyName("eventsRaw")]
        public List<Event>? EventsRaw { get; set; }

        // Pentru trimitere Participanti raw (ex: AllParticipants)
        [JsonPropertyName("participantsRaw")]
        public List<Participant>? ParticipantsRaw { get; set; }

        public ResponseJson() { }
    }
}