package example.model;

import java.io.Serializable;

public class User extends Identifiable<Long> implements Serializable {
    private String userName;
    private String password;

    public User(String userName, String password) {
        this.userName = userName;
        this.password = password;
    }

    public String getUserName() {
        return userName;
    }

    public String getPassword() {
        return password;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    @Override
    public String toString() {
        return "User{" + super.toString() +
                "userName='" + userName + '\'' +
                ", password='" + password + '\'' +
                '}';
    }
}
