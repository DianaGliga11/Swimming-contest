using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    
    public class GetParticipantsForEventWithCountResponse: IResponse
    {
        [JsonPropertyName("participants")]
        public List<ParticipantDTO> participants{ get; set; }

        [JsonConstructor]
        public GetParticipantsForEventWithCountResponse()
        {
            
        }

        public GetParticipantsForEventWithCountResponse(List<ParticipantDTO> participants)
        {
            this.participants = participants;
        }
    }
}