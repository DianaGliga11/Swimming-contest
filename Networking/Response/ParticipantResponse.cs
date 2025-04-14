using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class ParticipantResponse : IResponse
    {
        private  Participant participant{ get; set; }

        public ParticipantResponse()
        {
        }

        public ParticipantResponse(Participant participant)
        {
            this.participant = participant;
        }
        
    }
}