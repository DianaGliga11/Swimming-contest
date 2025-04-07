package Controller;

import DTO.EventDTO;
import contestUtils.IContestServices;
import example.example.*;
import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import java.io.IOException;
import java.util.List;
import java.util.Optional;
import java.util.Properties;
import contestUtils.IMainObserver;

public class MainController extends AnchorPane{
    private IContestServices server;
    //private UserService userService;
    //private Properties properties;
    private Stage currentStage;

    @FXML
    private TextField usernameTextField;

    @FXML
    private PasswordField passwordTextField;

    @FXML
    private Button loginButton;

    @FXML
    protected void onLoginButtonClick() {
        String username = usernameTextField.getText();
        String password = passwordTextField.getText();
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/home-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            HomeController controller = fxmlLoader.getController();
            User user = server.login(username, password, controller);
            controller.init(server, user, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (Exception exception) {
            showAlert("Error on Login button: ", exception.getMessage());
        }
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

//    public void init(Properties properties, Stage currentStage) {
//        currentStage.setTitle("Login");
//        UserRepository userRepository = new UserDBRepository(properties);
//        userService = new UserImplementationService(userRepository);
//        this.properties = properties;
//        this.currentStage = currentStage;
//    }

    public void init(IContestServices server, Stage currentStage) {
        currentStage.setTitle("Login");
        this.server=server;
        this.currentStage = currentStage;
    }
}
