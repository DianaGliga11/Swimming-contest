using Microsoft.VisualBasic.Logging;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using Service;
using log4net;


namespace Controller
{
    public class HomeController : Form, IMainObserver
    {
        private IDictionary<string, string> properties;
        private User currentUser;
        private IContestServices server;
        private static readonly ILog log = LogManager.GetLogger(typeof(MainController));

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

        public HomeController(IDictionary<string, string> properties, User currentUser, IContestServices server)
        {
            this.properties = properties;
            this.server = server;

            if (currentUser != null)
            {
                SetLoggedInUser(currentUser);
            }
        }

        private void InitializeComponents()
        {
            this.Text = "Swimming Contest";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            usernameLabel = new Label
            {
                Text = $"User: {currentUser.UserName}",
                Location = new Point(20, 20),
                AutoSize = true
            };

            eventComboBox = new ComboBox
            {
                Location = new Point(20, 50),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(230, 50),
                Width = 80
            };
            btnSearch.Click += OnSearchClicked;

            searchMessageLabel = new Label
            {
                Location = new Point(320, 55),
                AutoSize = true,
                Visible = false
            };

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

            searchResultsContainer = new Panel
            {
                Location = new Point(450, 100),
                Size = new Size(300, 320),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

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

            btnCloseSearchResults = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom
            };
            btnCloseSearchResults.Click += OnCloseSearchResults;
            searchResultsContainer.Controls.Add(btnCloseSearchResults);

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

        private void LoadEventComboBox()
        {
            UpdateUI(() =>
            {
                var events = server.GetAllEvents();
                if (events == null || !events.Any())
                {
                    MessageBox.Show("No events found.");
                    return;
                }

                eventComboBox.DataSource = events.ToList();
                eventComboBox.SelectedIndex = -1;

            });
        }


        public void UpdateUI(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }
        private void LoadParticipants()
        {
            UpdateUI(() => {
                participantTable.Rows.Clear();
                foreach (var participant in server.GetAllParticipants())
                {
                    log.Debug($"Loading participant: {participant.Name}");
                    participantTable.Rows.Add(participant.Name, participant.Age, participant.Id);
                }
            });
        }

        private void LoadEvents()
        {
            UpdateUI(() => {
                eventTable.Rows.Clear();
                foreach (var ev in server.GetEventsWithParticipantsCount())
                {
                    eventTable.Rows.Add(ev.style, ev.distance, ev.participantsCount);
                }
            });
        }

        
        private void OnSearchClicked(object sender, EventArgs e)
        {
            var selectedEvent = eventComboBox.SelectedItem as Event;
            log.Info($"Selected item: {selectedEvent}");
            if (selectedEvent == null)
            {
                ShowSearchMessage("Please select an event first!", true);
                return;
            }

            try
            {
                var results = server.GetParticipantsForEventWithCount(selectedEvent.Id);
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
            catch (Exception ex)
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
            var loginForm = new MainController(properties, server);
            loginForm.Show();
            this.Close();
        }

        private void OnParticipantButtonClicked(object sender, EventArgs e)
        {
            var newParticipantForm = new NewParticipantController(server);
            if (newParticipantForm.ShowDialog() == DialogResult.OK)
            {
                LoadParticipants();
            }
        }

        private void OnNewEntryClicked(object sender, EventArgs e)
        {
            var allEvents = server.GetAllEvents();
            var allParticipants = server.GetAllParticipants();

            using (var eventEntriesForm = new EventEntriesController(server, null, allEvents, allParticipants))
            {
                if (eventEntriesForm.ShowDialog() == DialogResult.OK)
                {
                    LoadEvents(); // Reload event table to reflect updated participant counts
                }
            }
        }

        private void ShowAlert(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ParticipantAdded(Participant participant)
        {
            UpdateUI(() => {
                participantTable.Rows.Add(participant.Name, participant.Age);
                MessageBox.Show($"Participant {participant.Name} added successfully!");
            });
        }

        public void EventEvntriesAdded(List<EventDTO> events)
        {
            UpdateUI(() => {
                LoadEvents();
                MessageBox.Show("Event entries updated successfully!");
            });
        }
        public void SetLoggedInUser(User user)
        {
            this.currentUser = user;
            InitializeComponents();
            LoadEventComboBox();
            LoadParticipants();
            LoadEvents();
            
        }
    }
}
