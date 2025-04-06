package Controller;

import contestUtils.IContestServices;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.*;
import javafx.scene.layout.AnchorPane;
import javafx.scene.Scene;
import javafx.stage.Stage;
import org.example.*;

import java.io.IOException;
import java.util.Optional;
import java.util.Properties;

public class MainController extends AnchorPane {
    private IContestServices server;
    private UserService userService;
    private Properties properties;
    private Stage currentStage;

    @FXML
    private TextField usernameTextField;

    @FXML
    private PasswordField passwordTextField;

    @FXML
    protected void onLoginButtonClick() {
        String username = usernameTextField.getText();
        String password = passwordTextField.getText();
        try {
            Optional<User> user = userService.getLogin(username, password);
            if (user.isPresent()) {
                FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/org/example/home-view.fxml"));
                AnchorPane root = fxmlLoader.load();
                HomeController controller = fxmlLoader.getController();
                controller.init(properties, user.get(), currentStage);
                currentStage.getScene().setRoot(root);
            } else {
                showAlert("Login failed", "Invalid username or password");
            }
        } catch (EntityRepoException e) {
            throw new RuntimeException(e);
        } catch (IOException e) {
            System.out.println(e.getMessage());
        }
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    public void init(Properties properties, Stage currentStage) {
        currentStage.setTitle("Login");
        UserRepository userRepository = new UserDBRepository(properties);
        userService = new UserImplementationService(userRepository);
        this.properties = properties;
        this.currentStage = currentStage;
    }

    public void init(IContestServices server, Stage currentStage) {
        currentStage.setTitle("Login");
        this.server=server;
        this.currentStage = currentStage;
    }
}
