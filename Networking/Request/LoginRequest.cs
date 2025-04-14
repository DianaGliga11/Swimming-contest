using System.Text.Json.Serialization;
using Service;

namespace Networking.Request
{

    public class LoginRequest : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        public LoginRequest()
        {
        }

        public LoginRequest(string username, string password)
        {
           Username = username;
            Password = password;
        }
    }
}