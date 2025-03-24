package Controller;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Optional;
import java.util.Properties;

public class NewParticipantController extends AnchorPane {
    private ParticipantService participantService;
    private Properties properties;
    private Stage currentStage;
    private User currentUser;

    @FXML
    private TextField name;

    @FXML
    private TextField age;

    @FXML
    private Button confirmButton;

    @FXML
    protected void onConfirmClicked() throws EntityRepoException {
        String name = this.name.getText();
        Integer age = Integer.parseInt(this.age.getText());
        Participant participant = new Participant(name, age);
        participantService.add(participant);
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/event-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            EventEntriesController controller = fxmlLoader.getController();
            Optional<Participant> currentParticipant = participantService.getParticipantByData(participant);
            if (currentParticipant.isPresent()) {
                controller.init(properties, currentStage, currentParticipant.get(), currentUser);
                currentStage.setScene(scene);
                currentStage.show();
            } else {
                System.out.println("No participant found");
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void init(Properties properties, User currentUser, Stage currentStage) {
        currentStage.setTitle("New Participant");
        this.properties = properties;
        participantService = new ParticipantImplementationService(new ParticipantDBRepository(properties));
        this.currentStage = currentStage;
        this.currentUser = currentUser;
        name.clear();
        age.clear();
    }
}

