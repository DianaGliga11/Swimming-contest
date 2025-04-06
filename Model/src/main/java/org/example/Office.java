package org.example;

public class Office extends Entity<Long> {
    private Participant participant;
    private Event event;

    public Office(Participant participant, Event event) {
        this.participant = participant;
        this.event = event;
    }

    public Participant getParticipant() {
        return this.participant;
    }

    public Event getEvent() {
        return this.event;
    }

    public void setParticipant(Participant participant) {
        this.participant = participant;
    }

    public void setEvent(Event event) {
        this.event = event;
    }

    @Override
    public String toString() {
        return "Office{" + super.toString() +
                " participant=" + (participant != null ? participant : "No participant") +
                ", event=" + (event != null ? event : "No event") +
                '}';
    }
}
