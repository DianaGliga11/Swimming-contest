package example.model;

import example.model.Identifiable;
import jakarta.persistence.*;
        import jakarta.validation.constraints.NotNull;

import java.io.Serializable;

//@Entity
//@Table(name = "Participants")
//public class Participant extends Identifiable<Long> implements Serializable {
//    @Id
//    @GeneratedValue(strategy = GenerationType.SEQUENCE)
//    private Long id;
//    private String name;
//    private int age;
//
//    public Participant(String name, int age) {
//        this.name = name;
//        this.age = age;
//    }
//
//    public Participant() {
//
//    }
//
//    @Override
//    public Long getId() {
//        return this.id;
//    }
//
//    @Override
//    public void setId(Long id) {
//        this.id = id;
//    }
//
//    @NotNull
//    public String getName() {
//        return this.name;
//    }
//
//    @NotNull
//    public int getAge() {
//        return this.age;
//    }
//
//
//    public void setName(String name) {
//        this.name = name;
//    }
//
//    public void setAge(int age) {
//        this.age = age;
//    }
//
//    @Override
//    public String toString() {
//        return "Participant{" + super.toString() +
//                " name='" + name + '\'' +
//                ", age=" + age +
//                '}';
//    }
//
//}

public class Participant extends Identifiable<Long> {
    private String name;
    private int age;

    public Participant(String name, int age) {
        this.name = name;
        this.age = age;
    }

    public String getName() {
        return this.name;
    }

    public int getAge() {
        return this.age;
    }


    public void setName(String name) {
        this.name = name;
    }

    public void setAge(int age) {
        this.age = age;
    }

    @Override
    public String toString() {
        return "Participant{" + super.toString() +
                " name='" + name + '\'' +
                ", age=" + age +
                '}';
    }

}
