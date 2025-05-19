package example.model;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;

import java.io.Serializable;

//@Entity
//@Table(name = "Events")
//public class Event extends Identifiable<Long> implements Serializable {
//    @Id
//    @GeneratedValue
//    private Long id;
//
//    private int distance;
//    private String style;
//
//    public Event() {}
//
//    public Event(String style, int distance) {
//        this.style = style;
//        this.distance = distance;
//    }
//
//    public Long getId() {
//        return this.id;
//    }
//
//    public void setId(Long id) {
//        this.id = id;
//    }
//
//    @NotNull
//    public int getDistance() {
//        return distance;
//    }
//
//    public void setDistance(int distance) {
//        this.distance = distance;
//    }
//
//    @NotNull
//    public String getStyle() {
//        return style;
//    }
//
//    public void setStyle(String style) {
//        this.style = style;
//    }
//
//    @Override
//    public String toString() {
//        return "Event{" + super.toString() +
//                ", distance=" + distance +
//                ", style='" + style + '\'' +
//                '}';
//    }}

public class Event extends Identifiable<Long> {
    private int distance;
    private String style;


    public Event(){}


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