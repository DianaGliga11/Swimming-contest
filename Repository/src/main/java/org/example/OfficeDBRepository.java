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
    private ParticipantRepository participantRepository;
    private EventRepository eventRepository;
    private static final Logger logger = LogManager.getLogger();

    public OfficeDBRepository(Properties props, ParticipantRepository participantRepository, EventRepository eventRepository) {
        logger.info("Initializing OfficeDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
        this.participantRepository = participantRepository;
        this.eventRepository = eventRepository;
    }

//    private String serializeParticipant(Participant participant) {
//        return participant.getId() + "-" + participant.getName() + "-" + participant.getAge();
//    }
//
//    private String serializeEvent(Event event) {
//        return event.getId() + "-" + event.getStyle() + "-" + event.getDistance();
//    }
//
//    private Participant deserializeParticipant(String serializedParticipant) {
//        if (serializedParticipant == null || serializedParticipant.isEmpty()) {
//            return null;
//        }
//        String[] parts = serializedParticipant.split("-");
//        long id = Long.parseLong(parts[0]);
//        String name = parts[1];
//        int age = Integer.parseInt(parts[2]);
//        Participant participant = new Participant(name, age);
//        participant.setId(id);
//        return participant;
//    }
//
//    private Event deserializeEvent(String serializedEvent) {
//        if (serializedEvent == null || serializedEvent.isEmpty()) {
//            return null;
//        }
//        String[] parts = serializedEvent.split("-");
//        long id = Long.parseLong(parts[0]);
//        String style = parts[1];
//        int distance = Integer.parseInt(parts[2]);
//        Event event = new Event(style, distance);
//        event.setId(id);
//        return event;
//    }

    @Override
    public void add(Office entity) throws EntityRepoException {
        logger.traceEntry("Adding office {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Offices(idEvent, style, distance, idParticipant, name, age) VALUES (?,?,?,?,?,?)";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, entity.getEvent().getId());
            ps.setString(2, entity.getEvent().getStyle());
            ps.setInt(3, entity.getEvent().getDistance());
            ps.setLong(4, entity.getParticipant().getId());
            ps.setString(5, entity.getParticipant().getName());
            ps.setInt(6, entity.getParticipant().getAge());
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
                Office office = extract(rs);
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
                    Office office = extract(rs);
                    logger.traceExit("Found office {}", office);
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
        String sql ="UPDATE Offices SET idEvent=?, style=?, distance=?, idParticipant=?, name=?, age=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, entity.getEvent().getId());
            ps.setString(2, entity.getEvent().getStyle());
            ps.setInt(3, entity.getEvent().getDistance());
            ps.setLong(4, entity.getParticipant().getId());
            ps.setString(5, entity.getParticipant().getName());
            ps.setInt(6, entity.getParticipant().getAge());
            ps.setLong(7, id);
            ps.executeUpdate();
            logger.traceExit("Office {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public Collection<Office> getEntriesByEvent(Long eventID) {
        logger.traceEntry("getEntriesByEvent with task od id={} ", eventID);
        Connection connection = dbUtils.getConnection();
        Collection<Office> offices = new ArrayList<>();
        try (PreparedStatement ps = connection.prepareStatement(
                "SELECT * FROM Offices WHERE idEvent=?")) {

            ps.setLong(1, eventID);

            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    Office office = extract(rs);
                    offices.add(office);
                }
            }
            logger.traceExit("Found {} offices for event {}", eventID);
        } catch (SQLException | EntityRepoException e) {
            logger.error(e);
            System.err.println("Error getting offices by event: " + e.getMessage());
        }
        return offices;
    }

    @Override
    public void deleteByIDs(Long participantID, Long eventID) {
        logger.traceEntry("deleting task of participantID={}, eventID={} ", participantID,eventID);
        Connection connection=dbUtils.getConnection();
        try(PreparedStatement preparedStatement = connection.prepareStatement(
                "Delete from Offices where idParticipant=? and idEvent=?")){
            preparedStatement.setLong(1,participantID);
            preparedStatement.setLong(2,eventID);
            preparedStatement.executeUpdate();
            logger.traceExit("Offices {} deleted", participantID);
        }
        catch (SQLException e) {
            logger.error(e);
            System.err.println("Error deleting task of participantID=" + participantID);
        }
    }

    private Office extract(ResultSet resultSet) throws SQLException, EntityRepoException {
        Long id = resultSet.getLong("id");
        Long participantId = resultSet.getLong("idParticipant");
        Long eventId = resultSet.getLong("idEvent");

        final Event event = eventRepository.findById(eventId);
        final Participant participant = participantRepository.findById(participantId);

        if (participant != null && event != null) {
            Office entry = new Office(participant, event);
            entry.setId(id);
            return entry;
        } else {
            throw new SQLException("Database error: Unable to reference participant and/or event for EventParticipantEntry");
        }
    }

    private Event extractEvent(ResultSet resultSet) throws SQLException, EntityRepoException {
        Long id = resultSet.getLong("id");
        String style = resultSet.getString("style");
        Integer distance = resultSet.getInt("distance");
        Event event = new Event(style, distance);
        event.setId(id);
        return event;
    }
}