using System.Text.Json.Serialization;

namespace Networking
{
    using System.Text.Json.Serialization;

    namespace Networking
    {
        public class TypedMessage
        {
            [JsonPropertyName("$type")]
            public string Type { get; set; }

            [JsonPropertyName("payload")]
            public object Payload { get; set; }

            [JsonConstructor]
            public TypedMessage() { }

            public TypedMessage(object payload)
            {
                Payload = payload;
                Type = payload.GetType().Name;
            }
        }
    }
}