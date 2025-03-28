using System.Collections.Generic;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public class Office : Entity<long>
    {
        public Participant Participant { get; set; }
        public Event Event { get; set; }

        public Office(Participant participants, Event events)
        {   
            Participant = participants;
            Event = events;
        }

        public override string ToString()
        {
            return $"ID={Id}, Participants={Participant}, Events={Event}";
        }
    }
}


