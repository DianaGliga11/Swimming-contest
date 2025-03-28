using System;

namespace mpp_proiect_csharp_DianaGliga11.Model.DTO;

public class ParticipantDTO : Entity<long>
{
    public String name { get; set; }
    public int age {get; set;}
    public int eventCount {get; set;}

    public ParticipantDTO(string name, int age, int eventCount)
    {
        this.name = name;
        this.age = age;
        this.eventCount = eventCount;
    }
}