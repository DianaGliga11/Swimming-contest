package org.example;

import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Alert.AlertType;
import javafx.scene.control.Button;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

public class LoginController {

    private OfficeService officeService;
    private TextField usernameField;
    private PasswordField passwordField;

    // Constructorul primește un OfficeService și câmpurile pentru username și password
    public LoginController(OfficeService officeService, TextField usernameField, PasswordField passwordField) {
        this.officeService = officeService;
        this.usernameField = usernameField;
        this.passwordField = passwordField;
    }

    // Metoda care se va apela când butonul de login este apăsat
    public void handleLogin() {
        String username = usernameField.getText();
        String password = passwordField.getText();

        // Verificăm logarea
        if (isValidLogin(username, password)) {
            // Dacă login-ul este valid, deschidem fereastra principală
            openMainWindow();
        } else {
            // Dacă datele de login sunt incorecte, afișăm un mesaj de eroare
            showErrorMessage("Login failed", "Invalid username or password.");
        }
    }


    // Verifică dacă datele de login sunt corecte, pe baza configurației din db.config
    private boolean isValidLogin(String username, String password) {
        Properties dbConfig = loadDbConfig();
        String correctUsername = dbConfig.getProperty("jdbc.user");
        String correctPassword = dbConfig.getProperty("jdbc.password");

        return username.equals(correctUsername) && password.equals(correctPassword);
    }

    // Încarcă configurațiile din db.config
    private Properties loadDbConfig() {
        Properties properties = new Properties();
        try (FileInputStream fis = new FileInputStream("db.config")) {
            properties.load(fis);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return properties;
    }

    // Deschide fereastra principală
    private void openMainWindow() {
        MainController mainController = new MainController(officeService);

        // Creăm câmpul de căutare pentru ID-ul evenimentului
        TextField eventSearchField = new TextField();
        eventSearchField.setLayoutX(100);
        eventSearchField.setLayoutY(100);

        Button searchButton = new Button("Căutare");
        searchButton.setLayoutX(100);
        searchButton.setLayoutY(150);

        // Acționăm căutarea la apăsarea butonului
        searchButton.setOnAction(e -> mainController.searchParticipantsByEvent());

        // Layout pentru fereastra principală
        AnchorPane mainRoot = new AnchorPane();
        mainRoot.getChildren().addAll(eventSearchField, searchButton);

        // Crearea scenei pentru fereastra principală
        Scene mainScene = new Scene(mainRoot, 400, 300);
        Stage mainStage = new Stage();
        mainStage.setScene(mainScene);
        mainStage.setTitle("Main Window");
        mainStage.show();
    }

    // Arată un mesaj de eroare
    private void showErrorMessage(String title, String message) {
        Alert alert = new Alert(AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
}

