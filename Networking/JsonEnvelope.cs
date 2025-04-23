using System.Text.Json;
using System.Text.Json.Serialization;

namespace Networking
{

    public class JsonEnvelope
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("payload")]
        public JsonElement Payload { get; set; }
    }
}
