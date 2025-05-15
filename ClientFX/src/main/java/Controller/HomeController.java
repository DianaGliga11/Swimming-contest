package Controller;

import DTO.EventDTO;
import DTO.ParticipantDTO;
import contestUtils.IContestServices;
import example.model.Event;
import example.model.Participant;
import example.model.User;
import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.CompletableFuture;

import contestUtils.IMainObserver;

public class HomeController extends AnchorPane implements IMainObserver {
    private IContestServices server;
    private User currentUser;
    //private EventImplementationService eventService;
    //private ParticipantService participantService;
    private Stage currentStage;
    //private Properties properties;

    @FXML
    private ComboBox<Event> eventComboBox;

    @FXML
    private Label usernameLabel;

    @FXML
    private Label searchMessageLabel;

    @FXML
    private TableView<EventDTO> eventTable;

    @FXML
    private TableColumn<EventDTO, String> eventStyle;

    @FXML
    private TableColumn<EventDTO, Integer> eventDistance;

    @FXML
    private TableColumn<EventDTO, Integer> eventParticipantsCount;

    @FXML
    private TableView<Participant> participantTable;

    @FXML
    private TableColumn<Participant, String> participantName;

    @FXML
    private TableColumn<Participant, Integer> participantAge;

    @FXML
    private TableView<ParticipantDTO> searchResultsTable;

    @FXML
    private TableColumn<ParticipantDTO, String> searchNameColumn;

    @FXML
    private TableColumn<ParticipantDTO, Integer> searchAgeColumn;

    @FXML
    private TableColumn<ParticipantDTO, Integer> searchEventCountColumn;

    @FXML
    private VBox searchResultsContainer;

    @FXML
    private void onCloseSearchResults() {
        searchResultsContainer.setVisible(false);
    }

    @FXML
    protected void onSearchClicked() {
        Event selectedEvent = eventComboBox.getValue();
        if (selectedEvent == null) {
            showSearchMessage("Please select an event first!", true);
            return;
        }

        CompletableFuture.runAsync(() -> {
            try {
                Collection<ParticipantDTO> results = server.getParticipantsForEventWithCount(selectedEvent.getId());

                if (results == null || results.isEmpty()) {
                    Platform.runLater(() -> {
                        showSearchMessage("No participants found for this event.", true);
                        searchResultsContainer.setVisible(false);
                    });
                } else {
                    Platform.runLater(() -> {
                        updateSearchResults(results);
                        showSearchResults();
                    });
                }
            } catch (Exception e) {
                Platform.runLater(() -> {
                    showAlert("Search Error", "An error occurred: " + e.getMessage());
                    searchResultsContainer.setVisible(false);
                });
            }
        });
    }


    private void updateSearchResults(Collection<ParticipantDTO> results) {
        if (searchResultsTable != null) {
            CompletableFuture.runAsync(() -> {
                System.out.println("Search result size" + results.size());
                searchResultsTable.getItems().clear();
                searchResultsTable.getItems().addAll(results);
            });
        } else {
            System.err.println("searchResultsTable is null!");
        }
    }

    private void showSearchResults() {
        if (searchResultsContainer != null && searchResultsTable != null) {
            CompletableFuture.runAsync(() -> {
                searchResultsTable.setPlaceholder(new Label("No results found"));
                searchResultsContainer.setVisible(true);
                searchResultsTable.setVisible(true);
                searchMessageLabel.setVisible(false);
            });
        } else {
            System.err.println("Search results components not initialized!");
        }
    }

