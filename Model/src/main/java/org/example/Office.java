package org.example;

import java.util.List;

public class Office extends Entity<Integer> {
    private List<Participant> participants;
    private List<Event> events;

    public Office(int id, List<Participant> participants, List<Event> events) {
        super(id);
        this.participants = participants;
        this.events = events;
    }

    public List<Participant> getParticipants() {
        return this.participants;
    }

    public List<Event> getEvents() {
        return this.events;
    }

    public void setPrticipants(List<Participant> prticipants) {
        this.participants = prticipants;
    }

    public void setEvents(List<Event> events) {
        this.events = events;
    }

    @Override
    public String toString() {
        return "Office{" + super.toString() +
                " prticipants=" + (participants != null ? participants: "No participants") +
                " events=" + (events != null ? events: "No events") +
                '}';
    }

    //    public String toString() {
//        return super.toString() + " " + prticipants.toString() + events.toString();
//    }
}
