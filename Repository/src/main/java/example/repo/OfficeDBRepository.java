package example.repo;

import Utils.JdbcUtils;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class OfficeDBRepository implements OfficeRepository {
    private final JdbcUtils dbUtils;
    private ParticipantRepository participantRepository;
    private EventRepository eventRepository;
    private static final Logger logger = LogManager.getLogger();

    public OfficeDBRepository(Properties props, ParticipantRepository participantRepository, EventRepository eventRepository) {
        logger.info("Initializing OfficeDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
        this.participantRepository = participantRepository;
        this.eventRepository = eventRepository;
    }

    @Override
    public Collection<Participant> findParticipantsByEvent(Long eventId) {
        String sql = "SELECT idParticipant, name, age FROM Offices WHERE idEvent = ?";
        Connection connection = dbUtils.getConnection();
        List<Participant> participants = new ArrayList<>();
        try (PreparedStatement preparedStatement = connection.prepareStatement(sql)) {
            preparedStatement.setLong(1, eventId);
            ResultSet rs = preparedStatement.executeQuery();
            while (rs.next()) {
                Long id = rs.getLong("idParticipant");
                String name = rs.getString("name");
                int age = rs.getInt("age");
                Participant participant = new Participant(name, age);
                participant.setId(id);
                participants.add(participant);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return participants;
    }

    @Override
    public int countEventsForParticipant(Long participantId) throws EntityRepoException {
        String sql = "SELECT COUNT(*) FROM Offices WHERE idParticipant = ?";
        Connection connection = dbUtils.getConnection();
        try (PreparedStatement preparedStatement = connection.prepareStatement(sql)) {
            preparedStatement.setLong(1, participantId);
            ResultSet resultSet = preparedStatement.executeQuery();

            return resultSet.next() ? resultSet.getInt(1) : 0;
        } catch (SQLException e) {
            throw new EntityRepoException(e);
        }
    }


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
        String sql = "UPDATE Offices SET idEvent=?, style=?, distance=?, idParticipant=?, name=?, age=? WHERE id=?";
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
        logger.traceEntry("deleting task of participantID={}, eventID={} ", participantID, eventID);
        Connection connection = dbUtils.getConnection();
        try (PreparedStatement preparedStatement = connection.prepareStatement(
                "Delete from Offices where idParticipant=? and idEvent=?")) {
            preparedStatement.setLong(1, participantID);
            preparedStatement.setLong(2, eventID);
            preparedStatement.executeUpdate();
            logger.traceExit("Offices {} deleted", participantID);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error deleting task of participantID=" + participantID);
        }
    }

    private Office extract(ResultSet resultSet) throws SQLException, EntityRepoException {
        Long eventId = resultSet.getLong("idEvent");
        String eventStyle = resultSet.getString("style");
        Integer eventDistance = resultSet.getInt("distance");
        Event event = new Event(eventStyle, eventDistance);
        event.setId(eventId);
        Long participantId = resultSet.getLong("idParticipant");
        String participantName = resultSet.getString("name");
        Integer participantAge = resultSet.getInt("age");
        Participant participant = new Participant(participantName, participantAge);
        participant.setId(participantId);
        Office entry = new Office(participant, event);
        return entry;
    }

}