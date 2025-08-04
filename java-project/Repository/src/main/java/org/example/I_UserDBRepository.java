package org.example;

public interface I_UserDBRepository extends  I_Repository<User>{
    boolean checkUserPassword(User user);
}
