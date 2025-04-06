package Controller;

import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Collection;
import java.util.List;
import java.util.Properties;

public class EventEntriesController extends AnchorPane {
    private EventService eventService;
    private ParticipantService participantService;
    private Properties properties;
    private Participant currentParticipant;
    private Stage currentStage;
    private User currentUser;

    @FXML
    private ComboBox<Participant> participantBox;

    @FXML
    private ListView<Event> eventListView;

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
            System.out.println(e.getMessage());
        }
    }

    @FXML
    protected void onParticipantSelected() throws EntityRepoException {
        currentParticipant = participantBox.getValue();
        loadEvents();
    }

    public void init(Properties properties, Stage currentStage, User currentUser) throws EntityRepoException {
        initialise(properties, currentStage, currentUser);
        setParticipants();
        eventListView.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        loadEvents();
    }

    private void setParticipants() throws EntityRepoException {
        participantBox.getItems().clear();
        List<Participant> participants = participantService.getAll();
        participantBox.getItems().addAll(participants);
        participantBox.getSelectionModel().clearAndSelect(0);
        currentParticipant = participantBox.getValue();
    }

    private void loadEvents() throws EntityRepoException {
        eventListView.getItems().clear();
        Collection<Event> events = eventService
                .getAll();
        eventListView.getItems().addAll(events);
    }

    private void initialise(Properties properties, Stage currentStage, User currentUser) {
        this.currentUser = currentUser;
        this.properties = properties;
        final EventRepository eventRepository = new EventDBRepository(properties);
        final ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
        final OfficeRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);
        eventService = new EventImplementationService(eventRepository, officeRepository);
        participantService = new ParticipantImplementationService(participantRepository);
        this.currentStage = currentStage;
    }
}
