package org.example;

public class Participant extends Entity<Integer> {
    private String name;
    private int age;
//    private List<Event> registeredEvents;

    public Participant(int id, String name, int age) {
        super(id);
        this.name = name;
        this.age = age;
        //       this.registeredEvents = registeredEvents;
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

//    public List<Event> getRegisteredEvents() {
//        return registeredEvents;
//    }
//
//    public void setRegisteredEvents(List<Event> registeredEvents) {
//        this.registeredEvents = registeredEvents;
//    }

    @Override
    public String toString() {
        return "Participant{" + super.toString() +
                " name='" + name + '\'' +
                ", age=" + age +
//                ", registeredEvents=" + registeredEvents.size() +
                '}';
    }

    @Override
    public Integer getId() {
        return this.id;
    }

}
