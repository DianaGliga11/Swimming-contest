using System;
using System.Text.Json.Serialization;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    [Serializable]
    public class Event : Entity<long>
    {
        [JsonPropertyName("distance")]
        public int Distance { get; set; }
        
        [JsonIgnore] // opțional, ca să nu fie serializată
        public string DisplayText => $"{Style} - {Distance}m";


        [JsonPropertyName("style")]
        public String Style { get; set; }
        //private List<Participant>  participants;

        [JsonConstructor]
        public Event()
        {
        }

        public Event( String style, int distance)
        {
            Style = style;
            Distance = distance;
            //this.participants = participants;
        }

        public override String ToString()
        {
            return $"ID={Id}, Distance={Distance}, style={Style}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Event other)
                return this.Id == other.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}