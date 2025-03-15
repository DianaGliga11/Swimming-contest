package org.example;

public interface UserRepository extends Repository<User> {
    boolean checkUserPassword(User user);
}
