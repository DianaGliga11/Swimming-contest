using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{

    public class AllParticipantsResponse : IResponse
    {
        [JsonPropertyName("participants")]
        public List<Participant> Participants{ get; set; }

        [JsonConstructor]
        public AllParticipantsResponse()
        {
        }

        public AllParticipantsResponse(List<Participant> participants)
        {
            this.Participants = participants;
        }
    }
}