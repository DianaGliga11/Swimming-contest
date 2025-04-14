package Networking;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import contestUtils.IMainObserver;
import example.example.Event;
import example.example.Participant;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.Collection;
import java.util.List;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import example.example.User;
import request.*;
import response.*;

public class ClientWorker implements Runnable, IMainObserver {
    private IContestServices contestServices;
    private Socket socketConnection;

    private final static Logger logger = LogManager.getLogger();

    private ObjectInputStream input;
    private ObjectOutputStream output;
    private volatile boolean connected;

    public ClientWorker(IContestServices contestServices, Socket socketConnection) throws Exception {
        this.contestServices = contestServices;
        this.socketConnection = socketConnection;
        try {
            output = new ObjectOutputStream(socketConnection.getOutputStream());
            output.flush();

            input = new ObjectInputStream(socketConnection.getInputStream());
            connected = true;
        } catch (IOException e) {
            logger.error("Server.ClientWorker failed: " + e.getMessage());
            throw new Exception("Server.ClientWorker failed: " + e.getMessage());
        }
    }

    @Override
    public void participantAdded(Participant participant) throws Exception {
        try {
            sendResponse(new NewParticipantResponse(participant));
        } catch (IOException e) {
            logger.error("Server.ClientWorker failed: " + e.getMessage());
            throw new Exception("Server.ClientWorker failed: " + e.getMessage());
        }
    }

    private void sendResponse(Response response) throws IOException {
        logger.info("Server.ClientWorker sendResponse: " + response);
        synchronized (output) {
            output.reset();
            output.writeObject(response);
            output.flush();
        }
    }

    @Override
    public void newRegistration(List<EventDTO> events) throws Exception {
        try {
            sendResponse(new UpdatedEventsResponse(events));
        } catch (IOException e) {
            logger.error("New registration failed: " + e.getMessage());
            throw new Exception("New registration failed: " + e.getMessage());
        }
    }

    @Override
    public void eventAdded(EventDTO event) throws Exception {
        try{
            sendResponse(new NewEventResponse(event));
        }catch (IOException e){
            logger.error("New event failed: " + e.getMessage());
            throw new Exception("New event failed: " + e.getMessage());
        }
    }


    @Override
    public void run() {
        while (connected) {
            try {
                Object request = input.readObject();
                Response response = handleRequest((Request) request);
                if (response != null) {
                    sendResponse(response);
                }
            } catch (IOException | ClassNotFoundException e) {
                logger.error("Response failed: " + e.getMessage());
                connected=false;
                e.printStackTrace();
            }
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                logger.error("Thread failed: " + e.getMessage());
                e.printStackTrace();
                connected=false;
            }
        }
        try {
            input.close();
            output.close();
            socketConnection.close();
        } catch (IOException e) {
            logger.error("Socket connection close failed: " + e.getMessage());
            e.printStackTrace();
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
                return new ErrorResponse(e.getMessage());
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
                return new ErrorResponse(e.getMessage());
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
                return new ErrorResponse(e.getMessage());
            }
        }

        if(request instanceof CreateEventEntriesRequest createEventEntriesRequest){
            logger.info("Create event request..");
            try{
                contestServices.saveEventEntry(createEventEntriesRequest.getEventEntries());
                return new UpdatedEventsResponse(contestServices.getEventsWithParticipantsCount().stream().toList());
            } catch (Exception e) {
                logger.error("Create event entries failed: " + e.getMessage());
                return new ErrorResponse(e.getMessage());
            }
        }

        if(request instanceof GetEventsWithParticipantsCounRequest getEventsWithParticipantsCounRequest){
            logger.info("Get event request..");
            try{
                return new GetEventWithParticipantsCountResponse(contestServices.getEventsWithParticipantsCount());
            }catch (Exception e){
                logger.error("Get event request failed: " + e.getMessage());
                return new ErrorResponse(e.getMessage());
            }
        }

        if(request instanceof GetAllParticipantsRequest getAllParticipantsRequest){
            logger.info("Get all participant request..");
            try{
                return new AllParticipantsResponse(contestServices.findAllParticipants());
            }catch (Exception e){
                logger.error("Get all participant request failed: " + e.getMessage());
                return new ErrorResponse(e.getMessage());
            }
        }

        if (request instanceof GetAllEventsRequest getAllEventsRequest) {
            logger.info("Get all events request...");
            try {
                Collection<Event> events = contestServices.findAllEvents();
                return new AllEventsResponse(events); // Asigură-te că această clasă există
            } catch (Exception e) {
                logger.error("Get all events failed: " + e.getMessage());
                return new ErrorResponse(e.getMessage());
            }
        }

        if (request instanceof GetParticipantsForEventWithCountRequest getParticipantsForEventRequest) {
            try {
                Collection<ParticipantDTO> participants = contestServices.getParticipantsForEventWithCount(getParticipantsForEventRequest.getEventId());
                return new GetParticipantsForEventWithCountResponse(participants);
            } catch (Exception e) {
                logger.error("Get participants for event failed: " + e.getMessage());
                return new ErrorResponse(e.getMessage());
            }
        }


        return null;
    }
}
