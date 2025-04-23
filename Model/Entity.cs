using System;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public abstract class Entity<ID>
    {
        public ID Id { get; set; }
        
        public override String ToString()
        {
            return $"ID={Id}";
        }

    }

}