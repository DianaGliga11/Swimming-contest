package Controller;

import DTO.EventDTO;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Collection;
import java.util.Properties;

public class HomeController extends AnchorPane {
    private User currentUser;
    private EventImplementationService eventService;
    private ParticipantService participantService;
    private Stage currentStage;
    private Properties properties;

    @FXML
    private Label usernameLabel;

    @FXML
    private TableView<EventDTO> eventTable;

    @FXML
    private TableColumn<EventDTO, String> eventStyle;

    @FXML
    private TableColumn<EventDTO, Integer> eventDistance;

    @FXML
    private TableColumn<EventDTO, Integer> eventParticipantsCount;

    @FXML
    private TableView<Participant> participantTable;

    @FXML
    private TableColumn<Participant, String> participantName;

    @FXML
    private TableColumn<Participant, Integer> participantAge;

    @FXML
    private Button newParticipantButton;

    @FXML
    private Button newEntryButton;

    @FXML
    protected void onEventClicked() {
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/newEvent-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            NewEventController controller = fxmlLoader.getController();
            controller.init(properties, eventService, currentUser, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException e) {
            System.out.println(e);
        }
    }

    @FXML
    protected void onLogoutClicked() {
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/main-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            MainController controller = fxmlLoader.getController();
            controller.init(properties, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException ioException) {
            System.out.println(ioException.getMessage());
        }
    }

    @FXML
    protected void onParticipantButtonClicked() {
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/new-participant.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            NewParticipantController controller = fxmlLoader.getController();
            controller.init(properties, currentUser, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException ioException) {
            System.out.println(ioException.getMessage());
        }
    }

    @FXML
    protected void onNewEntryClicked() {
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/event-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            OfficeController controller = fxmlLoader.getController();
            controller.init(properties, currentStage, currentUser);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException | EntityRepoException ioException) {
            System.out.println(ioException.getMessage());
        }
    }

    public void init(Properties properties, User currentUser, Stage currentStage) throws EntityRepoException {
        currentStage.setTitle("Swiming Contest");
        this.currentUser = currentUser;
        ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
        participantService = new ParticipantImplementationService(participantRepository);
        EventRepository eventRepository = new EventDBRepository(properties);
        eventService = new EventImplementationService(eventRepository,
                new OfficeDBRepository(properties, participantRepository, eventRepository));
        this.currentStage = currentStage;
        this.properties = properties;

        usernameLabel.setText(" (" + currentUser.getUserName() + ")");
        initialiseParticipantsTable();
        initialiseEventTable();
    }

    private void initialiseParticipantsTable() throws EntityRepoException {
        participantTable.getItems().clear();
        participantTable.setPlaceholder(new Label("No Participants"));
        participantName.setCellValueFactory(new PropertyValueFactory<>("name"));
        participantAge.setCellValueFactory(new PropertyValueFactory<>("age"));
        Collection<Participant> participants = participantService.getAll();
        participantTable.getItems().addAll(participants);
    }

    private void initialiseEventTable() throws EntityRepoException {
        eventTable.getItems().clear();
        eventTable.setPlaceholder(new Label("No Events"));
        eventStyle.setCellValueFactory(new PropertyValueFactory<>("style"));
        eventDistance.setCellValueFactory(new PropertyValueFactory<>("distance"));
        eventParticipantsCount.setCellValueFactory(new PropertyValueFactory<>("participantsCount"));
        Collection<EventDTO> events = eventService.getEventsWithParticipantsCount();
        eventTable.getItems().addAll(events);
    }
}
