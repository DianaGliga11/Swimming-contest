package Hibernate;

import example.repo.EntityRepoException;
import example.model.Event;
import example.repo.EventRepository;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.query.Query;

import java.util.List;

public class EventORMRepository implements EventRepository {
    private final SessionFactory sessionFactory;
    private static final Logger logger = LogManager.getLogger(EventORMRepository.class);

    public EventORMRepository(SessionFactory sessionFactory) {
        this.sessionFactory = sessionFactory;
    }

    @Override
    public void add(Event entity) throws EntityRepoException {
        Transaction transaction = null;
        try (Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            session.persist(entity);
            transaction.commit();
        } catch (Exception e) {
            if (transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la adăugarea evenimentului: " + e.getMessage());
        }
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        Transaction transaction = null;
        try (Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            Event event = session.get(Event.class, id);
            if (event != null) {
                session.remove(event);
            } else {
                throw new EntityRepoException("Evenimentul cu id " + id + " nu a fost găsit.");
            }
            transaction.commit();
        } catch (Exception e) {
            if (transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la ștergerea evenimentului: " + e.getMessage());
        }
    }

    @Override
    public List<Event> getAll() throws EntityRepoException {
        try (Session session = sessionFactory.openSession()) {
            Query<Event> query = session.createQuery("FROM Event", Event.class);
            return query.list();
        } catch (Exception e) {
            logger.error(e);
            throw new EntityRepoException("Eroare la preluarea tuturor evenimentelor: " + e.getMessage());
        }
    }

    @Override
    public Event findById(long id) throws EntityRepoException {
        try (Session session = sessionFactory.openSession()) {
            return session.get(Event.class, id);
        } catch (Exception e) {
            logger.error(e);
            throw new EntityRepoException("Eroare la căutarea evenimentului după ID: " + e.getMessage());
        }
    }

    @Override
    public void update(long id, Event entity) throws EntityRepoException {
        Transaction transaction = null;
        try (Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            session.merge(entity);
            transaction.commit();
        } catch (Exception e) {
            if (transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la actualizarea evenimentului: " + e.getMessage());
        }
    }
}
