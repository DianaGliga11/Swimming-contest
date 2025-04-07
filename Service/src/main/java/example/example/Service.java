package example.example;

import java.util.List;

public interface Service<ID, E extends Entity<ID>> {
    void add(E entity) throws EntityRepoException;

    void remove(ID id) throws EntityRepoException;

    void update(ID id, E entity) throws EntityRepoException;

    List<E> getAll() throws EntityRepoException;

    E findById(ID id) throws EntityRepoException;
}
