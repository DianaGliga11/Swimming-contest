package Controller;

import contestUtils.IContestServices;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Alert;
import javafx.scene.layout.AnchorPane;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import example.example.EntityRepoException;
import example.example.Event;
import example.example.EventImplementationService;
import example.example.User;

import java.io.IOException;
import java.util.Properties;

public class NewEventController extends AnchorPane {
    //private EventImplementationService eventService;
    //private Properties properties;
    private IContestServices server;
    private User currentUser;
    private Stage currentStage;

    @FXML
    private TextField styleField;

    @FXML
    private TextField distanceField;

    @FXML
    protected void onConfirmClicked() throws Exception {
        String style = styleField.getText();
        int distance = Integer.parseInt(distanceField.getText());
        server.saveEvent(new Event(style, distance));
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/home-view.fxml"));
            AnchorPane root = fxmlLoader.load();
            HomeController controller = fxmlLoader.getController();
            controller.init(server, currentUser, currentStage);
            currentStage.getScene().setRoot(root);
        } catch (IOException e) {
            showAlert("IOException ", e.getMessage());
        }
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

}