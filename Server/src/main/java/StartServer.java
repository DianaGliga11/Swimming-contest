import example.example.*;
import contestUtils.ContestServices;
import contestUtils.IContestServices;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.IOException;
import java.rmi.ServerException;
import java.util.Properties;

public class StartServer {
    public static final int DEFAULT_PORT = 55558;
    public static final Logger logger = LogManager.getLogger();

    public static void main(String[] args) {
        Properties serverProperties = new Properties();
        try{
            serverProperties.load(StartServer.class.getResourceAsStream("/server.properties"));
            logger.info("Server properties loaded");
            serverProperties.list(System.out);
        }catch (IOException e){
            logger.error("Cannot find file server.properties " + e.getMessage());
            return;
        }
        IContestServices eventServices = initializeServices(serverProperties);
        int server_port = DEFAULT_PORT;
        try{
            server_port = Integer.parseInt(serverProperties.getProperty("server.port"));
        }catch (NumberFormatException e){
            logger.error("Wrong port number:" + e.getMessage());
            logger.info("Using default port: " + DEFAULT_PORT);
        }
        logger.info("Starting server on port " + server_port);

        Server server = new ProtocolBufferServer(server_port, eventServices);
        try{
            server.start();
        } catch (ServerException e) {
            logger.error("Cannot start server " + e.getMessage());
        }
    }

    private static IContestServices initializeServices(Properties serverProperties) {

        ParticipantRepository participantRepository = new ParticipantDBRepository(serverProperties);
        EventRepository eventRepository = new EventDBRepository(serverProperties);
        OfficeRepository officeRepository = new OfficeDBRepository(serverProperties, participantRepository, eventRepository);
        UserRepository userRepository = new UserDBRepository(serverProperties);

        ParticipantService participantService = new ParticipantImplementationService(participantRepository);
        EventService eventService = new EventImplementationService(eventRepository, officeRepository);
        UserService userService = new UserImplementationService(userRepository);

        return new ContestServices(eventService,participantService,userService);
    }
}
