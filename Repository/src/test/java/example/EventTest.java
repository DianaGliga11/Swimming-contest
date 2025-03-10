package example;

import org.example.Event;
import org.example.Participant;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

class EventTest {

    @Test
    void getDistance() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        assertEquals(150, event.getDistance());
    }

    @Test
    void setDistance() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        event.setDistance(200);
        assertEquals(200, event.getDistance());
    }

    @Test
    void getStyle() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        assertEquals("mixt", event.getStyle());
    }

    @Test
    void setStyle() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        event.setStyle("fluture");
        assertEquals("fluture", event.getStyle());
    }

    @Test
    void testToString() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        assertEquals("Event{Entity{id=1}, distance=150, style='mixt'}", event.toString());
    }

    @Test
    void getId() {
        List<Participant> participanti = new ArrayList<Participant>();
        Event event = new Event(1, "mixt", 150 );
        assertEquals(1, event.getId());
    }
}