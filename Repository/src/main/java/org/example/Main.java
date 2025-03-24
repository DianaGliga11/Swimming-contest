package org.example;

import java.io.FileReader;
import java.io.IOException;
import java.util.List;
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
           OfficeRepository officeRepo = new OfficeDBRepository(props);

            List<Participant> participantsFluture = officeRepo.findParticipantsByEvent(1L);
            // officeRepo.add(new Office(participantRepo.findById(1), eventRepo.findById(1)));
            System.out.println("Participants at event fluture: ");
            for (Participant participant : participantsFluture) {
                System.out.println(participant);
            }
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
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
}
