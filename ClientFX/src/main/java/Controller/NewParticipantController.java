package Controller;

import contestUtils.IContestServices;
import example.example.*;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;

import java.util.Properties;

public class NewParticipantController extends AnchorPane {
    //private ParticipantService participantService;
    //private Runnable onParticipantAdded;
    private IContestServices server;
   // private Properties properties;
    private Stage currentStage;
    private User currentUser;

    @FXML
    private TextField nameTextField;

    @FXML
    private TextField ageTextField;

    @FXML
    private Button confirmButton;

    @FXML
    private void onConfirmClicked() {
        try {
            String name = nameTextField.getText();
            int age = Integer.parseInt(ageTextField.getText());
            if (name.isEmpty()) {
                showAlert("Error", "Name cannot be empty!");
                return;
            }
            Participant participant = new Participant(name, age);
            server.saveParticipant(participant);
            //onParticipantAdded.run();
            Stage stage = (Stage) confirmButton.getScene().getWindow();
            stage.close();

        } catch (Exception e) {
            showAlert("Error ", e.getMessage());
        }
    }

    public void init(IContestServices server, User currentUser, Stage currentStage) {
        //this.properties = properties;
        this.server = server;
        this.currentUser = currentUser;
        this.currentStage = currentStage;
        //ParticipantRepository participantRepository = new ParticipantDBRepository(properties);
        //this.participantService = new ParticipantImplementationService(participantRepository);
        nameTextField.clear();
        ageTextField.clear();
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
}