    @FXML
    protected void onLogoutClicked() {
        try{
            server.logout(currentUser,this);
        } catch (Exception e) {
            showAlert("Logout error ", e.getMessage());
        }
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/main-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            MainController controller = fxmlLoader.getController();
            controller.init(server, currentStage);
            currentStage.setScene(scene);
            currentStage.show();
        } catch (IOException ioException) {
            System.out.println(ioException.getMessage());
        }
    }

    @FXML
    protected void onParticipantButtonClicked() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/new-participant-view.fxml"));
            Parent root = loader.load();
            NewParticipantController controller = loader.getController();
            controller.init(server, currentUser, new Stage());

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("Add New Participant");
            stage.showAndWait();
            initialiseParticipantsTable();
        } catch (IOException e) {
            showAlert("Error", "Cannot open participant form: " + e.getMessage());
        }
    }

    @FXML
    protected void onNewEntryClicked() throws Exception {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/event-view.fxml"));
            Parent root = loader.load();
            EventEntriesController controller = loader.getController();
            controller.init(server, new Stage(), currentUser, this);

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("New Event Registration");
            stage.initOwner(currentStage);
            stage.showAndWait();
        } catch (IOException e) {
            showAlert("Error", "Failed to open new entry form: " + e.getMessage());
        }
        initialiseEventTable();
    }

    public void init(IContestServices server, User currentUser, Stage currentStage) {
        try {
            currentStage.setTitle("Swiming Contest");
            this.server = server;
            this.currentUser = currentUser;
            this.currentStage = currentStage;

            usernameLabel.setText(" (" + currentUser.getUserName() + ")");

            searchNameColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
            searchAgeColumn.setCellValueFactory(new PropertyValueFactory<>("age"));
            searchEventCountColumn.setCellValueFactory(new PropertyValueFactory<>("eventCount"));
            searchResultsContainer.setVisible(false);

            initializeEventComboBox();

            Platform.runLater(() -> {
                try {
                    initializeEventComboBox();
                    initialiseParticipantsTable();
                    initialiseEventTable();
                } catch (Exception e) {
                    showAlert("Init error", "Eroare la inițializare tabele: " + e.getMessage());
                }
            });

        } catch (Exception e) {
            showAlert("An error occurred in init (HomeController): ", e.getMessage());
        }
    }


    private void initializeEventComboBox() {
        CompletableFuture.runAsync(() -> {
            try {
                var events = server.findAllEvents();
                Platform.runLater(() -> {
                    eventComboBox.getItems().clear();
                    eventComboBox.getItems().addAll(events);
                });
            } catch (Exception e) {
                Platform.runLater(() -> showAlert("Error", "Could not load events: " + e.getMessage()));
            }
        });
    }


    private void initialiseParticipantsTable()  {
        CompletableFuture.runAsync(() -> {
            try {
                participantTable.getItems().clear();
                //participantTable.setPlaceholder(new Label("No Participants"));
                participantName.setCellValueFactory(new PropertyValueFactory<>("name"));
                participantAge.setCellValueFactory(new PropertyValueFactory<>("age"));
                Collection<Participant> participants = server.findAllParticipants();
                Platform.runLater(() -> {
                    participantTable.getItems().addAll(participants);

                });
            } catch (Exception e) {
                Platform.runLater(() -> showAlert("Error", "Failed to load participants: " + e.getMessage()));
            }
        });
    }

    private void initialiseEventTable(){
        CompletableFuture.runAsync(() -> {
            try {
                eventTable.getItems().clear();
                eventTable.setPlaceholder(new Label("No Events"));
                eventStyle.setCellValueFactory(new PropertyValueFactory<>("style"));
                eventDistance.setCellValueFactory(new PropertyValueFactory<>("distance"));
                eventParticipantsCount.setCellValueFactory(new PropertyValueFactory<>("participantsCount"));
                Collection<EventDTO> events = server.getEventsWithParticipantsCount();
                Platform.runLater(() -> eventTable.getItems().addAll(events));
            } catch (Exception e) {
                Platform.runLater(() -> showAlert("Error", "Failed to load events: " + e.getMessage()));
            }
        });
    }

    private void showAlert(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    private void showSearchMessage(String message, boolean isError) {
        if (searchMessageLabel != null) {
            searchMessageLabel.setText(message);
            searchMessageLabel.setStyle(isError ? "-fx-text-fill: #a80c0c;" : "-fx-text-fill: #006400;");
            searchMessageLabel.setVisible(true);
        }
        if (searchResultsContainer != null) {
            searchResultsContainer.setVisible(false);
        }
    }

    @Override
    public void participantAdded(Participant participant) {
        Platform.runLater(() -> participantTable.getItems().add(participant));
    }

    @Override
    public void newRegistration(List<EventDTO> events) {
        Platform.runLater(() -> {
            eventTable.getItems().clear();
            eventTable.getItems().addAll(events);
        });
    }

    @Override
    public void eventAdded(EventDTO event) {
        Platform.runLater(() -> eventTable.getItems().add(event));
    }
}

