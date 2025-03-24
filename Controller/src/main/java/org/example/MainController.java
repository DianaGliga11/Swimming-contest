package org.example;

import javafx.fxml.FXML;
import javafx.scene.control.ListView;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.OfficeService;
import org.example.Participant;
import org.example.Event;

import java.util.List;
import java.util.Map;

public class MainController {

    private OfficeService officeService;

    @FXML
    private TextField eventSearchField;

    @FXML
    private ListView<String> participantListView;

    @FXML
    private AnchorPane mainPane;

    public MainController(OfficeService officeService) {
        this.officeService = officeService;
    }

    @FXML
    public void searchParticipantsByEvent() {
        try {
            String eventName = eventSearchField.getText();
            Long eventId = getEventIdByName(eventName);  // Metodă de a obține ID-ul evenimentului pe baza numelui
            List<Participant> participants = officeService.findParticipantsByEvent(eventId);
            updateParticipantListView(participants);
        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("Eroare la căutarea participanților!");
        }
    }

    private void updateParticipantListView(List<Participant> participants) {
        participantListView.getItems().clear();
        for (Participant p : participants) {
            participantListView.getItems().add(p.getName());  // Adaugă numele participantului în ListView
        }
    }

    private Long getEventIdByName(String eventName) {
        // Căutăm ID-ul evenimentului pe baza numelui (poți implementa această metodă cum vrei)
        return 1L;  // Exemplu simplu
    }

    @FXML
    public void registerParticipant() {
        try {
            Long participantId = 1L;  // Exemplu de participant ID
            List<Long> eventIds = List.of(1L);  // Exemplu de ID-uri evenimente
            officeService.registerParticipantToEvents(participantId, eventIds);
            System.out.println("Participantul a fost înregistrat.");
        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("Eroare la înregistrarea participantului.");
        }
    }
}
