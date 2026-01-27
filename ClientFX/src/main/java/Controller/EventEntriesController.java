package Controller;

import contestUtils.IContestServices;
import example.model.Event;
import example.model.Office;
import example.model.Participant;
import example.model.User;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class EventEntriesController extends AnchorPane {
    private IContestServices server;
    //private EventService eventService;
    //private ParticipantService participantService;
    //private Properties properties;
    private Participant currentParticipant;
    private Stage currentStage;
    private User currentUser;

    private HomeController homeController;

    @FXML
    private ComboBox<Participant> participantBox;

    @FXML
    private ListView<Event> eventListView;

    @FXML
    protected void onConfirmClicked() throws Exception {
        ObservableList<Event> eventsSelected = eventListView.getSelectionModel().getSelectedItems();
        List<Office> eventEntries = new ArrayList<>();
        for (Event event : eventsSelected) {
            eventEntries.add(new Office(currentParticipant, event));
        }

        try {
            server.saveEventEntry(eventEntries);
            ((Stage) this.getScene().getWindow()).close();
        } catch (IOException e) {
            showAlert("Eroare", e.getMessage());
        }
    }


    @FXML
    protected void onParticipantSelected()  {
        currentParticipant = participantBox.getValue();
        loadEvents();
    }

    public void init(IContestServices server, Stage currentStage, User currentUser, HomeController homeController) {
        try {
            initialise(server, currentStage, currentUser);
            this.homeController = homeController;
            setParticipants();
            eventListView.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
            loadEvents();
        }catch (Exception e){
            showAlert("Error in init (EventEvtriesController) ", e.getMessage());
        }
    }

    private void setParticipants() throws Exception {
        participantBox.getItems().clear();
        Collection<Participant> participants = server.findAllParticipants();
        participantBox.getItems().addAll(participants);
        participantBox.getSelectionModel().clearAndSelect(0);
        currentParticipant = participantBox.getValue();
    }

    private void loadEvents() {
        new Thread(() -> {
            try {
                Collection<Event> events = server.findAllEvents();
                javafx.application.Platform.runLater(() -> {
                    eventListView.getItems().setAll(events);
                });
            } catch (Exception e) {
                javafx.application.Platform.runLater(() ->
                        showAlert("Error", "Failed to load events: " + e.getMessage())
                );
            }
        }).start();
    }

    private void initialise(IContestServices server, Stage currentStage, User currentUser) {
        this.currentUser = currentUser;
        this.server = server;
        //this.properties = properties;
        //final EventRepository eventRepository = new EventDBRepository(properties);
        //final ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
        //final OfficeRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);
        //eventService = new EventImplementationService(eventRepository, officeRepository);
        //participantService = new ParticipantImplementationService(participantRepository);
        this.currentStage = currentStage;
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
}
