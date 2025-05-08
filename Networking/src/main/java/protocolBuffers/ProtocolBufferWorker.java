package protocolBuffers;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import example.example.Event;
import example.example.Office;
import example.example.Participant;
import example.example.User;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.List;

public class ProtocolBufferWorker implements Runnable, IMainObserver {
    private final IContestServices contestServices;
    private final Socket socket;
    private InputStream input;
    private OutputStream output;
    private volatile boolean connected;

    private final static Logger logger = LogManager.getLogger();

    public ProtocolBufferWorker(IContestServices contestServices, Socket socket) {
        this.contestServices = contestServices;
        this.socket = socket;
        try {
            input = socket.getInputStream();
            output = socket.getOutputStream();
            connected = true;
        } catch (Exception e) {
            logger.error("Error initializing worker: " + e.getMessage());
        }
    }

    @Override
    public void run() {
        while (connected) {
            try {
                SwimmingContestProtocol.SwimmingContestRequest request =
                        SwimmingContestProtocol.SwimmingContestRequest.parseDelimitedFrom(input);

                if (request != null) {
                    SwimmingContestProtocol.SwimmingContestResponse response = handleRequest(request);
                    if (response != null) {
                        sendResponse(response);
                    }
                }

                Thread.sleep(100);
            } catch (Exception e) {
                logger.error("Error in worker loop: " + e.getMessage(), e);
                connected = false;
            }
        }

        try {
            input.close();
            output.close();
            socket.close();
        } catch (Exception e) {
            logger.error("Error closing connection: " + e.getMessage(), e);
        }
    }

    private void sendResponse(SwimmingContestProtocol.SwimmingContestResponse response) throws Exception {
        response.writeDelimitedTo(output);
        output.flush();
    }

    private SwimmingContestProtocol.SwimmingContestResponse handleRequest(SwimmingContestProtocol.SwimmingContestRequest request) {
        switch (request.getType()) {
            case Login -> {
                try {
                    logger.info("Login attempt for user: " + request.getUserName());
                    User user = contestServices.login(request.getUserName(), request.getPassword(), this);
                    logger.info("Login successful for user: " + user.getUserName());
                    return ProtoBuilderUtils.createOkResponse(user);
                } catch (Exception e) {
                    logger.error("Login failed: " + e.getMessage());
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case Logout -> {
                System.out.println("> handling logout request");
                final User user = ProtoBuilderUtils.getUser(request);

                try {
                    contestServices.logout(user, this);
                    connected = false;
                    return ProtoBuilderUtils.createOkResponse();
                } catch (Exception contestDataException) {
                    return ProtoBuilderUtils.createErrorResponse(contestDataException.getMessage());
                }
            }

            case CreateParticipant -> {
                try {
                    Participant participant = ProtoBuilderUtils.getParticipant(request);
                    contestServices.saveParticipant(participant);
                    return ProtoBuilderUtils.createNewParticipantResponse(participant);
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case CreateEventEntry -> {
                try {
                    List<Office> offices = ProtoBuilderUtils.getEventEntries(request);
                    contestServices.saveEventEntry(offices);
                    return ProtoBuilderUtils.createUpdatedEventsResponse(contestServices.getEventsWithParticipantsCount().stream().toList());
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case GetEventsWithParticipantsCount -> {
                try {
                    List<EventDTO> events = contestServices.getEventsWithParticipantsCount().stream().toList();
                    return ProtoBuilderUtils.createEventsWithParticipantsCountResponse(events);
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case GetAllParticipants -> {
                try {
                    List<Participant> participants = contestServices.findAllParticipants().stream().toList();
                    return ProtoBuilderUtils.createAllParticipantsResponse(participants);
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case GetAllEvents -> {
                try {
                    List<Event> events = contestServices.findAllEvents().stream().toList();
                    return ProtoBuilderUtils.createAllEventsResponse(events);
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            case GetParticipantsForEventWithCount -> {
                try {
                    long eventId = request.getEventId();
                    List<ParticipantDTO> participants = contestServices.getParticipantsForEventWithCount(eventId).stream().toList();
                    return ProtoBuilderUtils.createParticipantsForEventWithCountResponse(participants);
                } catch (Exception e) {
                    return ProtoBuilderUtils.createErrorResponse(e.getMessage());
                }
            }

            default -> {
                return ProtoBuilderUtils.createErrorResponse("Unknown request type: " + request.getType());
            }
        }
    }

    @Override
    public void participantAdded(Participant participant) {
        try {
            sendResponse(ProtoBuilderUtils.createNewParticipantResponse(participant));
        } catch (Exception e) {
            logger.error("Failed to notify about participant: " + e.getMessage(), e);
        }
    }

    @Override
    public void newRegistration(List<EventDTO> events) {
        try {
            sendResponse(ProtoBuilderUtils.createUpdatedEventsResponse(events));
        } catch (Exception e) {
            logger.error("Failed to notify about new registration: " + e.getMessage(), e);
        }
    }

    @Override
    public void eventAdded(EventDTO event) {
        try {
            sendResponse(ProtoBuilderUtils.createNewEventEntryResponse(event));
        } catch (Exception e) {
            logger.error("Failed to notify about new event: " + e.getMessage(), e);
        }
    }
}
