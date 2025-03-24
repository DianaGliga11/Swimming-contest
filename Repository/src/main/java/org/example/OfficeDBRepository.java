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

//    @Override
//    public Map<Event, Integer> getEventsWithParticipantsCount() {
//        logger.info("Get events and number of participants");
//        Map<Event, Integer> eventsCount = new HashMap<>();
//        String query = "SELECT e.id, e.distance, e.style, COUNT(o.participant_id) as numParticipants " +
//                "FROM Events e " +
//                "LEFT JOIN Offices o ON e.id = o.event_id " +
//                "GROUP BY e.id";
//
//        try (Connection con = dbUtils.getConnection();
//             PreparedStatement statement = con.prepareStatement(query);
//             ResultSet resultSet = statement.executeQuery()) {
//
//            while (resultSet.next()) {
//                Long eventId = resultSet.getLong("id");
//                int distance = resultSet.getInt("distance");
//                String style = resultSet.getString("style");
//                int numParticipants = resultSet.getInt("numParticipants");
//
//                Event event = new Event(style, distance);
//                event.setId(eventId);
//                eventsCount.put(event, numParticipants);
//            }
//        } catch (SQLException e) {
//            e.printStackTrace();
//        }
//        return eventsCount;
//    }

//    public List<Participant> findParticipantsByEvent(Long eventId) {
//        logger.info("Find participant by event {}", eventId);
//        List<Participant> participants = new ArrayList<>();
//        String query = "SELECT p.id, p.name, p.age FROM Participants p " +
//                "JOIN Offices o ON p.id = o.participant_id " +
//                "WHERE o.event_id = ?";
//
//        try (Connection con = dbUtils.getConnection();
//             PreparedStatement statement = con.prepareStatement(query)) {
//            statement.setLong(1, eventId);
//            ResultSet resultSet = statement.executeQuery();
//
//            while (resultSet.next()) {
//                Long id = resultSet.getLong("id");
//                String name = resultSet.getString("name");
//                int age = resultSet.getInt("age");
//                Participant participant = new Participant(name, age);
//                participant.setId(id);
//                participants.add(participant);
//            }
//        } catch (SQLException e) {
//            e.printStackTrace();
//        }
//        return participants;
//    }
//
//    @Override
//    public void registerParticipantToEvents(Long participantId, List<Long> eventIds) {
//        String query = "INSERT INTO Offices (participant_id, event_id) VALUES (?, ?)";
//
//        try (Connection con = dbUtils.getConnection();
//             PreparedStatement statement = con.prepareStatement(query)) {
//
//            for (Long eventId : eventIds) {
//                statement.setLong(1, participantId);
//                statement.setLong(2, eventId);
//                statement.addBatch();
//            }
//            statement.executeBatch();
//        } catch (SQLException e) {
//            e.printStackTrace();
//        }
//    }


}
