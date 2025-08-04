package org.example;

import java.util.List;

public interface I_Repository<E extends Entity<Integer>> {
    void add(E entity);
    void remove(E entity);
    List<E> getAll();
    E findById(int id);
}
