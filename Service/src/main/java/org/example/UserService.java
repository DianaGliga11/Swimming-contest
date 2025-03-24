package org.example;

public interface UserService extends Service<Long, User> {
    boolean login(String username, String password);
}
