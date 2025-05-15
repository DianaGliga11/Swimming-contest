package example.repo;

import example.model.Identifiable;

import java.io.Serializable;
import java.util.List;

public interface Repository<E extends Identifiable<Long>> extends Serializable {
    void add(E entity) throws EntityRepoException;

    void remove(long id) throws EntityRepoException;

    List<E> getAll() throws EntityRepoException;

    E findById(long id) throws EntityRepoException;

    void update(long id, E entity) throws EntityRepoException;
}
