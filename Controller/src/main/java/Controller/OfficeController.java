package Controller;

import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Collection;
import java.util.Properties;

public class OfficeController {
    private EventService eventService;
    private ParticipantService participantService;
    private Properties properties;
    private Participant currentParticipant;
    private Stage currentStage;
    private User currentUser;

    @FXML
    private ComboBox<Participant> participantComboBox;

    @FXML
    private ListView<Event> eventListView;

    @FXML
    private Label eventFoundLabel;

    @FXML
    private Button confirmButton;

    @FXML
    protected void onConfirmClicked() throws EntityRepoException {
        ObservableList<Event> eventsSelected = eventListView.getSelectionModel().getSelectedItems();
        for (Event event : eventsSelected) {
            eventService.saveEventEntry(new Office(currentParticipant, event));
        }
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/home-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            HomeController controller = fxmlLoader.getController();
            controller.init(properties, currentUser, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @FXML
    protected void onParticipantSelected() throws EntityRepoException {
        currentParticipant = participantComboBox.getValue();
        loadRaces();
    }

    public void init(Properties properties, Stage currentStage, User currentUser) throws EntityRepoException {
        initialise(properties, currentStage, currentUser);
        setParticipants();
        eventListView.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        loadRaces();
    }

    public void init(Properties properties, Stage currentStage, Participant currentParticipant, User currentUser) throws EntityRepoException {
        currentStage.setTitle("Register to Events");
        initialise(properties, currentStage, currentUser);
        setNewlyCreatedParticipant(currentParticipant);
        eventListView.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        loadRaces();
    }

    private void initialise(Properties properties, Stage currentStage, User currentUser) {
        this.currentUser = currentUser;
        this.properties = properties;
        final EventRepository eventRepository = new EventDBRepository(properties);
        final ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
        final OfficeRepository officeRepository =
                new OfficeDBRepository(properties, participantRepository, eventRepository);
        eventService = new EventImplementationService(eventRepository, officeRepository);
        participantService = new ParticipantImplementationService(participantRepository);
        this.currentStage = currentStage;
    }

    private void setNewlyCreatedParticipant(Participant participant) {
        currentParticipant = participant;
        participantComboBox.getItems().clear();
        participantComboBox.getItems().add(currentParticipant);
        participantComboBox.getSelectionModel().clearAndSelect(0);
    }

    private void setParticipants() throws EntityRepoException {
        participantComboBox.getItems().clear();
        Collection<Participant> participants = participantService.getAll();
        participantComboBox.getItems().addAll(participants);
        participantComboBox.getSelectionModel().clearAndSelect(0);
        currentParticipant = participantComboBox.getValue();
    }

    private void loadRaces() throws EntityRepoException {
        eventListView.getItems().clear();
        Collection<Event> events = eventService.getAll();
        eventListView.getItems().addAll(events);
    }
}
