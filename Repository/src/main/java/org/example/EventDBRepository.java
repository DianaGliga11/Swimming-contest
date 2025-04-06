package org.example;

import Utils.JdbcUtils;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class EventDBRepository implements EventRepository {

    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public EventDBRepository(Properties props) {
        logger.info("Initializing EventDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(Event entity) throws EntityRepoException {
        logger.traceEntry("add task {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Events(style, distance) values (?,?)";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getStyle());
            ps.setInt(2, entity.getDistance());
            ps.executeUpdate();
            logger.traceExit("task {} added", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        logger.traceExit("task {} added", entity);
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        logger.traceEntry("remove task {} ", id);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Events WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            ps.executeUpdate();
            logger.traceExit("task {} removed", id);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        logger.traceExit("task {} removed", id);
    }

    @Override
    public List<Event> getAll() throws EntityRepoException {
        logger.traceEntry("task getALL");
        Connection connection = dbUtils.getConnection();
        List<Event> events = new ArrayList<>();
        String sql = "SELECT * FROM Events";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    Event event = extract(rs);
                    events.add(event);
                    logger.traceExit("task {} added", event);
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        return events;
    }

    @Override
    public Event findById(long id) throws EntityRepoException {
        logger.traceEntry("task findById {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Events WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    Event event = extract(rs);
                    logger.traceExit("task {} found", event);
                    return event;
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
    public void update(long id, Event entity) throws EntityRepoException {
        logger.traceEntry("update task {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "UPdate Events SET style=?, distance=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getStyle());
            ps.setInt(2, entity.getDistance());
            ps.setLong(3, entity.getId());
            ps.executeUpdate();
            logger.traceExit("task {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
    }

    private Event extract(ResultSet resultSet) throws SQLException {
        Long id = resultSet.getLong("id");
        String style = resultSet.getString("style");
        int distance = resultSet.getInt("distance");
        Event event = new Event(style, distance);
        event.setId(id);
        return event;
    }
}
