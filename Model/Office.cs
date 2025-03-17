using System.Collections.Generic;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public class Office : Entity<int>
    {
        public List<Participant> Participants { get; set; }
        public List<Event> Events { get; set; }

        public Office(List<Participant> participants, List<Event> events)
        {   
            Participants = participants;
            Events = events;
        }

        public override string ToString()
        {
            return $"ID={Id}, Participants={Participants}, Events={Events}";
        }
    }
}


