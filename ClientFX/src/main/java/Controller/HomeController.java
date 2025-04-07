package Controller;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import example.example.*;
import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.Collection;
import java.util.List;
import java.util.Properties;
import contestUtils.IMainObserver;

public class HomeController extends AnchorPane implements IMainObserver{
    private IContestServices server;
    private User currentUser;
    //private EventImplementationService eventService;
    //private ParticipantService participantService;
    private Stage currentStage;
    //private Properties properties;

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
    private VBox searchResultsContainer;

    @FXML
    private void onCloseSearchResults() {
        searchResultsContainer.setVisible(false);
    }

    @FXML
    protected void onSearchClicked() throws Exception {
        try {
            Event selectedEvent = eventComboBox.getValue();
            if (selectedEvent == null) {
                showSearchMessage("Please select an event first!", true);
                return;
            }

            Collection<ParticipantDTO> results = server.getParticipantsForEventWithCount(selectedEvent.getId());

            if (results == null || results.isEmpty()) {
                showSearchMessage("No participants found for this event.", true);
                searchResultsContainer.setVisible(false);
            } else {
                updateSearchResults(results);
                showSearchResults();
            }
        } catch (EntityRepoException e) {
            showAlert("Search Error", "An error occurred: " + e.getMessage());
            searchResultsContainer.setVisible(false);
        }
    }

    private void updateSearchResults(Collection<ParticipantDTO> results) {
        if (searchResultsTable != null) {
            searchResultsTable.getItems().clear();
            searchResultsTable.getItems().addAll(results);
        } else {
            System.err.println("searchResultsTable is null!");
        }
    }

    private void showSearchResults() {
        if (searchResultsContainer != null && searchResultsTable != null) {
            searchResultsContainer.setVisible(true);
            searchResultsTable.setVisible(true);
            searchMessageLabel.setVisible(false);
        } else {
            System.err.println("Search results components not initialized!");
        }
    }


    @FXML
    protected void onLogoutClicked() {
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/main-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            MainController controller = fxmlLoader.getController();
            controller.init(server, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException ioException) {
            System.out.println(ioException.getMessage());
        }
    }

    @FXML
    protected void onParticipantButtonClicked() throws Exception {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/new-participant-view.fxml"));
            Parent root = loader.load();
            NewParticipantController controller = loader.getController();
            controller.init(server, currentUser, new Stage());

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("Add New Participant");
            stage.showAndWait();
            initialiseParticipantsTable();
        } catch (IOException | EntityRepoException e) {
            showAlert("Error", "Cannot open participant form: " + e.getMessage());
        }
    }

    @FXML
    protected void onNewEntryClicked() throws Exception {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/event-view.fxml"));
            Parent root = loader.load();
            EventEntriesController controller = loader.getController();
            controller.init(server, currentStage, currentUser);

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("New Event Registration");
            stage.initOwner(currentStage);
            stage.show();
        } catch (IOException | EntityRepoException e) {
            showAlert("Error", "Failed to open new entry form: " + e.getMessage());
        }
        initialiseEventTable();
    }

    public void init(IContestServices server, User currentUser, Stage currentStage) throws Exception {
        currentStage.setTitle("Swiming Contest");
        this.server = server;
        this.currentUser = currentUser;
//        ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
//        participantService = new ParticipantImplementationService(participantRepository);
//        EventRepository eventRepository = new EventDBRepository(properties);
//        eventService = new EventImplementationService(eventRepository,
//                new OfficeDBRepository(properties, participantRepository, eventRepository));
        this.currentStage = currentStage;
        //this.properties = properties;

        usernameLabel.setText(" (" + currentUser.getUserName() + ")");
        searchNameColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
        searchAgeColumn.setCellValueFactory(new PropertyValueFactory<>("age"));
        searchEventCountColumn.setCellValueFactory(new PropertyValueFactory<>("eventCount"));
        searchResultsContainer.setVisible(false);

        initializeEventComboBox();
        initialiseParticipantsTable();
        initialiseEventTable();
    }

    private void initializeEventComboBox() throws Exception {
        eventComboBox.getItems().clear();
        eventComboBox.getItems().addAll(server.findAllEvents());
    }

    private void initialiseParticipantsTable() throws Exception {
        participantTable.getItems().clear();
        participantTable.setPlaceholder(new Label("No Participants"));
        participantName.setCellValueFactory(new PropertyValueFactory<>("name"));
        participantAge.setCellValueFactory(new PropertyValueFactory<>("age"));
        Collection<Participant> participants = server.findAllParticipants();
        participantTable.getItems().addAll(participants);
    }

    private void initialiseEventTable() throws Exception {
        eventTable.getItems().clear();
        eventTable.setPlaceholder(new Label("No Events"));
        eventStyle.setCellValueFactory(new PropertyValueFactory<>("style"));
        eventDistance.setCellValueFactory(new PropertyValueFactory<>("distance"));
        eventParticipantsCount.setCellValueFactory(new PropertyValueFactory<>("participantsCount"));
        Collection<EventDTO> events = server.getEventsWithParticipantsCount();
        eventTable.getItems().addAll(events);
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    private void showSearchMessage(String message, boolean isError) {
        if (searchMessageLabel != null) {
            searchMessageLabel.setText(message);
            searchMessageLabel.setStyle(isError ? "-fx-text-fill: #a80c0c;" : "-fx-text-fill: #006400;");
            searchMessageLabel.setVisible(true);
        }
        if (searchResultsContainer != null) {
            searchResultsContainer.setVisible(false);
        }
    }

    @Override
    public void participantAdded(Participant participant) throws Exception {
        Platform.runLater(() -> participantTable.getItems().add(participant));
    }

    @Override
    public void newRegistration(List<EventDTO> events) throws Exception {
        Platform.runLater(() -> {
            eventTable.getItems().clear();
            eventTable.getItems().addAll(events);
        });
    }

    @Override
    public void eventAdded(EventDTO event) {
        Platform.runLater(() -> eventTable.getItems().add(event));
    }
}

