using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    
    public class GetParticipantsForEventWithCountResponse: IResponse
    {
        private IEnumerable<ParticipantDTO> participants;

        public GetParticipantsForEventWithCountResponse()
        {
            
        }

        public GetParticipantsForEventWithCountResponse(IEnumerable<ParticipantDTO> participants)
        {
            this.participants = participants;
        }
    }
}