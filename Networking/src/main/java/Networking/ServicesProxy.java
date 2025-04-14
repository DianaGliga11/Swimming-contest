package Networking;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import example.example.Event;
import example.example.Office;
import example.example.Participant;
import example.example.User;
import request.*;
import response.*;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.BlockingDeque;
import java.util.concurrent.LinkedBlockingDeque;

public class ServicesProxy implements IContestServices {
    private final String host;
    private final int port;
    private IMainObserver client;
    private ObjectInputStream input;
    private ObjectOutputStream output;
    private Socket connection;
    private final static Logger logger = LogManager.getLogger();

    private final BlockingDeque<Response> responseQueue;
    private volatile boolean finished;

    public ServicesProxy(String host, int port) {
        this.host = host;
        this.port = port;
        responseQueue = new LinkedBlockingDeque<>();
    }

    @Override
    public User login(String userName, String password, IMainObserver client) throws Exception {
        initializeConnection();
        sendRequest(new LoginRequest(userName, password));
        Response response = readResponse();
        if (response instanceof OKResponse) {
            this.client = client;
            return ((OKResponse) response).getUser();
        }
        if (response instanceof ErrorResponse) {
            closeConnection();
            logger.error(((ErrorResponse) response).getError());
            throw new Exception(((ErrorResponse) response).getError());
        }
        return null;
    }


    @Override
    public void logout(User user, IMainObserver client) throws Exception {
        sendRequest(new LogoutRequest(user));
        Response response = readResponse();
        closeConnection();
        if(response instanceof ErrorResponse) {
            logger.error(((ErrorResponse) response).getError());
            throw new Exception(((ErrorResponse) response).getError());
        }
    }

    @Override
    public Collection<EventDTO> getEventsWithParticipantsCount() throws Exception {
        sendRequest(new GetEventsWithParticipantsCounRequest());
        Response response = readResponse();
        if(response instanceof ErrorResponse){
            logger.error(((ErrorResponse) response).getError());
            throw new Exception(((ErrorResponse) response).getError());
        }
        GetEventWithParticipantsCountResponse result = (GetEventWithParticipantsCountResponse) response;
        return result.getEvents();
    }

    @Override
    public Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) throws Exception {
        sendRequest( new GetParticipantsForEventWithCountRequest(eventId));
        Response response = readResponse();
        if(response instanceof ErrorResponse) {
            logger.error(((ErrorResponse) response).getError());
            throw new Exception(((ErrorResponse) response).getError());
        }
        GetParticipantsForEventWithCountResponse result = (GetParticipantsForEventWithCountResponse) response;
        return result.getParticipants();
    }

    @Override
    public void saveEventEntry(List<Office> offices) throws Exception {
        sendRequest(new CreateEventEntriesRequest(offices));
    }

    @Override
    public Collection<Participant> findAllParticipants() throws Exception {
        sendRequest(new GetAllParticipantsRequest());
        Response response = readResponse();
        if(response instanceof ErrorResponse){
            logger.error(((ErrorResponse) response).getError());
            throw new Exception(((ErrorResponse) response).getError());
        }
        AllParticipantsResponse result = (AllParticipantsResponse) response;
        return result.getParticipants();
    }

    @Override
    public Collection<Event> findAllEvents() throws Exception {
        sendRequest(new GetAllEventsRequest());
        Response response = readResponse();
        if (response instanceof ErrorResponse) {
            throw new Exception(((ErrorResponse) response).getError());
        }
        return ((AllEventsResponse) response).getEvents();
    }

    @Override
    public void saveEvent(Event event) throws Exception {
        sendRequest(new CreateEventRequest(event));
    }

    @Override
    public void saveParticipant(Participant participant) throws Exception {
        sendRequest(new CreateParticipantRequest(participant));
    }

    private void closeConnection() {
        finished=true;
        try{
            input.close();
            output.close();
            connection.close();
            client = null;
        }catch (IOException e){
            logger.error("Error while closing connection " + e.getMessage());
            e.printStackTrace();
        }
    }

    private Response readResponse() throws Exception {
        try {
            Response response = responseQueue.take();
            if (response instanceof ErrorResponse) {
                throw new Exception(((ErrorResponse) response).getError());
            }
            return response;
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new Exception("Interrupted while waiting for response");
        }
    }

    private void sendRequest(Request request) throws Exception {
        try {
            output.writeObject(request);
            output.flush();
        } catch (IOException e) {
            logger.error("Sending request failed", e);
            throw new Exception("Sending request failed", e);
        }
    }

    private void initializeConnection() {
        try{
            connection = new Socket(host,port);
            output = new ObjectOutputStream(connection.getOutputStream());
            output.flush();
            input = new ObjectInputStream(connection.getInputStream());
            finished=false;
            startResponseReader();
        }catch (IOException e){
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
                    Object response = input.readObject();
                    logger.info("response received " + response);
                    if (response instanceof UpdateResponse) {
                        handleUpdate((UpdateResponse) response);
                    } else {
                        try {
                            responseQueue.put((Response) response);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                } catch (IOException | ClassNotFoundException exception) {
                    logger.error("reading error: {}", exception);
                }
            }
        }
    }

    private void handleUpdate(UpdateResponse response) {
        try{
            if(response instanceof final NewParticipantResponse participantResponse){
                client.participantAdded(participantResponse.getParticipant());
            }
            if(response instanceof final NewEventResponse eventResponse){
                client.eventAdded(eventResponse.getEvent());
            }

            if(response instanceof UpdatedEventsResponse updatedEventsResponse){
                client.newRegistration(updatedEventsResponse.getEventsDTO());
            }
        }catch(Exception e){
            logger.error("Error while handling update " + e.getMessage());
        }
    }
}


