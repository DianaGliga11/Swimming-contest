using System.Text.Json.Serialization;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class OkResponse : IResponse
    {
        [JsonPropertyName("user")]
        public User user { get; set; }

        [JsonConstructor]
        public OkResponse() { }

        public OkResponse(User user)
        {
            this.user = user;
        }
    }
}