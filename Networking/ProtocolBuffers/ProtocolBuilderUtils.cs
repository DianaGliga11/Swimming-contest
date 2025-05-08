using Org.Example.Protocolbuffers;
using Participant = mpp_proiect_csharp_DianaGliga11.Model.Participant;
using User = mpp_proiect_csharp_DianaGliga11.Model.User;
using Event = mpp_proiect_csharp_DianaGliga11.Model.Event;
using EventDTO = mpp_proiect_csharp_DianaGliga11.Model.DTO.EventDTO;
using ParticipantDTO = mpp_proiect_csharp_DianaGliga11.Model.DTO.ParticipantDTO;
using Office = mpp_proiect_csharp_DianaGliga11.Model.Office;

namespace Networking.ProtocolBuffers;

public class ProtocolBuilderUtils
{
    public static SwimmingContestRequest CreateLoginRequest(string username, string password)
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.Login,
            UserName = username,
            Password = password
        };
    }

    public static SwimmingContestRequest CreateLogoutRequest(User user)
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.Logout,
            User = BuildUser(user)
        };
    }

    public static SwimmingContestRequest CreateCreateParticipantRequest(Participant participant)
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.CreateParticipant,
            Participant = BuildParticipant(participant)
        };
    }

    public static SwimmingContestRequest CreateCreateEventEntryRequest(List<Office> offices)
    {
        var request = new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.CreateEventEntry
        };
        
        foreach (var office in offices)
        {
            request.Office.Add(BuildEventEntry(office));
        }
        
        return request;
    }

    public static SwimmingContestRequest CreateCreateEventRequest(Event @event)
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.CreateEvent,
            Event = BuildEvent(@event)
        };
    }

    public static SwimmingContestRequest CreateGetAllEventsRequest()
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.GetAllEvents
        };
    }

    public static SwimmingContestRequest CreateGetAllParticipantsRequest()
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.GetAllParticipants
        };
    }

    public static SwimmingContestRequest CreateGetEventsWithParticipantsCountRequest()
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.GetEventsWithParticipantsCount
        };
    }

    public static SwimmingContestRequest CreateGetParticipantsForEventWithCountRequest(long eventId)
    {
        return new SwimmingContestRequest
        {
            Type = SwimmingContestRequest.Types.Type.GetParticipantsForEventWithCount,
            EventId = eventId
        };
    }

    public static SwimmingContestResponse CreateOkResponse()
    {
        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.Ok
        };
    }

    public static SwimmingContestResponse CreateOkResponse(User user)
    {
        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.Ok,
            User = BuildUser(user)
        };
    }

    public static SwimmingContestResponse CreateErrorResponse(string errorMessage)
    {
        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.Error,
            ErrorMessage = errorMessage
        };
    }

    public static SwimmingContestResponse CreateNewParticipantResponse(Participant participant)
    {
        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.NewParticipant,
            Participant = BuildParticipant(participant)
        };
    }

    public static SwimmingContestResponse CreateNewEventEntryResponse(EventDTO eventDTO)
    {
        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.NewEventEntry,
            EventDTO = BuildEventDTO(eventDTO)
        };
    }

    public static SwimmingContestResponse CreateAllEventsResponse(List<Event> events)
    {
        List<Org.Example.Protocolbuffers.Event> buildEvents = new();
        events.ForEach(@event => buildEvents.Add(BuildEvent(@event)));

        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.AllEvents,
            Events = {buildEvents}
        };
    }

    public static SwimmingContestResponse CreateAllParticipantsResponse(List<Participant> participants)
    {
        List<Org.Example.Protocolbuffers.Participant> buildParticipants = new();
        participants.ForEach(participant => buildParticipants.Add(BuildParticipant(participant)));

        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.AllParticipants,
            Participants = { buildParticipants }
        };
    }

    public static SwimmingContestResponse CreateEventsWithParticipantsCountResponse(List<EventDTO> eventDTOs)
    {
        List<Org.Example.Protocolbuffers.EventDTO> buildEvents = new();
        eventDTOs.ForEach(@event => buildEvents.Add(BuildEventDTO(@event)));

        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.GetEventWithParticipantsCount,
            EventsDTO = { buildEvents }
        };
    }

    public static SwimmingContestResponse CreateParticipantsForEventWithCountResponse(List<ParticipantDTO> participants)
    {
        List<Org.Example.Protocolbuffers.ParticipantDTO> buildParticipants = new();
        participants.ForEach(participant => buildParticipants.Add(BuildParticipantDTO(participant)));

        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.GetParticipantsForEventWithCount,
            ParticipantsDTO = { buildParticipants }
        };
    }

    public static SwimmingContestResponse CreateUpdatedEventsResponse(List<EventDTO> updatedEvents)
    {
        List<Org.Example.Protocolbuffers.EventDTO> buildEvents = new();
        updatedEvents.ForEach(@event => buildEvents.Add(BuildEventDTO(@event)));

        return new SwimmingContestResponse
        {
            Type = SwimmingContestResponse.Types.Type.UpdatedEvents,
            EventsDTO = { buildEvents }
        };
    }

    // Builder methods
    private static Org.Example.Protocolbuffers.User BuildUser(User user)
    {
        return new Org.Example.Protocolbuffers.User
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password
        };
    }

    private static Org.Example.Protocolbuffers.Office BuildEventEntry(Office office)
    {
        return new Org.Example.Protocolbuffers.Office
        {
            Id = office.Id,
            Participant = BuildParticipant(office.Participant),
            Event = BuildEvent(office.Event)
        };
    }

    private static Org.Example.Protocolbuffers.Event BuildEvent(Event @event)
    {
        return new Org.Example.Protocolbuffers.Event
        {
            Id = @event.Id,
            Style = @event.Style,
            Distance = @event.Distance
        };
    }

    private static Org.Example.Protocolbuffers.Participant BuildParticipant(Participant participant)
    {
        return new Org.Example.Protocolbuffers.Participant
        {
            Id = participant.Id,
            Name = participant.Name,
            Age = participant.Age
        };
    }

    private static Org.Example.Protocolbuffers.EventDTO BuildEventDTO(EventDTO eventDTO)
    {
        return new Org.Example.Protocolbuffers.EventDTO
        {
            Name = eventDTO.style,
            Distance = eventDTO.distance,
            ParticipantsCount = eventDTO.participantsCount
        };
    }

    private static Org.Example.Protocolbuffers.ParticipantDTO BuildParticipantDTO(ParticipantDTO participantDTO)
    {
        return new Org.Example.Protocolbuffers.ParticipantDTO
        {
            Name = participantDTO.name,
            Age = participantDTO.age,
            EventCount = participantDTO.eventCount
        };
    }

    public static User GetUser(SwimmingContestResponse response)
    {
        var user = response.User;
        return new User
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password
        };
    }

    public static User GetUser(SwimmingContestRequest request)
    {
        var user = request.User;
        return new User
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password
        };
    }


    public static List<EventDTO> GetEventDTOs(SwimmingContestResponse response)
    {
        List<EventDTO> result = new();
        foreach (Org.Example.Protocolbuffers.EventDTO dto in response.EventsDTO)
        {
            result.Add(UnbuildEventDTO(dto));
        }
        return result;
    }

    private static Org.Example.Protocolbuffers.EventDTO UnbuildEventDTO(EventDTO eventDTO)
    {
        return new Org.Example.Protocolbuffers.EventDTO
        {
            Name = eventDTO.style,
            Distance = eventDTO.distance,
            ParticipantsCount = eventDTO.participantsCount
        };
    }

    private static EventDTO UnbuildEventDTO(Org.Example.Protocolbuffers.EventDTO eventDTO)
    {
        EventDTO ev = new EventDTO(eventDTO.Name, eventDTO.Distance, eventDTO.ParticipantsCount);
        ev.Id = eventDTO.Id;
        return ev;
    }
    
    public static List<ParticipantDTO> GetParticipantDTOs(SwimmingContestResponse response)
    {
        var result = new List<ParticipantDTO>();
        foreach (var dto in response.ParticipantsDTO)
        {
            result.Add(UnbuildParticipantDTO(dto));
        }
        return result;
    }

    private static Org.Example.Protocolbuffers.ParticipantDTO UnbuildParticipantDTO(ParticipantDTO participantDTO)
    {
        return new Org.Example.Protocolbuffers.ParticipantDTO
        {
            Name = participantDTO.name,
            Age = participantDTO.age,
            EventCount = participantDTO.eventCount
        };
    }
    
    private static ParticipantDTO UnbuildParticipantDTO(Org.Example.Protocolbuffers.ParticipantDTO participantDTO)
    {
       ParticipantDTO participant = new ParticipantDTO(participantDTO.Name, participantDTO.Age, participantDTO.EventCount);
       participant.Id = participantDTO.Id;
       return participant;
    }
    
    public static List<Participant> GetParticipants(SwimmingContestResponse response)
    {
        var result = new List<Participant>();
        foreach (var p in response.Participants)
        {
            result.Add(UnbuildParticipant(p));
        }
        return result;
    }

    private static Participant UnbuildParticipant(Org.Example.Protocolbuffers.Participant participant)
    {
        Participant p = new Participant(participant.Name, participant.Age);
        p.Id = participant.Id;
        return p;
    }


    public static List<Event> GetEvents(SwimmingContestResponse response)
    {
        var result = new List<Event>();
        foreach (var e in response.Events)
        {
            result.Add(UnbuildEvent(e));
        }
        return result;
    }

    private static Event UnbuildEvent(Org.Example.Protocolbuffers.Event @event)
    {
        Event ev = new Event(@event.Style, @event.Distance);
        ev.Id = @event.Id;
        return ev;
    }

    public static Participant GetParticipant(SwimmingContestResponse response)
    {
        return UnbuildParticipant(response.Participant);
    }

    public static Participant GetParticipant(SwimmingContestRequest request)
    {
        return UnbuildParticipant(request.Participant);
    }

    public static string GetUsername(SwimmingContestRequest request)
    {
        return request.UserName;
    }

    public static string GetPassword(SwimmingContestRequest request)
    {
        return request.Password;
    }

    public static List<Office> GetEventEntries(SwimmingContestRequest request)
    {
        var result = new List<Office>();
        foreach (var entry in request.Office)
        {
            result.Add(UnbuildEventEntry(entry));
        }
        return result;
    }

    private static Office UnbuildEventEntry(Org.Example.Protocolbuffers.Office office)
    {
        Office of = new Office(UnbuildParticipant(office.Participant), UnbuildEvent(office.Event));
        of.Id = office.Id;
        return of;
    }
}