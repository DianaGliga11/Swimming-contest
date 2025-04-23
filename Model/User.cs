using System;
using System.Text.Json.Serialization;

namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public class User : Entity<long>
    {
        [JsonPropertyName("userName")]
        public String UserName { get; set; }
        
        [JsonPropertyName("password")]
        public String Password { get; set; }

        [JsonConstructor]
        public User()
        {
            
        }
        public User(String userName, String password)
        {
            UserName = userName;
            Password = password;
        }

        public override String ToString()
        {
            return $"ID={Id}, userName={UserName}, password={Password}";
        }

    }
}