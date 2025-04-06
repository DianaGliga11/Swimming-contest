package response;

import org.example.Participant;

import java.util.Collection;

public class AllParticipantsResponse implements Response {
    private final Collection<Participant> participants;

    public AllParticipantsResponse(Collection<Participant> participants) {
        this.participants = participants;
    }

    public Collection<Participant> getParticipants() {
        return participants;
    }
}
