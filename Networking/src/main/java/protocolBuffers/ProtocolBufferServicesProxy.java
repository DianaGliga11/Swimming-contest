package protocolBuffers;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import example.model.User;

import java.io.*;
import java.net.Socket;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.BlockingDeque;
import java.util.concurrent.LinkedBlockingDeque;

public class ProtocolBufferServicesProxy implements IContestServices {
    private final String host;
    private final int port;
    private IMainObserver client;
    private InputStream input;
    private OutputStream output;
    private Socket connection;
    private final static Logger logger = LogManager.getLogger();

    private final BlockingDeque<SwimmingContestProtocol.SwimmingContestResponse> responseQueue;
    private volatile boolean finished;

    public ProtocolBufferServicesProxy(String host, int port) {
        this.host = host;
        this.port = port;
        responseQueue = new LinkedBlockingDeque<>();
    }

    @Override
    public User login(String userName, String password, IMainObserver client) throws Exception {
        initializeConnection();
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createLoginRequest(userName, password);
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Ok) {
            this.client = client;
            return ProtoBuilderUtils.getUser(response);
        } else if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            closeConnection();
            logger.error(response.getErrorMessage());
            throw new Exception(response.getErrorMessage());
        }
        return null;
    }

    @Override
    public void logout(User user, IMainObserver client) throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createLogoutRequest(user);
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        closeConnection();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            logger.error(response.getErrorMessage());
            throw new Exception(response.getErrorMessage());
        }
    }

    @Override
    public Collection<EventDTO> getEventsWithParticipantsCount() throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createGetEventsWithParticipantsCountRequest();
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            logger.error(response.getErrorMessage());
            throw new Exception(response.getErrorMessage());
        }
        return ProtoBuilderUtils.getEventDTOs(response);
    }

    @Override
    public Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createGetParticipantsForEventWithCountRequest(eventId);
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            logger.error(response.getErrorMessage());
            throw new Exception(response.getErrorMessage());
        }
        return ProtoBuilderUtils.getParticipantDTOs(response);
    }

    @Override
    public void saveEventEntry(List<Office> offices) throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createCreateEventEntryRequest(offices);
        sendRequest(request);
    }

    @Override
    public Collection<Participant> findAllParticipants() throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createGetAllParticipantsRequest();
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            logger.error(response.getErrorMessage());
            throw new Exception(response.getErrorMessage());
        }
        return ProtoBuilderUtils.getParticipants(response);
    }

    @Override
    public Collection<Event> findAllEvents() throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createGetAllEventsRequest();
        sendRequest(request);
        SwimmingContestProtocol.SwimmingContestResponse response = readResponse();
        if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
            throw new Exception(response.getErrorMessage());
        }
        return ProtoBuilderUtils.getEvents(response);
    }

    @Override
    public void saveEvent(Event event) throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createCreateEventRequest(event);
        sendRequest(request);
    }

    @Override
    public void saveParticipant(Participant participant) throws Exception {
        SwimmingContestProtocol.SwimmingContestRequest request = ProtoBuilderUtils.createCreateParticipantRequest(participant);
        sendRequest(request);
    }

    private void closeConnection() {
        finished = true;
        try {
            input.close();
            output.close();
            connection.close();
            client = null;
        } catch (IOException e) {
            logger.error("Error while closing connection " + e.getMessage());
            e.printStackTrace();
        }
    }

    private SwimmingContestProtocol.SwimmingContestResponse readResponse() throws Exception {
        try {
            SwimmingContestProtocol.SwimmingContestResponse response = responseQueue.take();
            if (response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.Error) {
                throw new Exception(response.getErrorMessage());
            }
            return response;
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new Exception("Interrupted while waiting for response");
        }
    }

    private void sendRequest(SwimmingContestProtocol.SwimmingContestRequest request) throws Exception {
        try {
            logger.info("Sending request..." + request);
            request.writeDelimitedTo(output);
            output.flush();
        } catch (IOException e) {
            logger.error("Sending request failed", e);
            throw new Exception("Sending request failed", e);
        }
    }

    private void initializeConnection() {
        try {
            connection = new Socket(host, port);
            output = connection.getOutputStream();
            output.flush();
            input = connection.getInputStream();
            finished = false;
            startResponseReader();
        } catch (IOException e) {
            logger.error("Error while initializing connection " + e.getMessage());
            e.printStackTrace();
        }
    }

    private void startResponseReader() {
        Thread responseReader = new Thread(new ReaderThread());
        responseReader.start();
    }

    private class ReaderThread implements Runnable {
        public void run() {
            while (!finished) {
                try {
                    SwimmingContestProtocol.SwimmingContestResponse response = SwimmingContestProtocol.SwimmingContestResponse.parseDelimitedFrom(input);
                    logger.info("response received " + response);
                    if (isUpdateResponse(response)) {
                        handleUpdate(response);
                    }
                    else {
                        try{
                            responseQueue.put(response);
                        }catch(InterruptedException e){
                            e.printStackTrace();
                        }
                    }
                } catch (Exception exception) {
                    logger.error("reading error: {}", exception);
                }
            }
        }
    }

    private boolean isUpdateResponse(SwimmingContestProtocol.SwimmingContestResponse response) {
        return response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.NewParticipant
                || response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.NewEvent
                || response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.NewEventEntry
                || response.getType() == SwimmingContestProtocol.SwimmingContestResponse.Type.UpdatedEvents;
    }

    private void handleUpdate(SwimmingContestProtocol.SwimmingContestResponse response) throws Exception {
        switch (response.getType()) {
            case NewParticipant -> client.participantAdded(ProtoBuilderUtils.getParticipant(response));

            //case NewEvent -> client.eventAdded(ProtoBuilderUtils.getEvent(response));

            case UpdatedEvents -> client.newRegistration(ProtoBuilderUtils.getEventDTOs(response));
        }
    }
}
