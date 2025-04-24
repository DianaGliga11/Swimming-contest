using System;
using System.Text.Json.Serialization;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    [Serializable]
    public class Participant : Entity<long>
    {
        [JsonPropertyName("name")]
        public String Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
//    private List<Event> registeredEvents;
        
        [JsonConstructor]
        public Participant()
        {
        }

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