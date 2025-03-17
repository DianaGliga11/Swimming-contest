using System;

namespace mpp_proiect_csharp_DianaGliga11.Model
{

    public class Participant : Entity<int>
    {
        public String Name { get; set; }

        public int Age { get; set; }
//    private List<Event> registeredEvents;

        public Participant(String name, int age)
        {
            Name = name;
            Age = age;
            //       this.registeredEvents = registeredEvents;
        }

        public override String ToString()
        {
            return $"ID={Id}, name={Name}, age={Age}";
        }
    }
}