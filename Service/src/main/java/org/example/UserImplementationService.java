package org.example;

import java.util.List;
import java.util.Optional;

public class UserImplementationService implements UserService {
    private final UserRepository userRepository;

    public UserImplementationService(final UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public boolean login(String username, String password) {
        return userRepository.checkUserPassword(new User(username,password));
    }

    @Override
    public Optional<User> getLogin(String username, String password) {
        return userRepository.getByUsernameAndPassword(username,password);
    }

    @Override
    public void add(User entity) throws EntityRepoException {
        userRepository.add(entity);
    }

    @Override
    public void remove(Long id) throws EntityRepoException {
        userRepository.remove(id);
    }

    @Override
    public void update(Long id, User entity) throws EntityRepoException {
        userRepository.update(id,entity);
    }

    @Override
    public List<User> getAll() throws EntityRepoException{
        return userRepository.getAll();
    }

    @Override
    public User findById(Long id) throws EntityRepoException {
        return userRepository.findById(id);
    }
}
