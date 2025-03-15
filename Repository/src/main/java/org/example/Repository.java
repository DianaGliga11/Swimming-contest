package org.example;

import java.util.List;

public interface Repository<E extends Entity<Integer>> {
    void add(E entity) throws EntityRepoException;
    void remove(E entity) throws EntityRepoException;
    List<E> getAll() throws EntityRepoException ;
    E findById(int id) throws EntityRepoException;
    void update(Integer id, E entity) throws EntityRepoException;
}
