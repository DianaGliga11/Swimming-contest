using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class CreateParticipantRequest : IRequest
    {
        [JsonPropertyName("participants")]
        public List<Participant> Participants{get;set;}

        [JsonConstructor]
        public CreateParticipantRequest()
        {
        }

        public CreateParticipantRequest(List<Participant> participants)
        {
            this.Participants = participants;
        }
    }
}