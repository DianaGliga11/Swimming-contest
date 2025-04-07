package response;

import example.example.User;

public class OKResponse implements Response {
    private final User user;

    public OKResponse(User user) {
        this.user = user;
    }

    public User getUser() {
        return user;
    }
}
