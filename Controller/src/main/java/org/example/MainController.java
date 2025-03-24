package org.example;

import javafx.fxml.FXML;
import javafx.scene.control.ListView;
import javafx.scene.control.TextField;
import javafx.scene.control.Alert;
import javafx.scene.control.Alert.AlertType;
import javafx.scene.input.MouseEvent;

import java.util.List;

public class MainController {

    private OfficeService officeService;

    @FXML
    private TextField eventSearchField; // Câmpul de căutare pentru ID-ul evenimentului

    @FXML
    private ListView<String> participantListView; // ListView pentru a afișa participanții

    // Constructorul primește OfficeService pentru a avea acces la metodele de căutare
    public MainController(OfficeService officeService) {
        this.officeService = officeService;
    }

    // Metoda care se va apela când se apasă butonul de căutare
    @FXML
    public void searchParticipantsByEvent() {
        try {
            // Obține ID-ul evenimentului din câmpul de text
            String eventIdText = eventSearchField.getText();
            if (eventIdText == null || eventIdText.trim().isEmpty()) {
                showErrorMessage("ID-ul evenimentului este necesar", "Vă rugăm să introduceți un ID valid al evenimentului.");
                return;
            }

            // Convertim ID-ul evenimentului într-un long
            long eventId = Long.parseLong(eventIdText);

            // Căutăm participanții pe baza ID-ului evenimentului
            List<Participant> participants = officeService.findParticipantsByEvent(eventId);

            // Verificăm dacă am găsit participanți
            if (participants.isEmpty()) {
                showErrorMessage("Nu au fost găsiți participanți", "Nu există participanți pentru acest eveniment.");
            } else {
                // Actualizăm ListView cu numele participanților
                participantListView.getItems().clear();
                for (Participant participant : participants) {
                    participantListView.getItems().add(participant.getName());
                }
            }
        } catch (NumberFormatException e) {
            showErrorMessage("ID invalid", "Vă rugăm să introduceți un ID valid al evenimentului.");
        } catch (Exception e) {
            e.printStackTrace();
            showErrorMessage("Eroare", "A apărut o problemă la căutarea participanților.");
        }
    }

    // Metoda pentru a arăta mesajele de eroare
    private void showErrorMessage(String title, String message) {
        Alert alert = new Alert(AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    // Eventual, metoda de click pe un participant pentru a arăta detalii suplimentare
    @FXML
    public void handleParticipantSelection(MouseEvent event) {
        String selectedParticipant = participantListView.getSelectionModel().getSelectedItem();
        if (selectedParticipant != null) {
            // Aici poți adăuga logica pentru a arăta detalii despre participant
            System.out.println("Ai selectat participantul: " + selectedParticipant);
        }
    }
}
