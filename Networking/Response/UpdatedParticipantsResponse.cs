using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{

    public class UpdatedParticipantsResponse : UpdateResponse
    {
        [JsonPropertyName("participants")]
        public List<Participant> Participants { get; set; }

        [JsonConstructor]
        public UpdatedParticipantsResponse()
        {
            
        }

        public UpdatedParticipantsResponse(List<Participant> participants)
        {
            Participants = participants;
        }
    }
}