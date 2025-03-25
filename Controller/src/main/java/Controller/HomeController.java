package Controller;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.VBox;
import javafx.stage.Modality;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Collection;
import java.util.List;
import java.util.Properties;

public class HomeController extends AnchorPane {
    private User currentUser;
    private EventImplementationService eventService;
    private ParticipantService participantService;
    private Stage currentStage;
    private Properties properties;

    @FXML
    private ComboBox<Event> eventComboBox;

    @FXML
    private Label usernameLabel;

    @FXML
    private Label searchMessageLabel;

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
    private TableView<ParticipantDTO> searchResultsTable;

    @FXML
    private TableColumn<ParticipantDTO, String> searchNameColumn;

    @FXML
    private TableColumn<ParticipantDTO, Integer> searchAgeColumn;

    @FXML
    private TableColumn<ParticipantDTO, Integer> searchEventCountColumn;

    @FXML
    private Button newParticipantButton;

    @FXML
    private Button newEntryButton;

    @FXML
    private VBox searchResultsContainer;


    @FXML
    protected void onSearchClicked() {
        try {
            Event selectedEvent = eventComboBox.getValue();
            if (selectedEvent == null) {
                showSearchMessage("Select an Event first!", true);
                return;
            }

            Collection<ParticipantDTO> results = eventService
                    .getParticipantsForEventWithCount(selectedEvent.getId());

            if (results.isEmpty()) {
                showSearchMessage("No participants :(", true);
            } else {
                searchResultsTable.getItems().clear();
                searchResultsTable.getItems().addAll(results);
                //showSearchResults();
            }

        } catch (EntityRepoException e) {
            showAlert("Error", "Search failed: " + e.getMessage());
        } catch (Exception e) {
            e.printStackTrace();
            showAlert("Error", "Unexpected error: " + e.getMessage());
        }
    }


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
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/org/example/new-participant-view.fxml"));
            Parent root = loader.load();

            // Inițializează controllerul pentru fereastra nouă
            NewParticipantController controller = loader.getController();
//            controller.init(participantService, () -> {
//                try {
//                    initialiseParticipantsTable();
//                } catch (EntityRepoException e) {
//                    showAlert("Error", "Failed to refresh participants: " + e.getMessage());
//                }
//            });

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("Add New Participant");
            stage.initModality(Modality.APPLICATION_MODAL); // Blochează interacțiunea cu alte ferestre
            stage.showAndWait();

        } catch (IOException e) {
            showAlert("Error", "Cannot open participant form: " + e.getMessage());
        }
    }

    @FXML
    protected void onNewEntryClicked() throws EntityRepoException {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/org/example/event-view.fxml"));
            Parent root = loader.load();
            EventEntriesController controller = loader.getController();
            controller.init(properties, currentStage, currentUser);

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("New Event Registration");
            stage.initOwner(currentStage);
            stage.show();
        } catch (IOException | EntityRepoException e) {
            showAlert("Error", "Failed to open new entry form: " + e.getMessage());
            e.printStackTrace();
        }
        initialiseEventTable();
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
        searchNameColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
        searchAgeColumn.setCellValueFactory(new PropertyValueFactory<>("age"));
        searchEventCountColumn.setCellValueFactory(new PropertyValueFactory<>("eventCount"));
        initializeEventComboBox();
        initialiseParticipantsTable();
        initialiseEventTable();
    }

    private void initializeEventComboBox() throws EntityRepoException {
        eventComboBox.getItems().clear();
        eventComboBox.getItems().addAll(eventService.getAll());
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

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }


    private void showSearchResults() {
        searchResultsTable.setVisible(true);
        searchMessageLabel.setVisible(true);
    }

    private void showSearchMessage(String message, boolean isError) {
        searchMessageLabel.setText(message);
        searchMessageLabel.setStyle(isError ? "-fx-text-fill: #a80c0c;" : "-fx-text-fill: black;");
        searchMessageLabel.setVisible(true);
        searchResultsTable.setVisible(true);
    }

}

