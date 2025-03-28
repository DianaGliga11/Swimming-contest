using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace SwimmingCompetitionController
{
    public class HomeController : Form
    {
        private IDictionary<string, string> properties;
        private User currentUser;
        private I_EventService eventService;
        private I_ParticipantService participantService;
        
        private ComboBox eventComboBox;
        private Label usernameLabel;
        private Label searchMessageLabel;
        private DataGridView eventTable;
        private DataGridView participantTable;
        private DataGridView searchResultsTable;
        private Panel searchResultsContainer;
        private Button btnCloseSearchResults;
        private Button btnSearch;
        private Button btnLogout;
        private Button btnAddParticipant;
        private Button btnNewEntry;

        public HomeController(IDictionary<string, string> properties, User currentUser)
        {
            this.properties = properties;
            this.currentUser = currentUser;
            InitializeComponents();
            InitializeServices();
        }

        private void InitializeComponents()
        {
            // Main form setup
            this.Text = "Swimming Contest";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Username Label
            usernameLabel = new Label
            {
                Text = $"User: {currentUser.UserName}",
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Event ComboBox
            eventComboBox = new ComboBox
            {
                Location = new Point(20, 50),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Search Button
            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(230, 50),
                Width = 80
            };
            btnSearch.Click += OnSearchClicked;

            // Search Message Label
            searchMessageLabel = new Label
            {
                Location = new Point(320, 55),
                AutoSize = true,
                Visible = false
            };

            // Event Table
            eventTable = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 400,
                Height = 150,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            eventTable.Columns.Add("Style", "Style");
            eventTable.Columns.Add("Distance", "Distance");
            eventTable.Columns.Add("ParticipantsCount", "Participants Count");

            // Participant Table
            participantTable = new DataGridView
            {
                Location = new Point(20, 270),
                Width = 400,
                Height = 150,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            participantTable.Columns.Add("Name", "Name");
            participantTable.Columns.Add("Age", "Age");

            // Search Results Container
            searchResultsContainer = new Panel
            {
                Location = new Point(450, 100),
                Size = new Size(300, 320),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            // Search Results Table
            searchResultsTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            searchResultsTable.Columns.Add("Name", "Name");
            searchResultsTable.Columns.Add("Age", "Age");
            searchResultsTable.Columns.Add("EventsCount", "Events Count");
            searchResultsContainer.Controls.Add(searchResultsTable);

            // Close Search Results Button
            btnCloseSearchResults = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom
            };
            btnCloseSearchResults.Click += OnCloseSearchResults;
            searchResultsContainer.Controls.Add(btnCloseSearchResults);

            // Action Buttons
            btnAddParticipant = new Button
            {
                Text = "Add Participant",
                Location = new Point(20, 450),
                Width = 120
            };
            btnAddParticipant.Click += OnParticipantButtonClicked;

            btnNewEntry = new Button
            {
                Text = "New Event Entry",
                Location = new Point(150, 450),
                Width = 120
            };
            btnNewEntry.Click += OnNewEntryClicked;

            btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(700, 20),
                Width = 80
            };
            btnLogout.Click += OnLogoutClicked;

            // Add controls to form
            this.Controls.Add(usernameLabel);
            this.Controls.Add(eventComboBox);
            this.Controls.Add(btnSearch);
            this.Controls.Add(searchMessageLabel);
            this.Controls.Add(eventTable);
            this.Controls.Add(participantTable);
            this.Controls.Add(searchResultsContainer);
            this.Controls.Add(btnAddParticipant);
            this.Controls.Add(btnNewEntry);
            this.Controls.Add(btnLogout);
        }

        private void InitializeServices()
        {
            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            participantService = new ParticipantService(participantRepository);
            
            EventDBRepository eventRepository = new EventDBRepository(properties);
            OfficeDBRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);
            eventService = new EventService(eventRepository, officeRepository);

            LoadEventComboBox();
            LoadParticipants();
            LoadEvents();
        }

        private void LoadEventComboBox()
        {
            eventComboBox.Items.Clear();
            var events = eventService.getAll();
            if (events == null || !events.Any())
            {
                MessageBox.Show("No events found.");
                return;
            }

            foreach (var ev in events)
            {
                eventComboBox.Items.Add(ev);
            }
        }


        private void LoadParticipants()
        {
            participantTable.Rows.Clear();
            foreach (var participant in participantService.getAll())
            {
                participantTable.Rows.Add(participant.Name, participant.Age);
            }
        }

        private void LoadEvents()
        {
            eventTable.Rows.Clear();
            foreach (var ev in eventService.getEventsWithParticipantsCount())
            {
                eventTable.Rows.Add(ev.style, ev.distance, ev.participantsCount);
            }
        }
        
        public void Init(IDictionary<string, string> properties, User currentUser)
        {
            this.properties = properties;
            this.currentUser = currentUser;
            LoadEvents();
            LoadParticipants();
        }


        private void OnSearchClicked(object sender, EventArgs e)
        {
            var selectedEvent = eventComboBox.SelectedItem as Event;
            if (selectedEvent == null)
            {
                ShowSearchMessage("Please select an event first!", true);
                return;
            }

            try
            {
                var results = eventService.getParticipantsForEventWithCount(selectedEvent.Id);
                if (results == null || !results.Any())
                {
                    ShowSearchMessage("No participants found for this event.", true);
                    searchResultsContainer.Visible = false;
                }
                else
                {
                    UpdateSearchResults(results);
                    searchResultsContainer.Visible = true;
                }
            }
            catch (EntityRepoException ex)
            {
                ShowAlert("Search Error", ex.Message);
                searchResultsContainer.Visible = false;
            }
        }

        private void UpdateSearchResults(IEnumerable<ParticipantDTO> results)
        {
            searchResultsTable.Rows.Clear();
            foreach (var result in results)
            {
                searchResultsTable.Rows.Add(result.name, result.age, result.eventCount);
            }
        }

        private void ShowSearchMessage(string message, bool isError)
        {
            searchMessageLabel.Text = message;
            searchMessageLabel.ForeColor = isError ? Color.DarkRed : Color.DarkGreen;
            searchMessageLabel.Visible = true;
        }

        private void OnCloseSearchResults(object sender, EventArgs e)
        {
            searchResultsContainer.Visible = false;
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var loginForm = new MainController(properties);
            loginForm.Show();
            this.Close();
        }

        private void OnParticipantButtonClicked(object sender, EventArgs e)
        {
            var newParticipantForm = new NewParticipantController(properties);
            if (newParticipantForm.ShowDialog() == DialogResult.OK)
            {
                LoadParticipants();
            }
        }

        private void OnNewEntryClicked(object sender, EventArgs e)
        {
            using (var eventEntriesForm = new EventEntriesController(properties, currentUser))
            {
                if (eventEntriesForm.ShowDialog() == DialogResult.OK)
                {
                    LoadEvents();
                }
            }
        }

        private void ShowAlert(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}