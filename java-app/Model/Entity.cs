using System;
using System.Text.Json.Serialization;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public abstract class Entity<ID>
    {
        [JsonPropertyName("id")]
        public ID Id { get; set; }
        
        public override String ToString()
        {
            return $"ID={Id}";
        }

    }

}