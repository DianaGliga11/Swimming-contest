package org.example;

public class Event extends Entity<Integer> {
    private int distance;
    private String style;
    //private List<Participant>  participants;

    public Event(Integer id, String style, int distance) {
        super(id);
        this.style = style;
        this.distance = distance;
        //this.participants = participants;
    }

    public int getDistance() {
        return distance;
    }

    public void setDistance(int distance) {
        this.distance = distance;
    }

    public String getStyle() {
        return style;
    }

    public void setStyle(String style) {
        this.style = style;
    }

//    public List<Participant> getParticipants() {
//        return participants;
//    }
//
//    public void setParticipants(List<Participant> participants) {
//        this.participants = participants;
//    }

    @Override
    public String toString() {
        return "Event{" + super.toString() +
                ", distance=" + distance +
                ", style='" + style + '\'' +
                //               ", participants=" + participants.toString() +
                '}';
    }

    @Override
    public Integer getId() {
        return this.id;
    }


}
