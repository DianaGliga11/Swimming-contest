using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;

namespace Networking
{
    public static class JsonProtocolUtils
    { 
        public static RequestJson CreateLoginRequest(string username, string password)
        {
            return new RequestJson
            {
                Type = RequestType.LOGIN,
                User = new User { UserName = username, Password = password }
            };
        }

        public static RequestJson CreateLogoutRequest(User user)
        {
            return new RequestJson
            {
                Type = RequestType.LOGOUT,
                User = user
            };
        }

        public static RequestJson CreateCreateEventEntriesRequest(List<Office> offices)
        {
            return new RequestJson
            {
                Type = RequestType.CREATE_EVENT_ENTRIES,
                EventEntries = offices
            };
        }

        public static RequestJson CreateCreateParticipantRequest(Participant participant)
        {
            return new RequestJson
            {
                Type = RequestType.CREATE_PARTICIPANT,
                Participant = participant
            };
        }

        public static RequestJson CreateGetAllEventsRequest()
        {
            return new RequestJson
            {
                Type = RequestType.GET_ALL_EVENTS
            };
        }

        public static RequestJson CreateGetAllParticipantsRequest()
        {
            return new RequestJson
            {
                Type = RequestType.GET_ALL_PARTICIPANTS
            };
        }

        public static RequestJson CreateGetEventsWithParticipantsCountRequest()
        {
            return new RequestJson
            {
                Type = RequestType.GET_EVENTS_WITH_PARTICIPANTS_COUNT
            };
        }

        public static RequestJson CreateGetParticipantsForEventWithCountRequest(long eventId)
        {
            return new RequestJson
            {
                Type = RequestType.GET_PARTICIPANTS_FOR_EVENT_WITH_COUNT,
                EventId = eventId
            };
        }

        public static ResponseJson CreateOkResponse(User? user = null)
        {
            return new ResponseJson
            {
                Type = ResponseType.OK,
                User = user
            };
        }

        public static ResponseJson CreateErrorResponse(string message)
        {
            return new ResponseJson
            {
                Type = ResponseType.ERROR,
                Error = message
            };
        }

        public static ResponseJson CreateAllEventsResponse(List<Event> events)
        {
            return new ResponseJson
            {
                Type = ResponseType.ALL_EVENTS,
                EventsRaw = events
            };
        }

        public static ResponseJson CreateAllParticipantsResponse(List<Participant> participants)
        {
            return new ResponseJson
            {
                Type = ResponseType.ALL_PARTICIPANTS,
                ParticipantsRaw = participants
            };
        }

        public static ResponseJson CreateEventsWithParticipantsCountResponse(List<EventDTO> eventDTOs)
        {
            return new ResponseJson
            {
                Type = ResponseType.EVENTS_WITH_PARTICIPANTS_COUNT,
                Events = eventDTOs
            };
        }

        public static ResponseJson CreateGetParticipantsForEventWithCountResponse(List<ParticipantDTO> participantDTOs)
        {
            return new ResponseJson
            {
                Type = ResponseType.GET_PARTICIPANTS_FOR_EVENT_WITH_COUNT,
                Participants = participantDTOs
            };
        }

        public static ResponseJson CreateNewParticipantResponse(Participant participant)
        {
            return new ResponseJson
            {
                Type = ResponseType.NEW_PARTICIPANT,
                Participant = participant
            };
        }

        public static ResponseJson CreateUpdatedEventsResponse(List<EventDTO> updatedEvents)
        {
            return new ResponseJson
            {
                Type = ResponseType.UPDATED_EVENTS,
                Events = updatedEvents
            };
        }
    }
}
