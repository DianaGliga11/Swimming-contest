package example.repo;

import Utils.JdbcUtils;
import example.model.Participant;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.Properties;

public class ParticipantDBRepository implements ParticipantRepository {
    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public ParticipantDBRepository(Properties props) {
        logger.info("Initializing ParticipantDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(Participant entity) throws EntityRepoException {
        logger.traceEntry("add task {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Participants(name,age) VALUES (?,?)";
        try (var ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getName());
            ps.setInt(2, entity.getAge());
            ps.executeUpdate();
            logger.traceExit("task {} added", entity);
        } catch (Exception e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        logger.traceEntry("remove task {} ", id);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Participants WHERE id=?";
        try (var ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            ps.executeUpdate();
            logger.traceExit("task {} removed", id);
        } catch (Exception e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public List<Participant> getAll() throws EntityRepoException {
        logger.traceEntry("task getALL");
        Connection connection = dbUtils.getConnection();
        List<Participant> participants = new ArrayList<>();
        String sql = "SELECT * FROM Participants";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    Participant participant = extract(rs);
                    participants.add(participant);
                    logger.traceExit("task {} added", participant);
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        return participants;
    }

    @Override
    public Participant findById(long id) throws EntityRepoException {
        logger.traceEntry("task findById {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Participants WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    Participant participant = extract(rs);
                    logger.traceExit("task {} found", participant);
                    return participant;
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        return null;
    }

    @Override
    public void update(long id, Participant entity) throws EntityRepoException {
        logger.traceEntry("update task {} ", entity);
        if (entity.getId() == null || entity.getId() < 0) {
            throw new EntityRepoException("Cannot update participant with null ID.");
        }
        Connection connection = dbUtils.getConnection();
        String sql = "UPDATE Participants SET name=?, age=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getName());
            ps.setInt(2, entity.getAge());
            ps.setLong(3, id);
            ps.executeUpdate();
            logger.traceExit("task {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    private Participant extract(ResultSet resultSet) throws SQLException {
        Long id = resultSet.getLong("id");
        String name = resultSet.getString("name");
        int age = resultSet.getInt("age");
        Participant participant = new Participant(name, age);
        participant.setId(id);
        return participant;
    }

    @Override
    public Optional<Participant> getParticipantByData(Participant participant) {
        logger.traceEntry("task getParticipantByData {}", participant);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Participants WHERE name=? AND age=?";
        try (PreparedStatement preparedStatement = connection.prepareStatement(sql)) {
            preparedStatement.setString(1, participant.getName());
            preparedStatement.setInt(2, participant.getAge());
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if (rs.next()) {
                    Participant part = extract(rs);
                    logger.traceExit("task {} found", participant);
                    return Optional.of(part);
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        return Optional.empty();
    }

}

