package org.example;

import javafx.scene.control.Alert;
import javafx.scene.control.Alert.AlertType;
import javafx.scene.control.ListView;
import javafx.scene.layout.AnchorPane;
import javafx.scene.Scene;
import javafx.stage.Stage;

import java.util.Map;

public class MainController {

    private OfficeService officeService;

    private ListView<String> eventListView; // ListView pentru a afișa evenimentele și numărul de participanți

    // Constructorul primește OfficeService pentru a avea acces la metodele de căutare
    public MainController(OfficeService officeService) {
        this.officeService = officeService;

        // Creăm UI-ul pentru fereastra principală
        eventListView = new ListView<>();
        eventListView.setLayoutX(100);
        eventListView.setLayoutY(100);
        eventListView.setPrefWidth(300);
        eventListView.setPrefHeight(200);
    }

    // Metoda care se va apela pentru a încărca evenimentele și numărul de participanți
//    public void loadEventsAndParticipants() {
//        try {
//            // Obținem lista de evenimente și numărul de participanți
//            Map<Event, Integer> eventsWithParticipants = officeService.getEventsWithParticipantsCount();
//
//            // Verificăm dacă am găsit evenimente
//            if (eventsWithParticipants.isEmpty()) {
//                showErrorMessage("Nu au fost găsite evenimente", "Nu există evenimente în baza de date.");
//            } else {
//                // Actualizăm ListView cu evenimentele și numărul de participanți
//                eventListView.getItems().clear();
//                for (Map.Entry<Event, Integer> entry : eventsWithParticipants.entrySet()) {
//                    Event event = entry.getKey();
//                    int participantsCount = entry.getValue();
//                    eventListView.getItems().add(event.getName() + " - " + participantsCount + " participanți");
//                }
//            }
//        } catch (Exception e) {
//            e.printStackTrace();
//            showErrorMessage("Eroare", "A apărut o problemă la încărcarea evenimentelor.");
//        }
//    }

    // Metoda pentru a arăta mesajele de eroare
    private void showErrorMessage(String title, String message) {
        Alert alert = new Alert(AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    // Deschide fereastra principală
    public void openMainWindow(Stage mainStage) {
        // Layout pentru fereastra principală
        AnchorPane root = new AnchorPane();
        root.getChildren().add(eventListView);

        // Crearea scenei pentru fereastra principală
        Scene mainScene = new Scene(root, 400, 300);
        mainStage.setScene(mainScene);
        mainStage.setTitle("Evenimente și Participanți");
        mainStage.show();

        // Încărcăm evenimentele și participanții
        //loadEventsAndParticipants();
    }
}
