package org.example;

import java.util.List;

public interface Repository<E extends Entity<Long>> {
    void add(E entity) throws EntityRepoException;

    void remove(long id) throws EntityRepoException;

    List<E> getAll() throws EntityRepoException;

    E findById(long id) throws EntityRepoException;

    void update(long id, E entity) throws EntityRepoException;
}
