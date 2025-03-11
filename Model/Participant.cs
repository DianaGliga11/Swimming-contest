namespace mpp_proiect_csharp_DianaGliga11.Model
{

    public class Participant : Entity<int>
    {
        private String Name { get; set; }
        private int Age { get; set; }
//    private List<Event> registeredEvents;

        public Participant(int id, String name, int age)
        {
            Id = id;
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