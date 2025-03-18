package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class EventDBRepository implements EventRepository {

    private JdbcUtils dbUtils;
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
    public void remove(Event entity) throws EntityRepoException {
        logger.traceEntry("remove task {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Events WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, entity.getId());
            ps.executeUpdate();
            logger.traceExit("task {} removed", entity);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
            throw new EntityRepoException(e);
        }
        logger.traceExit("task {} removed", entity);
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
                    int id = rs.getInt("id");
                    String style = rs.getString("style");
                    int distance = rs.getInt("distance");
                    Event event = new Event(style, distance);
                    event.setId(id);
                    events.add(event);
                    logger.traceExit("task {} added", event);
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
    public Event findById(int id) throws EntityRepoException {
        logger.traceEntry("task findById {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Events WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    String style = rs.getString("style");
                    int distance = rs.getInt("distance");
                    Event event = new Event(style, distance);
                    event.setId(id);
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
    public void update(Integer id, Event entity) throws EntityRepoException {
        logger.traceEntry("update task {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "UPdate Events SET style=?, distance=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getStyle());
            ps.setInt(2, entity.getDistance());
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
