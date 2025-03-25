package Controller;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.layout.AnchorPane;
import javafx.scene.control.TextField;
import javafx.scene.control.Button;
import javafx.stage.Stage;
import org.example.EntityRepoException;
import org.example.Event;
import org.example.EventImplementationService;
import org.example.User;

import java.io.IOException;
import java.util.Properties;

public class NewEventController extends AnchorPane {
    private EventImplementationService eventService;
    private Properties properties;
    private User currentUser;
    private Stage currentStage;

    @FXML
    private TextField styleField;

    @FXML
    private TextField distanceField;

    @FXML
    protected void onConfirmClicked() throws EntityRepoException {
        String style = styleField.getText();
        Integer distance = Integer.valueOf(distanceField.getText());
        eventService.add(new Event(style, distance));
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/home-view.fxml"));
            AnchorPane root = fxmlLoader.load();
            HomeController controller = fxmlLoader.getController();
            controller.init(properties, currentUser, currentStage);
            currentStage.getScene().setRoot(root);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}