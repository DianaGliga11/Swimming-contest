package org.example;

import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

class OfficeTest {

    @Test
    void getParticipants() {
        List<Participant> participants = new ArrayList<>();
        participants.add(new Participant("Ana",30));
        Office office = new Office(participants,new ArrayList<>());
        assertEquals(office.getParticipants().size(), 1);
    }

    @Test
    void getEvents() {
        List<Event> events = new ArrayList<>();
        events.add(new Event("mixt",280));
        events.add(new Event("fluture",150));
        Office office = new Office(new ArrayList<>(),events);
        assertEquals(office.getEvents().size(), 2);
    }

    @Test
    void setPrticipants() {
        List<Participant> participants = new ArrayList<>();
        participants.add(new Participant("Ana",30));
        Office office = new Office(participants,new ArrayList<>());
        assertEquals(office.getParticipants().size(), 1);
        office.setEvents(new ArrayList<>());
        assertEquals(office.getEvents().size(), 0);
    }

    @Test
    void setEvents() {
        List<Event> events = new ArrayList<>();
        events.add(new Event("mixt",280));
        events.add(new Event("fluture",150));
        Office office = new Office (new ArrayList<>(),events);
        assertEquals(office.getEvents().size(), 2);
        office.setEvents(new ArrayList<>());
        assertEquals(office.getEvents().size(), 0);
    }

    @Test
    void testToString() {
        List<Event> events = new ArrayList<>();
        events.add(new Event("mixt",280));
        events.add(new Event("fluture",150));
        Office office = new Office(new ArrayList<Participant>(), events);
        assertEquals("Office{Entity{id=1} prticipants=[] events=[Event{Entity{id=1}, distance=280, style='mixt'}, Event{Entity{id=2}, distance=150, style='fluture'}]}", office.toString());
    }
}