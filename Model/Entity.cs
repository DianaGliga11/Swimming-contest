using System;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    [Serializable]
    public abstract class Entity<ID>
    {
        public ID Id { get; set; }

        public override String ToString()
        {
            return $"ID={Id}";
        }

    }

}