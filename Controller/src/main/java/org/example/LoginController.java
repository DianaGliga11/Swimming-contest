package org.example;

import javafx.fxml.FXML;
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
    private Stage loginStage;

    // Constructorul primește serviciul și câmpurile de login
    public LoginController(OfficeService officeService, TextField usernameField, PasswordField passwordField) {
        this.officeService = officeService;
        this.usernameField = usernameField;
        this.passwordField = passwordField;
    }

    // Metoda care se va apela când butonul de login este apăsat
    @FXML
    public void handleLogin() {
        String username = usernameField.getText();
        String password = passwordField.getText();

        // Verificăm logarea
        if (isValidLogin(username, password)) {
            // Dacă login-ul este valid, închidem fereastra de login și deschidem fereastra principală
            closeLoginWindow();
            openMainWindow();
        } else {
            showErrorMessage("Login failed", "Invalid username or password.");
        }
    }

    // Închide fereastra de login
    private void closeLoginWindow() {
        if (loginStage != null) {
            loginStage.close();
        }
    }

    // Deschide fereastra principală
    private void openMainWindow() {
        MainController mainController = new MainController(officeService);
       Stage mainStage = new Stage();
       mainController.openMainWindow(mainStage);
    }

    // Verifică dacă datele de login sunt corecte
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

    // Arată un mesaj de eroare
    private void showErrorMessage(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    // Setează stage-ul de login
    public void setLoginStage(Stage stage) {
        this.loginStage = stage;
    }
}


