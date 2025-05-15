package example.example;

import example.model.User;

import java.util.Optional;

public interface UserService extends Service<Long, User> {
    boolean login(String username, String password);

    Optional<User> getLogin(String username, String password);
}
