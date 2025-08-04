package org.example;

import java.io.Serializable;

public abstract class Entity<T> implements Serializable {
    protected T id;
    public Entity(T id){
        this.id = id;
    }

    public T getId() {
        return id;
    }

//    public void setId(T id) {
//        this.id = id;
//    }

    @Override
    public String toString() {
        return "Entity{" +
                "id=" + id +
                '}';
    }

}
