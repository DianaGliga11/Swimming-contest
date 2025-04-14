using System;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    [Serializable]
    public class Event : Entity<long>
    {
        public int Distance { get; set; }

        public String Style { get; set; }
        //private List<Participant>  participants;

        public Event( String style, int distance)
        {
            Style = style;
            Distance = distance;
            //this.participants = participants;
        }

        public override String ToString()
        {
            return $"ID={this.Id}, distance={Distance}, style={Style}";
        }
    }
}