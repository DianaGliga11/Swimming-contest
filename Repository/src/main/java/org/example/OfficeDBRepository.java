package org.example;

import Utils.JdbcUtils;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class OfficeDBRepository implements OfficeRepository {
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public OfficeDBRepository(Properties props) {
        logger.info("Initializing OfficeDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    private String serializeParticipant(Participant participant) {
        return participant.getId() + "-" + participant.getName() + "-" + participant.getAge();
    }

    private String serializeEvent(Event event) {
        return event.getId() + "-" + event.getStyle() + "-" + event.getDistance();
    }

    private Participant deserializeParticipant(String serializedParticipant) {
        if (serializedParticipant == null || serializedParticipant.isEmpty()) {
            return null;
        }
        String[] parts = serializedParticipant.split("-");
        long id = Long.parseLong(parts[0]);
        String name = parts[1];
        int age = Integer.parseInt(parts[2]);
        Participant participant = new Participant(name, age);
        participant.setId(id);
        return participant;
    }

    private Event deserializeEvent(String serializedEvent) {
        if (serializedEvent == null || serializedEvent.isEmpty()) {
            return null;
        }
        String[] parts = serializedEvent.split("-");
        long id = Long.parseLong(parts[0]);
        String style = parts[1];
        int distance = Integer.parseInt(parts[2]);
        Event event = new Event(style, distance);
        event.setId(id);
        return event;
    }

    @Override
    public void add(Office entity) throws EntityRepoException {
        logger.traceEntry("Adding office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Offices(participant, event) VALUES (?,?)";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, serializeParticipant(entity.getParticipant()));
            ps.setString(2, serializeEvent(entity.getEvent()));
            ps.executeUpdate();
            logger.traceExit("Office {} added", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        logger.traceEntry("Removing office {} ", id);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Offices WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            ps.executeUpdate();
            logger.traceExit("Office {} removed", id);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public List<Office> getAll() throws EntityRepoException {
        logger.traceEntry("Fetching all offices");
        Connection connection = dbUtils.getConnection();
        List<Office> offices = new ArrayList<>();
        String sql = "SELECT * FROM Offices";
        try (PreparedStatement ps = connection.prepareStatement(sql);
             ResultSet rs = ps.executeQuery()) {
            while (rs.next()) {
                long id = rs.getLong("id");
                String participantStr = rs.getString("participant");
                String eventStr = rs.getString("event");

                Participant participant = deserializeParticipant(participantStr);
                Event event = deserializeEvent(eventStr);

                Office office = new Office(participant, event);
                office.setId(id);
                offices.add(office);
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        return offices;
    }

    @Override
    public Office findById(long id) throws EntityRepoException {
        logger.traceEntry("Finding office by ID {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Offices WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    String participantStr = rs.getString("participant");
                    String eventStr = rs.getString("event");

                    Participant participant = deserializeParticipant(participantStr);
                    Event event = deserializeEvent(eventStr);

                    Office office = new Office(participant, event);
                    office.setId(id);
                    return office;
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
        return null;
    }

    @Override
    public void update(long id, Office entity) throws EntityRepoException {
        logger.traceEntry("Updating office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "UPDATE Offices SET participant=?, event=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, serializeParticipant(entity.getParticipant()));
            ps.setString(2, serializeEvent(entity.getEvent()));
            ps.setLong(3, id);
            ps.executeUpdate();
            logger.traceExit("Office {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public Map<Event, Integer> getEventsWithParticipantsCount(Integer eventID) {
        logger.traceEntry("Fetching participant by event ID {}", eventID);
        Connection connection = dbUtils.getConnection();
        Map<Event, Integer> eventParticipantsMap = new HashMap<>();
        String eventQuery = "SELECT id, style, distance FROM Events WHERE id = ?";

        try (PreparedStatement eventStmt = connection.prepareStatement(eventQuery)) {
            eventStmt.setInt(1, eventID);
            ResultSet eventResult = eventStmt.executeQuery();

            if (eventResult.next()) {
                // Creăm un obiect Event pe baza datelor obținute
                int distance = eventResult.getInt("distance");
                String style = eventResult.getString("style");
                Event event = new Event(style, distance);
                event.setId((long) eventID); // Setăm ID-ul evenimentului

                // Query pentru a număra de câte ori apare evenimentul în tabelul Offices (adică numărul de participanți)
                String officeQuery = "SELECT COUNT(*) FROM Offices WHERE id = ?";

                try (PreparedStatement officeStmt = connection.prepareStatement(officeQuery)) {
                    officeStmt.setInt(1, eventID);  // Căutăm după event_id
                    ResultSet officeResult = officeStmt.executeQuery();

                    if (officeResult.next()) {
                        // Obținem numărul de participanți
                        int participantCount = officeResult.getInt(1);

                        // Adăugăm rezultatul în Map
                        eventParticipantsMap.put(event, participantCount);
                    }
                } catch (SQLException e) {
                    e.printStackTrace();
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return eventParticipantsMap;
    }

}