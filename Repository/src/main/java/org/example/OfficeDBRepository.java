package org.example;

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

    private String serializeParticipants(List<Participant> participants) {
        StringBuilder builder = new StringBuilder();
        for (Participant participant : participants) {
            builder.append(participant.getId()).append("-").append(
                    participant.getName()).append("-").append(participant.getAge()).append(";");
        }
        return builder.toString();
    }

    private String serializeEvents(List<Event> events) {
        StringBuilder builder = new StringBuilder();
        for (Event event : events) {
            builder.append(event.getId()).append("-").append(
                    event.getStyle()).append("-").append(event.getDistance()).append(";");
        }
        return builder.toString();
    }

    private List<Participant> deserializeParticipants(String serializedParticipants) {
        List<Participant> participants = new ArrayList<>();
        String[] pairs = serializedParticipants.split(";");
        for (String pair : pairs) {
            if (!pair.isEmpty()) {
                String[] parts = pair.split("-");
                int id = Integer.parseInt(parts[0]);
                String name = parts[1];
                int age = Integer.parseInt(parts[2]);
                Participant participant = new Participant(name, age);
                participant.setId(id);
                participants.add(participant);
            }
        }
        return participants;
    }

    private List<Event> deserializeEvents(String serializedEvents) {
        List<Event> events = new ArrayList<>();
        String[] pairs = serializedEvents.split(";");
        for (String pair : pairs) {
            if (!pair.isEmpty()) {
                String[] parts = pair.split("-");
                int id = Integer.parseInt(parts[0]);
                String style = parts[1];
                int distance = Integer.parseInt(parts[2]);
                Event event = new Event(style, distance);
                event.setId(id);
                events.add(event);
            }
        }
        return events;
    }

    private Map<Integer, Integer> countParticipantProbes(String data) {
        Map<Integer, Integer> probesCount = new HashMap<>();
        if (data == null || data.isEmpty()) return probesCount;

        String[] events = data.split(",");
        for (String event : events) {
            String[] parts = event.split(":");
            if (parts.length >= 2) {
                int participantId = Integer.parseInt(parts[0]);
                probesCount.put(participantId, probesCount.getOrDefault(participantId, 0) + 1);
            }
        }
        return probesCount;
    }


//    @Override
//    public List<Map<String,Object>> findParticipantsByEvent(Event event) throws EntityRepoException {
//        logger.traceEntry("task findParticipantsByEvent {}", event);
//        Connection connection = dbUtils.getConnection();
//        List<Map<String, Object>> results = new ArrayList<>();
//        String sql = "SELECT participants, events FROM Office WHERE events LIKE ?";
//        try (PreparedStatement ps = connection.prepareStatement(sql)) {
//            ps.setString(1, "%" + event.getId() + ":" + event.getStyle() + "%");
//            try (ResultSet rs = ps.executeQuery()) {
//                while (rs.next()) {
//                    String participantsStr = rs.getString("participants");
//                    String eventsStr = rs.getString("events");
//                    List<Participant> participants = deserializeParticipants(participantsStr);
//                    Map<Integer, Integer> participantProbesCount = countParticipantProbes(eventsStr);
//
//                    for (Participant participant : participants) {
//                        Map<String, Object> participantInfo = new HashMap<>();
//                        participantInfo.put("Nume", participant.getName());
//                        participantInfo.put("Vârstă", participant.getAge());
//                        participantInfo.put("Număr Probele", participantProbesCount.getOrDefault(participant.getId(), 0));
//
//                        results.add(participantInfo);
//                    }
//                }
//            }
//        } catch (SQLException e) {
//            logger.error(e);
//            throw new EntityRepoException(e);
//        }
//
//        return results;
//    }

    @Override
    public void add(Office entity) throws EntityRepoException {
        logger.traceEntry("add office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Offices(participants, events) VALUES (?,?)";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, serializeParticipants(entity.getParticipants()));
            ps.setString(2, serializeEvents(entity.getEvents()));
            ps.executeUpdate();
            logger.traceExit("task {} added", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public void remove(Office entity) throws EntityRepoException {
        logger.traceEntry("remove office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Offices WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, entity.getId());
            ps.executeUpdate();
            logger.traceExit("task {} removed", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public List<Office> getAll() throws EntityRepoException {
        logger.traceEntry("task getALL");
        Connection connection = dbUtils.getConnection();
        List<Office> offices = new ArrayList<>();
        String sql = "SELECT * FROM Offices";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    int id = rs.getInt("id");
                    String participants = rs.getString("participants");
                    String events = rs.getString("events");
                    List<Participant> participantsList = deserializeParticipants(participants);
                    List<Event> eventsList = deserializeEvents(events);
                    Office office = new Office(participantsList, eventsList);
                    office.setId(id);
                    offices.add(office);
                    logger.traceExit("task getAll");
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        return List.of();
    }

    @Override
    public Office findById(int id) throws EntityRepoException {
        logger.traceEntry("task findById {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Offices WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    String participantsStr = rs.getString("participants");
                    String eventsStr = rs.getString("events");

                    List<Participant> participants = deserializeParticipants(participantsStr);
                    List<Event> events = deserializeEvents(eventsStr);

                    Office office = new Office(participants, events);
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
    public void update(Office entity) throws EntityRepoException {
        logger.traceEntry("update office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "UPDATE Offices SET participants=?, events=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, serializeParticipants(entity.getParticipants()));
            ps.setString(2, serializeEvents(entity.getEvents()));
            ps.setInt(3, entity.getId());
            ps.executeUpdate();
            logger.traceExit("task {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }
}
