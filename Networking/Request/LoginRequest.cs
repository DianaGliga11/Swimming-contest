using System.Text.Json.Serialization;
using Service;

namespace Networking.Request
{
    public class LoginRequest : IRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
    
        [JsonPropertyName("password")]
        public string Password { get; set; }
    
        [JsonConstructor]
        public LoginRequest()
        {
            // Constructor necesar pentru deserializare
        }

        public LoginRequest(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}