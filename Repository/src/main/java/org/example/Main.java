package org.example;

import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

public class Main {
    public static void main(String[] args) throws EntityRepoException {
        try {
            Properties props = new Properties();
            try {
                props.load(new FileReader("db.config"));
            } catch (IOException e) {
                System.out.println("Cannot find db.config " + e);
                e.printStackTrace();
            }
            Repository<Event> eventRepo = new EventDBRepository(props);
            Repository<User> userRepo = new UserDBRepository(props);
            Repository<Participant> participantRepo = new ParticipantDBRepository(props);
            Repository<Office> officeRepo = new OfficeDBRepository(props);

            System.out.println("All events: ");
            for (Event event : eventRepo.getAll()) {
                System.out.println(event);
            }
            System.out.println("All users: ");
            for (User user : userRepo.getAll()) {
                System.out.println(user);
            }
            System.out.println("All participants: ");
            for (Participant participant : participantRepo.getAll()) {
                System.out.println(participant);
            }
            System.out.println("All offices: ");
            for (Office office : officeRepo.getAll()) {
                System.out.println(office);
            }
//            List<Participant> participantList = new ArrayList<>();
//            participantList.add(participantRepo.findById(2));
//            List<Event> eventList = new ArrayList<>();
//            eventList.add(eventRepo.findById(1));
//            Office office = new Office(participantList,eventList);
//            Participant participant = new Participant("Gliga Diana",20);
//            participant.setId(1);
//            participantRepo.update(1,participant); //la update da eroare indiferent de repo daca nu setez eu id ul
//            officeRepo.add(office);
//            eventList.add(eventRepo.findById(2));
//            office.setEvents(eventList);
//            office.setId(3);
//            officeRepo.update(1,office);
//            officeRepo.remove(office);
//            System.out.println(officeRepo.getAll());
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
}
