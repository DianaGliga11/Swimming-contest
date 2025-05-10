package Hibernate;

import example.example.EntityRepoException;
import example.example.Event;
import example.example.Participant;

import java.util.List;
import java.util.Optional;

public class TestHibernate {
    public static void main(String[] args) throws EntityRepoException {
        ParticipantORMRepository repo = new ParticipantORMRepository(HibernateUtil.getSessionFactory());

        Participant p = new Participant("Ana", 22);
        repo.add(p);

        List<Participant> list = repo.getAll();
        System.out.println("Participanti: " + list);

        Optional<Participant> opt = repo.getParticipantByData(new Participant("Ana", 22));
        opt.ifPresentOrElse(
                found -> System.out.println("Gasit: " + found),
                () -> System.out.println("Nu a fost gasit")
        );

        EventORMRepository eventRepo = new EventORMRepository(HibernateUtil.getSessionFactory());

        Event e = new Event("fluture", 200);
        eventRepo.add(e);

        List<Event> events = eventRepo.getAll();
        System.out.println("Evenimente: " + events);

        Event foundEvent = eventRepo.findById(e.getId());
        System.out.println("Eveniment gasit dupa ID: " + foundEvent);
    }
}
