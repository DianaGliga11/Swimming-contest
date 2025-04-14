using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{

    public class AllParticipantsResponse : IResponse
    {
        public List<Participant> participants{ get; set; }

        public AllParticipantsResponse()
        {
        }

        public AllParticipantsResponse(List<Participant> participants)
        {
            this.participants = participants;
        }
    }
}