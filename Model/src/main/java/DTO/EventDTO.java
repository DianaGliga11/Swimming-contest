package DTO;

import example.example.Entity;

import java.io.Serializable;

public class EventDTO extends Entity<Long> implements Serializable {
    private final String style;
    private final Integer distance;
    private final Integer participantsCount;

    public EventDTO(String style, Integer distance, Integer participantsCount) {
        this.style = style;
        this.distance = distance;
        this.participantsCount = participantsCount;
    }

    public String getStyle() {
        return style;
    }

    public Integer getDistance() {
        return distance;
    }

    public Integer getParticipantsCount() {
        return participantsCount;
    }
}
