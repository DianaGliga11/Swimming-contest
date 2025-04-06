package Networking;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import org.example.Office;
import org.example.Participant;
import org.example.User;
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
    private ObjectInputStream objectInputStream;
    private ObjectOutputStream objectOutputStream;
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
    public Collection<EventDTO> getEventsWithParticipantsCount() {
        sendRequest(new GetEventsWithParticipantsCounRequest());
        Response response = readResponse();
        if(response instanceof ErrorResponse){
            logger.error(((ErrorResponse) response).getError());
        }
        GetEventWithParticipantsCountResponse result = (GetEventWithParticipantsCountResponse) response;
        return result.getEvents();
    }

    @Override
    public Collection<ParticipantDTO> getParticipantsForEventWithCount(Long eventId) {
        sendRequest( new GetParticipantsForEventWithCountRequest());
        Response response = readResponse();
        if(response instanceof ErrorResponse) {
            logger.error(((ErrorResponse) response).getError());
        }
        GetParticipantsForEventWithCountResponse result = (GetParticipantsForEventWithCountResponse) response;
        return result.getParticipants();
    }

    @Override
    public void saveEventEntry(List<Office> offices) {
        sendRequest(new CreateEventEntriesRequest(offices));
    }

    @Override
    public Collection<Participant> findAllParticipants() {
        sendRequest(new GetAllParticipantsRequest());
        Response response = readResponse();
        if(response instanceof ErrorResponse){
            logger.error(((ErrorResponse) response).getError());
        }
        AllParticipantsResponse result = (AllParticipantsResponse) response;
        return result.getParticipants();
    }

//    @Override
//    public Collection<Event> findAllEvents() {
//        sendRequest(new GetAllEventsRequest());
//    }

    @Override
    public void saveParticipant(Participant participant) {
        sendRequest(new CreateParticipantRequest(participant));
    }

    private void closeConnection() {
        finished=true;
        try{
            objectInputStream.close();
            objectOutputStream.close();
            connection.close();
            client = null;
        }catch (IOException e){
            logger.error("Error while closing connection " + e.getMessage());
        }
    }

    private Response readResponse() {
        Response response = null;
        try {
            response = responseQueue.take();
        } catch (InterruptedException e) {
            logger.error("Error reading response: " + e.getMessage());
        }
        return response;
    }

    private void sendRequest(Request request) {
        try {
            objectOutputStream.writeObject(request);
            objectOutputStream.flush();
        } catch (IOException e) {
            logger.error("Sending request failed", e);
        }
    }

    private void initializeConnection() {
        try{
            connection = new Socket(host,port);
            objectOutputStream = new ObjectOutputStream(connection.getOutputStream());
            objectOutputStream.flush();
            objectInputStream = new ObjectInputStream(connection.getInputStream());
            finished=false;
            startResponseReader();
        }catch (IOException e){
            logger.error("Error while initializing connection " + e.getMessage());
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
                    Object response = objectInputStream.readObject();
                    System.out.println("response received " + response);
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
                    System.out.println("reading error: " + exception);
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


