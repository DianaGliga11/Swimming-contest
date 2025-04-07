//package Controller;
//
//import example.example.*;
//import javafx.collections.ObservableList;
//import javafx.fxml.FXML;
//import javafx.fxml.FXMLLoader;
//import javafx.scene.control.*;
//import javafx.scene.layout.AnchorPane;
//import javafx.stage.Stage;
//
//import java.io.IOException;
//import java.util.Collection;
//import java.util.Properties;
//
//public class OfficeController {
//    private EventService eventService;
//    private ParticipantService participantService;
//    private Properties properties;
//    private Participant currentParticipant;
//    private Stage currentStage;
//    private User currentUser;
//
//    @FXML
//    private ComboBox<Participant> participantComboBox;
//
//    @FXML
//    private ListView<Event> eventListView;
//
//    @FXML
//    protected void onConfirmClicked() throws EntityRepoException {
//        ObservableList<Event> eventsSelected = eventListView.getSelectionModel().getSelectedItems();
//        for (Event event : eventsSelected) {
//            eventService.saveEventEntry(new Office(currentParticipant, event));
//        }
//        try {
//            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/home-view.fxml"));
//            AnchorPane root = fxmlLoader.load();
//            HomeController controller = fxmlLoader.getController();
//            controller.init(properties, currentUser, currentStage);
//            currentStage.getScene().setRoot(root);
//        } catch (IOException e) {
//            showAlert("IOException ", e.getMessage());
//        }
//    }
//
//    @FXML
//    protected void onParticipantSelected() throws EntityRepoException {
//        currentParticipant = participantComboBox.getValue();
//        loadEntries();
//    }
//
//    private void loadEntries() throws EntityRepoException {
//        eventListView.getItems().clear();
//        Collection<Event> events = eventService.getAll();
//        eventListView.getItems().addAll(events);
//    }
//
//    private void showAlert(String title, String message) {
//        Alert alert = new Alert(Alert.AlertType.ERROR);
//        alert.setTitle(title);
//        alert.setHeaderText(null);
//        alert.setContentText(message);
//        alert.showAndWait();
//    }
//}
