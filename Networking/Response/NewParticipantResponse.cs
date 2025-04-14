using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class NewParticipantResponse : UpdateResponse
    {
        public Participant participant{get;set;}

        public NewParticipantResponse()
        {
        }

        public NewParticipantResponse(Participant participant)
        {
            this.participant = participant;
        }


    }
}