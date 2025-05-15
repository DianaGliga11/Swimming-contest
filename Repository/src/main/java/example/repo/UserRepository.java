package example.repo;

import example.model.User;

import java.util.Optional;

public interface UserRepository extends Repository<User> {
    boolean checkUserPassword(User user);

    Optional<User> getByUsernameAndPassword(String username, String password);
}
