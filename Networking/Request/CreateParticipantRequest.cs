using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class CreateParticipantRequest : IRequest
    {
        public Participant participant{get;set;}

        public CreateParticipantRequest()
        {
        }

        public CreateParticipantRequest(Participant participant)
        {
            this.participant = participant;
        }
    }
}