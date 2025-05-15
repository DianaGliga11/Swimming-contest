package contestUtils;

import example.example.*;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import example.model.User;
import example.repo.EntityRepoException;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import DTO.EventDTO;
import DTO.ParticipantDTO;

import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ContestServices implements IContestServices{
    private final UserService userService;
    private final ParticipantService participantService;
    private final EventService eventService;
    private final Map<String, IMainObserver> loggedClientes;
    private static final int DEFAULT_THREADS = 8;
    private static final Logger logger = LogManager.getLogger();

    public ContestServices(EventService eventService, ParticipantService participantService, UserService userService) {
        this.eventService = eventService;
        this.participantService = participantService;
        this.userService = userService;
        loggedClientes = new ConcurrentHashMap<>();
    }

    @Override
    public synchronized User login(String userName, String password, IMainObserver client) throws Exception {
        Optional<User> foundUser = userService.getLogin(userName, password);
        if (foundUser.isPresent()) {
            if(loggedClientes.get(userName) != null) {
                logger.error("User already logged in");
                throw new Exception("User already logged in");
            }
            loggedClientes.put(userName, client);
        }
        else{
            logger.error("Authentication failed");
            throw new Exception("Authentication failed");
        }
        return foundUser.get();
    }

    @Override
    public synchronized void logout(User user, IMainObserver client) throws Exception {
        IMainObserver localClient = loggedClientes.remove(user.getUserName());
        if(localClient == null) {
            logger.error("User: " + user.getUserName() + " is not logged in");
            throw new Exception("User : " + user.getUserName() + " is not logged in");
        }
    }

    @Override
    public synchronized Collection<EventDTO> getEventsWithParticipantsCount() throws EntityRepoException {
        return eventService.getEventsWithParticipantsCount();
    }

    @Override
    public synchronized Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws EntityRepoException {
        return eventService.getParticipantsForEventWithCount(eventId);
    }

    @Override
    public void saveEventEntry(List<Office> offices) {
        for (Office office : offices) {
            try {
                eventService.saveEventEntry(office);
            }catch(EntityRepoException e) {
                logger.error("Failed to save event entry for office: " + office, e);
            }
        }

        ExecutorService executor = Executors.newFixedThreadPool(DEFAULT_THREADS);
        for (IMainObserver client : loggedClientes.values()) {
            executor.execute(() -> {
                try {
                    client.newRegistration(eventService.getEventsWithParticipantsCount().stream().toList());
                } catch (Exception e) {
                    logger.error(e);
                }
            });
        }
        executor.shutdown();
    }

    @Override
    public synchronized Collection<Participant> findAllParticipants() throws EntityRepoException {
        return participantService.getAll();
    }


    @Override
    public void saveParticipant(Participant participant) {
        try {
            participantService.add(participant);
            ExecutorService executor = Executors.newFixedThreadPool(DEFAULT_THREADS);
            for (IMainObserver client : loggedClientes.values()) {
                executor.execute(() -> {
                    try {
                        client.participantAdded(participant);
                    } catch (Exception e) {
                        logger.error(e);
                    }
                });
            }
            executor.shutdown();
        }catch(EntityRepoException e) {
            logger.error("Failed to save participant", e);
        }
    }

    @Override
    public synchronized Collection<Event> findAllEvents() throws Exception {
        try {
            return eventService.getAll();
        } catch (EntityRepoException e) {
            logger.error("Failed to load events", e);
            throw new Exception("Could not load events");
        }
    }

    @Override
    public void saveEvent(Event event) throws Exception {

    }
}
