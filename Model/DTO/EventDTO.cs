using System;

namespace mpp_proiect_csharp_DianaGliga11.Model.DTO;

[Serializable]
public class EventDTO : Entity<long>
{
    public String style { get; set; }
    public int distance { get; set; }
    public int participantsCount { get; set; }

    public EventDTO(string style, int distance, int participantsCount)
    {
        this.style = style;
        this.distance = distance;
        this.participantsCount = participantsCount;
    }
    
}