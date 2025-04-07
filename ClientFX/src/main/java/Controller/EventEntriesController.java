package Controller;

import contestUtils.IContestServices;
import example.example.*;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Properties;

public class EventEntriesController extends AnchorPane {
    private IContestServices server;
    //private EventService eventService;
    //private ParticipantService participantService;
    //private Properties properties;
    private Participant currentParticipant;
    private Stage currentStage;
    private User currentUser;

    @FXML
    private ComboBox<Participant> participantBox;

    @FXML
    private ListView<Event> eventListView;

    @FXML
    protected void onConfirmClicked() throws Exception {
        ObservableList<Event> eventsSelected = eventListView.getSelectionModel().getSelectedItems();
        List<Office> eventEntries = new ArrayList<>();
        for (Event event : eventsSelected) {
            eventEntries.add(new Office(currentParticipant,event));
        }

        try {
            server.saveEventEntry(eventEntries );
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/home-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            HomeController controller = fxmlLoader.getController();
            controller.init(server, currentUser, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException e) {
            System.out.println(e.getMessage());
        }
    }

    @FXML
    protected void onParticipantSelected() throws Exception {
        currentParticipant = participantBox.getValue();
        loadEvents();
    }

    public void init(IContestServices server, Stage currentStage, User currentUser) throws Exception {
        initialise(server, currentStage, currentUser);
        setParticipants();
        eventListView.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        loadEvents();
    }

    private void setParticipants() throws Exception {
        participantBox.getItems().clear();
        Collection<Participant> participants = server.findAllParticipants();
        participantBox.getItems().addAll(participants);
        participantBox.getSelectionModel().clearAndSelect(0);
        currentParticipant = participantBox.getValue();
    }

    private void loadEvents() throws Exception {
        eventListView.getItems().clear();
        Collection<Event> events = server
                .findAllEvents();
        eventListView.getItems().addAll(events);
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
}
