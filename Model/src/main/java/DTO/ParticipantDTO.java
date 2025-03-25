package DTO;

public class ParticipantDTO {
    private String name;
    private int age;
    private int eventCount;

    public ParticipantDTO(String name, int age, int eventCount) {
        this.name = name;
        this.age = age;
        this.eventCount = eventCount;
    }

    public String getName() { return name; }
    public int getAge() { return age; }
    public int getEventCount() { return eventCount; }
}
