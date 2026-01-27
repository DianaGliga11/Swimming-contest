package protocolBuffers;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import example.model.User;

import java.util.ArrayList;
import java.util.List;

public class ProtoBuilderUtils {

    public static SwimmingContestProtocol.SwimmingContestRequest createLoginRequest(final String username, final  String password) {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.Login)
                .setUserName(username)
                .setPassword(password)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createLogoutRequest(final User user) {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.Logout)
                .setUser(buildUser(user))
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createCreateParticipantRequest(final Participant participant) {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.CreateParticipant)
                .setParticipant(buildParticipant(participant))
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createCreateEventEntryRequest(final List<Office> offices) {
        final List<SwimmingContestProtocol.Office> buildEndtries = new ArrayList<>();
        offices.forEach((office) -> buildEndtries.add(buildEventEntry(office)));
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.CreateEventEntry)
                .addAllOffice(buildEndtries).build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createCreateEventRequest(final Event event) {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.CreateEvent)
                .setEvent(buildEvent(event))
                .build();
    }
    public static SwimmingContestProtocol.SwimmingContestRequest createGetAllEventsRequest() {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.GetAllEvents)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createGetAllParticipantsRequest() {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.GetAllParticipants)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createGetEventsWithParticipantsCountRequest() {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.GetEventsWithParticipantsCount)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestRequest createGetParticipantsForEventWithCountRequest(long eventId) {
        return SwimmingContestProtocol.SwimmingContestRequest.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestRequest.Type.GetParticipantsForEventWithCount)
                .setEventId(eventId)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createOkResponse() {
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.Ok)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createOkResponse(final User user) {
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.Ok)
                .setUser(buildUser(user))
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createErrorResponse(final String errorMessage) {
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.Error)
                .setErrorMessage(errorMessage)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createNewParticipantResponse(final Participant participant) {
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.NewParticipant)
                .setParticipant(buildParticipant(participant))
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createNewEventEntryResponse(final EventDTO eventDTO) {
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.NewEventEntry)
                .setEventDTO(buildEventDTO(eventDTO))
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createAllEventsResponse(final List<Event> events) {
        final List<SwimmingContestProtocol.Event> buildEvents = new ArrayList<>();
        events.forEach((event) -> buildEvents.add(buildEvent(event)));
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.AllEvents)
                .addAllEvents(buildEvents)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createAllParticipantsResponse(final List<Participant> participants) {
        final List<SwimmingContestProtocol.Participant> buildParticipants = new ArrayList<>();
        participants.forEach((participant) -> buildParticipants.add(buildParticipant(participant)));
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.AllParticipants)
                .addAllParticipants(buildParticipants)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createEventsWithParticipantsCountResponse(final List<EventDTO> eventDTOs) {
        final List<SwimmingContestProtocol.EventDTO> buildEvents = new ArrayList<>();
        eventDTOs.forEach((eventDTO) -> buildEvents.add(buildEventDTO(eventDTO)));
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.GetEventWithParticipantsCount)
                .addAllEventsDTO(buildEvents)
                .build();
    }

    public static SwimmingContestProtocol.SwimmingContestResponse createParticipantsForEventWithCountResponse(final List<ParticipantDTO> participants) {
        final List<SwimmingContestProtocol.ParticipantDTO> buildParticipants = new ArrayList<>();
        participants.forEach((participantDTO) -> buildParticipants.add(buildParticipantDTO(participantDTO)));
        return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
                .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.GetParticipantsForEventWithCount)
                .addAllParticipantsDTO(buildParticipants)
                .build();
    }
    
    public static SwimmingContestProtocol.SwimmingContestResponse createUpdatedEventsResponse(final List<EventDTO> updatedEvents) {
       final List<SwimmingContestProtocol.EventDTO> buildEvents = new ArrayList<>();
       updatedEvents.forEach((eventDTO) -> buildEvents.add(buildEventDTO(eventDTO)));
       return SwimmingContestProtocol.SwimmingContestResponse.newBuilder()
               .setType(SwimmingContestProtocol.SwimmingContestResponse.Type.UpdatedEvents)
               .addAllEventsDTO(buildEvents)
               .build();
    }

    private static SwimmingContestProtocol.User buildUser(User user) {
        return SwimmingContestProtocol.User.newBuilder()
                .setId(user.getId())
                .setUserName(user.getUserName())
                .setPassword(user.getPassword())
                .build();
    }

    private static SwimmingContestProtocol.Office buildEventEntry(final Office office) {
        SwimmingContestProtocol.Office.Builder builder = SwimmingContestProtocol.Office.newBuilder()
                .setParticipant(buildParticipant(office.getParticipant()))
                .setEvent(buildEvent(office.getEvent()));

        if (office.getId() != null) {
            builder.setId(office.getId());
        }
        return builder.build();
    }


    private static SwimmingContestProtocol.Event buildEvent(final Event event) {
        return SwimmingContestProtocol.Event.newBuilder()
                .setId(event.getId())
                .setStyle(event.getStyle())
                .setDistance(event.getDistance())
                .build();
    }

    private static SwimmingContestProtocol.Participant buildParticipant(final Participant participant) {
        SwimmingContestProtocol.Participant.Builder builder =  SwimmingContestProtocol.Participant.newBuilder()
                .setName(participant.getName())
                .setAge(participant.getAge());

        if(participant.getId() != null) {
            builder.setId(participant.getId());
        }
        return builder.build();
    }

    private static SwimmingContestProtocol.EventDTO buildEventDTO(final EventDTO eventDTO) {
        return SwimmingContestProtocol.EventDTO.newBuilder()
                .setName(eventDTO.getStyle())
                .setDistance(eventDTO.getDistance())
                .setParticipantsCount(eventDTO.getParticipantsCount())
                .build();
    }

    private static SwimmingContestProtocol.ParticipantDTO buildParticipantDTO(final ParticipantDTO participantDTO) {
        return SwimmingContestProtocol.ParticipantDTO.newBuilder()
                .setName(participantDTO.getName())
                .setAge(participantDTO.getAge())
                .setEventCount(participantDTO.getEventCount())
                .build();
    }


    public static User getUser(final SwimmingContestProtocol.SwimmingContestResponse response) {
        final SwimmingContestProtocol.User user = response.getUser();
        final User result = new User(user.getUserName(), user.getPassword());
        result.setId(user.getId());
        return result;
    }

    public static User getUser(final SwimmingContestProtocol.SwimmingContestRequest request) {
        final SwimmingContestProtocol.User user = request.getUser();

        final User result = new User(user.getUserName(), user.getPassword());
        result.setId(user.getId());
        return result;
    }

    public static List<EventDTO> getEventDTOs(final SwimmingContestProtocol.SwimmingContestResponse response) {
        final List<EventDTO> result = new ArrayList<>();
        response.getEventsDTOList().forEach((eventDTO) -> result.add(unbuildEventDTO(eventDTO)));
        return result;
    }

    private static EventDTO unbuildEventDTO(final SwimmingContestProtocol.EventDTO eventDTO) {
        return new EventDTO(eventDTO.getName(), eventDTO.getDistance(), eventDTO.getParticipantsCount());
    }

    public static List<ParticipantDTO> getParticipantDTOs(final SwimmingContestProtocol.SwimmingContestResponse response) {
        final List<ParticipantDTO> result = new ArrayList<>();
        response.getParticipantsDTOList().forEach((participantDTO) -> result.add(unbuildParticipantDTO(participantDTO)));
        return result;
    }

    private static ParticipantDTO unbuildParticipantDTO(final SwimmingContestProtocol.ParticipantDTO participantDTO) {
        return new ParticipantDTO(participantDTO.getName(), participantDTO.getAge(), participantDTO.getEventCount());
    }

    public static List<Participant> getParticipants(final SwimmingContestProtocol.SwimmingContestResponse response) {
        final List<Participant> result = new ArrayList<>();
        response.getParticipantsList().forEach((paticipant) -> result.add(unbuildParticipant(paticipant)));
        return result;
    }

    private static Participant unbuildParticipant(final SwimmingContestProtocol.Participant paticipant) {
        final Participant result = new Participant(paticipant.getName(), paticipant.getAge());
        result.setId(paticipant.getId());
        return result;
    }

    public static List<Event> getEvents(final SwimmingContestProtocol.SwimmingContestResponse response) {
        final List<Event> result = new ArrayList<>();
        response.getEventsList().forEach((event) -> result.add(unbuildEvent(event)));
        return result;
    }

    private static Event unbuildEvent(final SwimmingContestProtocol.Event event) {
        final Event result = new Event(event.getStyle(), event.getDistance());
        result.setId(event.getId());
        return result;
    }

    public static Participant getParticipant(SwimmingContestProtocol.SwimmingContestResponse response) {
        return unbuildParticipant(response.getParticipant());
    }

    public static Participant getParticipant(SwimmingContestProtocol.SwimmingContestRequest request) {
        return unbuildParticipant(request.getParticipant());
    }

    public static String getUsername(final SwimmingContestProtocol.SwimmingContestRequest request) {
        return request.getUserName();
    }

    public static String getPassword(final SwimmingContestProtocol.SwimmingContestRequest request) {
        return request.getPassword();
    }

    public static List<Office> getEventEntries(final SwimmingContestProtocol.SwimmingContestRequest request) {
        final List<Office> result = new ArrayList<>();
        request.getOfficeList().forEach((buildEntry) -> result.add(unbuildEventEntry(buildEntry)));
        return result;
    }

    private static Office unbuildEventEntry(final SwimmingContestProtocol.Office buildEntry) {
        final Office entry = new Office(unbuildParticipant(buildEntry.getParticipant()), unbuildEvent(buildEntry.getEvent()));
        entry.setId(buildEntry.getId());
        return entry;
    }

//    public static Event getEvent(SwimmingContestProtocol.SwimmingContestResponse response) {
//        return unbuildEvent(response.getEvent());
//    }
}