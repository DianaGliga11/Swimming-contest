using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking.Response
{
    
    public class GetParticipantsForEventWithCountResponse: IResponse
    {
        public List<ParticipantDTO> participants;

        public GetParticipantsForEventWithCountResponse()
        {
            
        }

        public GetParticipantsForEventWithCountResponse(List<ParticipantDTO> participants)
        {
            this.participants = participants;
        }
    }
}