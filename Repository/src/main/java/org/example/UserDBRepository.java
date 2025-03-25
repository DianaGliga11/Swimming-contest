package org.example;

import Utils.JdbcUtils;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.core.Logger;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.Properties;

public class UserDBRepository implements UserRepository {
    private final JdbcUtils dbUtils;
    private static final Logger logger = (Logger) LogManager.getLogger();

    public UserDBRepository(Properties props) {
        logger.info("Initializing UserDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(User entity) throws EntityRepoException {
        logger.traceEntry("add user {} ", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "INSERT INTO Users(username, password) VALUES (?, ?)";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getUserName());
            ps.setString(2, entity.getPassword());
            ps.executeUpdate();
            logger.traceExit("User {} added", entity);
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
        logger.traceExit("User {} added", entity);
    }

    @Override
    public void remove(long id) throws EntityRepoException {
        logger.traceEntry("remove user {} ", id);
        Connection connection = dbUtils.getConnection();
        String sql = "DELETE FROM Users WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            ps.executeUpdate();
            logger.traceExit("User {} removed", id);
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
        logger.traceExit("User {} removed", id);
    }

    @Override
    public List<User> getAll() throws EntityRepoException {
        logger.traceEntry("get all users");
        Connection connection = dbUtils.getConnection();
        List<User> users = new ArrayList<>();
        String sql = "SELECT * FROM Users";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    long id = rs.getInt("id");
                    String username = rs.getString("username");
                    String password = rs.getString("password");
                    User user = new User(username, password);
                    user.setId(id);
                    users.add(user);
                    logger.traceExit("user {} added", user);
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
        return users;
    }

    @Override
    public User findById(long id) throws EntityRepoException {
        logger.traceEntry("find user by id {}", id);
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Users WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setLong(1, id);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    String username = rs.getString("username");
                    String password = rs.getString("password");
                    User user = new User(username, password);
                    user.setId(id);
                    logger.traceExit("user found {}", user);
                    return user;
                }
            }
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
        return null;
    }

    @Override
    public void update(long id, User entity) throws EntityRepoException {
        logger.traceEntry("update user {}", entity);
        Connection connection = dbUtils.getConnection();
        String sql = "UPDATE Users SET username=?, password=? WHERE id=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, entity.getUserName());
            ps.setString(2, entity.getPassword());
            ps.setLong(3, entity.getId());
            ps.executeUpdate();
            logger.traceExit("user {} updated", entity);
        } catch (SQLException e) {
            logger.error(e);
            throw new EntityRepoException(e);
        }
    }

    @Override
    public boolean checkUserPassword(User user) {
        logger.traceEntry("check user credentials for {}", user.getUserName());
        Connection connection = dbUtils.getConnection();
        String sql = "SELECT * FROM Users WHERE username=? AND password=?";
        try (PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setString(1, user.getUserName());
            ps.setString(2, user.getPassword());
            try (ResultSet rs = ps.executeQuery()) {
                return rs.next();
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        logger.traceExit("check user credentials for {}", user.getUserName());
        return false;
    }

    @Override
    public Optional<User> getByUsernameAndPassword(String username, String password) {
        logger.traceEntry("getByUsernameAndPassword with username {} ", username);
        Connection connection = dbUtils.getConnection();
        try (PreparedStatement preparedStatement = connection.prepareStatement(
                "SELECT * FROM Users WHERE username=? AND password=?")) {
            preparedStatement.setString(1, username);
            preparedStatement.setString(2, password);
            try (ResultSet resultSet = preparedStatement.executeQuery()) {
                if (resultSet.next()) {
                    User user = extract(resultSet);
                    logger.traceExit(user);
                    return Optional.of(user);
                }
            }
        } catch (SQLException sqlException) {
            logger.error(sqlException);
            System.err.println("DB Error : " + sqlException);
        }
        logger.traceExit("null");
        return Optional.empty();
    }

    private User extract(ResultSet resultSet) throws SQLException {
        Long id = resultSet.getLong("id");
        String username = resultSet.getString("username");
        String passwordToken = resultSet.getString("password");

        User user = new User(username, passwordToken);
        user.setId(id);
        user.setPassword(passwordToken);
        return user;
    }

}
