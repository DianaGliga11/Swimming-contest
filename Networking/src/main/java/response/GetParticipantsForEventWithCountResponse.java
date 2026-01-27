package response;

import DTO.ParticipantDTO;

import java.util.Collection;

public class GetParticipantsForEventWithCountResponse implements Response{
    private final Collection<ParticipantDTO> participants;

    public GetParticipantsForEventWithCountResponse(Collection<ParticipantDTO> participants) {
        this.participants = participants;
    }

    public Collection<ParticipantDTO> getParticipants() {
        return participants;
    }
}
