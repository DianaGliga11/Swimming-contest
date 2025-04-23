using System.Text.Json.Serialization;

namespace Networking.Response
{
    
    public class ErrorResponse : IResponse
    {
        [JsonPropertyName("message")]
        public string message { get; set; }

        [JsonConstructor]
        public ErrorResponse() { }

        public ErrorResponse(string message)
        {
            this.message = message;
        }
    }
}