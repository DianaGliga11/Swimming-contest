package org.example;

public class User extends Entity<Integer> {
    private int id;
    private String userName;
    private String password;

    public User(int id, String userName, String password) {
        super(id);
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

    public String toString() {
        return this.userName + "," + this.password;
    }

    @Override
    public Integer getId() {
        return this.id;
    }

}
