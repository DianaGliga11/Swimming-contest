using System.Text.Json.Serialization;

namespace Networking{

    public enum ResponseType
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        OK,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        ERROR,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        ALL_EVENTS,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        ALL_PARTICIPANTS,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        EVENTS_WITH_PARTICIPANTS_COUNT,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        GET_PARTICIPANTS_FOR_EVENT_WITH_COUNT,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        UPDATED_EVENTS,

        [JsonConverter(typeof(JsonStringEnumConverter))]
        NEW_PARTICIPANT
    }

}