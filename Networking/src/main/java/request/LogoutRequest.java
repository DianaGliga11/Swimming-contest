package request;

import example.example.User;

public class LogoutRequest implements Request {
    public final User user;

    public LogoutRequest(User user) {
        this.user = user;
    }

    public User getUser() {
        return user;
    }
}
