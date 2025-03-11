namespace mpp_proiect_csharp_DianaGliga11.Model
{

    public class Event : Entity<int>
    {
        private int Distance { get; set; }
        private String Style { get; set; }
        //private List<Participant>  participants;

        public Event(int id, String style, int distance)
        {
            Id = id;
            Style = style;
            Distance = distance;
            //this.participants = participants;
        }

        public override String ToString()
        {
            return $"ID={Id}, distance={Distance}, style={Style}";
        }
    }
}