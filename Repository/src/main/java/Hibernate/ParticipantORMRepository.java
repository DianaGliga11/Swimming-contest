package Hibernate;

import example.repo.EntityRepoException;
import example.model.Participant;
import example.repo.ParticipantRepository;
import org.apache.logging.log4j.LogManager;
import org.hibernate.Session;
import org.hibernate.SessionFactory;

import java.util.List;
import java.util.Optional;

import org.apache.logging.log4j.core.Logger;
import org.hibernate.Transaction;
import org.hibernate.query.Query;

public class ParticipantORMRepository implements ParticipantRepository {
    private final SessionFactory sessionFactory;
    private static final Logger logger = (Logger) LogManager.getLogger();

    public ParticipantORMRepository(SessionFactory sessionFactory) {
        this.sessionFactory = sessionFactory;
    }

    @Override
    public Optional<Participant> getParticipantByData(Participant participant) {
        try(Session session = sessionFactory.openSession()) {
            Query<Participant> querry = session.createQuery(
                    "FROM Participant WHERE name = :name AND age = :age", Participant.class)
                    .setParameter("name", participant.getName())
                    .setParameter("age", participant.getAge());
            return querry.uniqueResultOptional();
        }catch (Exception e) {
            logger.error(e);
            return Optional.empty();
        }
    }

    @Override
    public void add(Participant entity) throws EntityRepoException {
        Transaction transaction = null;
        try(Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            session.persist(entity);
            transaction.commit();
        }catch(Exception e) {
            if(transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la adăugare: " + e.getMessage());
        }
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        Transaction transaction = null;
        try (Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            Participant participant = session.get(Participant.class, id);
            if (participant != null) {
                session.remove(participant);
            } else {
                throw new EntityRepoException("Participant cu id " + id + " nu a fost găsit.");
            }
            transaction.commit();
        } catch (Exception e) {
            if (transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la ștergerea participantului: " + e.getMessage());
        }
    }


    @Override
    public List<Participant> getAll() throws EntityRepoException {
        try(Session session = sessionFactory.openSession()) {
            Query<Participant> query = session.createQuery("FROM Participant", Participant.class);
            return query.list();
        }catch(Exception e) {
            logger.error(e);
            throw new EntityRepoException("Eroare la obtinerea participantilor. " + e.getMessage());
        }
    }

    @Override
    public Participant findById(long id) throws EntityRepoException {
        try (Session session = sessionFactory.openSession()) {
            return session.get(Participant.class, id);
        } catch (Exception exception) {
            logger.error(exception);
            throw new EntityRepoException("Eroare la căutare după id."+ exception.getMessage());
        }
    }

    @Override
    public void update(long id, Participant entity) throws EntityRepoException {
        Transaction transaction = null;
        try(Session session = sessionFactory.openSession()) {
            transaction = session.beginTransaction();
            session.merge(entity);
            transaction.commit();
        }catch (Exception e) {
            if (transaction != null) {
                transaction.rollback();
            }
            logger.error(e);
            throw new EntityRepoException("Eroare la actualizare. " + e.getMessage());
        }
    }
}
