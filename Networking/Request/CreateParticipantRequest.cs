using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class CreateParticipantRequest : IRequest
    {
        [JsonPropertyName("participant")]
        public Participant Participant{get;set;}

        [JsonConstructor]
        public CreateParticipantRequest()
        {
        }

        public CreateParticipantRequest(Participant participant)
        {
            this.Participant = participant;
        }
    }
}