using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class NewParticipantResponse : UpdateResponse
    {
        [JsonPropertyName("participant")]
        public Participant Participant{get;set;}

        public NewParticipantResponse()
        {
        }

        public NewParticipantResponse(Participant participant)
        {
            this.Participant = participant;
        }


    }
}