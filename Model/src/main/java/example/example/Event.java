package example.example;

public class Event extends Entity<Long> {
    private int distance;
    private String style;

    public Event(String style, int distance) {
        this.style = style;
        this.distance = distance;
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

    @Override
    public String toString() {
        return "Event{" + super.toString() +
                ", distance=" + distance +
                ", style='" + style + '\'' +
                '}';
    }

}
