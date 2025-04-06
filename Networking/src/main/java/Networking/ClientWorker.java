package Networking;

import DTO.EventDTO;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import org.example.Participant;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.List;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.example.User;
import request.*;
import response.*;

public class ClientWorker implements Runnable, IMainObserver {
    private IContestServices contestServices;
    private Socket socketConnection;

    private final static Logger logger = LogManager.getLogger();

    private ObjectInputStream objectInputStream;
    private ObjectOutputStream objectOutputStream;
    private volatile boolean connected;

    public ClientWorker(IContestServices contestServices, Socket socketConnection) {
        this.contestServices = contestServices;
        this.socketConnection = socketConnection;
        try {
            objectOutputStream = new ObjectOutputStream(socketConnection.getOutputStream());
            objectOutputStream.flush();

            objectInputStream = new ObjectInputStream(socketConnection.getInputStream());
            connected = true;
        } catch (IOException e) {
            logger.error("Server.ClientWorker failed: " + e.getMessage());
        }
    }

    @Override
    public void participantAdded(Participant participant) {
        try {
            sendResponse(new NewParticipantResponse(participant));
        } catch (IOException e) {
            logger.error("Server.ClientWorker failed: " + e.getMessage());
        }
    }

    private void sendResponse(Response response) throws IOException {
        logger.info("Server.ClientWorker sendResponse: " + response);
        synchronized (objectOutputStream) {
            objectOutputStream.writeObject(response);
            objectOutputStream.flush();
        }
    }

    @Override
    public void newRegistration(List<EventDTO> events) {
        try {
            sendResponse(new UpdatedEventsResponse(events));
        } catch (IOException e) {
            logger.error("New registration failed: " + e.getMessage());
        }
    }

    @Override
    public void eventAdded(EventDTO event) {

    }


    @Override
    public void run() {
        while (connected) {
            try {
                Object request = objectInputStream.readObject();
                Response response = handleRequest((Request) request);
                if (response != null) {
                    sendResponse(response);
                }
            } catch (IOException | ClassNotFoundException e) {
                logger.error("Response failed: " + e.getMessage());
            }
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                logger.error("Thread failed: " + e.getMessage());
            }
        }
        try {
            objectInputStream.close();
            objectOutputStream.close();
            socketConnection.close();
        } catch (IOException e) {
            logger.error("Socket connection close failed: " + e.getMessage());
        }
    }

    private Response handleRequest(Request request) {
        if(request instanceof LoginRequest loginRequest) {
            logger.info("Login request...");
            String username = loginRequest.getUsername();
            String password = loginRequest.getPassword();
            try{
                User user = contestServices.login(username,password,this);
                return new OKResponse(user);
            }catch (Exception e){
                logger.error("Login failed: " + e.getMessage());
            }
        }

        if(request instanceof LogoutRequest logoutRequest){
            logger.info("Logout request...");
            User user = logoutRequest.getUser();
            try{
                contestServices.logout(user,this);
                return new OKResponse(null);
            }catch (Exception e){
                logger.error("Logout failed: " + e.getMessage());
            }
        }

        if(request instanceof CreateParticipantRequest createParticipantRequest){
            logger.info("Create participant request...");
            Participant participant = createParticipantRequest.getParticipant();
            try {
                contestServices.saveParticipant(participant);
                return new NewParticipantResponse(participant);
            } catch (Exception e) {
                logger.error("Create participant failed: " + e.getMessage());
            }
        }

        if(request instanceof CreateEventEntriesRequest createEventEntriesRequest){
            logger.info("Create event request..");
            try{
                contestServices.saveEventEntry(createEventEntriesRequest.getEventEntries());
                return new UpdatedEventsResponse(contestServices.getEventsWithParticipantsCount().stream().toList());
            } catch (Exception e) {
                logger.error("Create event entries failed: " + e.getMessage());
            }
        }

        if(request instanceof GetEventsWithParticipantsCounRequest getEventsWithParticipantsCounRequest){
            logger.info("Get event request..");
            try{
                return new GetEventWithParticipantsCountResponse(contestServices.getEventsWithParticipantsCount());
            }catch (Exception e){
                logger.error("Get event request failed: " + e.getMessage());
            }
        }

        return null;
    }
}
