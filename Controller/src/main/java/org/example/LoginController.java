package org.example;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;

import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class LoginController {

    private OfficeService officeService;

    @FXML
    private TextField usernameField;

    @FXML
    private PasswordField passwordField;

    @FXML
    private AnchorPane loginPane;

    public LoginController(OfficeService officeService) {
        this.officeService = officeService;
    }

    // Metoda pentru verificarea parolei și schimbarea feronței
    @FXML
    public void handleLogin() {
        String username = usernameField.getText();
        String password = passwordField.getText();

        // Verifică dacă login-ul este valid
        if (isLoginValid(username, password)) {
            // Dacă login-ul este valid, deschide fereastra principală
            openMainWindow();
        } else {
            // Dacă login-ul nu este valid, afișează un mesaj de eroare
            System.out.println("Login incorect!");
        }
    }

    private boolean isLoginValid(String username, String password) {
        // Logica pentru validarea login-ului (de exemplu, compara cu o valoare hardcoded)
        return username.equals("admin") && password.equals("admin123");  // exemplu simplu de validare
    }

    private void openMainWindow() {
        try {
            FXMLLoader mainLoader = new FXMLLoader(getClass().getResource("/org/example/main-view.fxml"));
            MainController mainController = new MainController(officeService);
            mainLoader.setController(mainController);
            Scene mainScene = new Scene(mainLoader.load());

            Stage stage = (Stage) loginPane.getScene().getWindow();
            stage.setScene(mainScene);
            stage.setTitle("Aplicatie JavaFX - Main");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
