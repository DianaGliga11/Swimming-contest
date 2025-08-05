using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Model.DTO;
using Service;
using log4net;


namespace Controller
{
    public class HomeController : Form, IMainObserver
    {
        //private IDictionary<string, string> properties;
        private User _currentUser;
        private readonly IContestServices server;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainController));

        private ComboBox _eventComboBox;
        private Label _usernameLabel;
        private Label _searchMessageLabel;
        private DataGridView _eventTable;
        private DataGridView _participantTable;
        private DataGridView _searchResultsTable;
        private Panel _searchResultsContainer;
        private Button _btnCloseSearchResults;
        private Button _btnSearch;
        private Button _btnLogout;
        private Button _btnAddParticipant;
        private Button _btnNewEntry;
        
        public HomeController(IContestServices server)
        {
            this.server = server;
        }


        private void InitializeComponents()
        {
            this.Text = "Swimming Contest";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            _usernameLabel = new Label
            {
                Text = $"User: {_currentUser.UserName}",
                Location = new Point(20, 20),
                AutoSize = true
            };

            _eventComboBox = new ComboBox
            {
                Location = new Point(20, 50),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(280, 50),
                Width = 80,
                Height = 30
            };
            _btnSearch.Click += OnSearchClicked;

            _searchMessageLabel = new Label
            {
                Location = new Point(390, 55),
                AutoSize = true,
                Visible = false
            };

            _eventTable = new DataGridView
            {
                Location = new Point(20, 90),
                Width = 450,
                Height = 200,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            _eventTable.Columns.Add("Style", "Style");
            _eventTable.Columns.Add("Distance", "Distance");
            _eventTable.Columns.Add("ParticipantsCount", "Participants Count");

            _participantTable = new DataGridView
            {
                Location = new Point(20, 310),
                Width = 450,
                Height = 200,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            _participantTable.Columns.Add("Name", "Name");
            _participantTable.Columns.Add("Age", "Age");

            _searchResultsContainer = new Panel
            {
                Location = new Point(500, 90),
                Size = new Size(450, 200),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            _searchResultsTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            _searchResultsTable.Columns.Add("Name", "Name");
            _searchResultsTable.Columns.Add("Age", "Age");
            _searchResultsTable.Columns.Add("EventsCount", "Events Count");
            _searchResultsContainer.Controls.Add(_searchResultsTable);

            _btnCloseSearchResults = new Button
            {
                Text = "Close",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            _btnCloseSearchResults.Click += OnCloseSearchResults;
            _searchResultsContainer.Controls.Add(_btnCloseSearchResults);

            _btnAddParticipant = new Button
            {
                Text = "Add Participant",
                Location = new Point(20, 530),
                Width = 150,
                Height = 30
            };
            _btnAddParticipant.Click += OnParticipantButtonClicked;

            _btnNewEntry = new Button
            {
                Text = "New Event Entry",
                Location = new Point(180, 530),
                Width = 150,
                Height = 30
            };
            _btnNewEntry.Click += OnNewEntryClicked;

            _btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(800, 20),
                Width = 100,
                Height = 30
            };
            _btnLogout.Click += OnLogoutClicked;

            this.Controls.Add(_usernameLabel);
            this.Controls.Add(_eventComboBox);
            this.Controls.Add(_btnSearch);
            this.Controls.Add(_searchMessageLabel);
            this.Controls.Add(_eventTable);
            this.Controls.Add(_participantTable);
            this.Controls.Add(_searchResultsContainer);
            this.Controls.Add(_btnAddParticipant);
            this.Controls.Add(_btnNewEntry);
            this.Controls.Add(_btnLogout);
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

                _eventComboBox.DataSource = events.ToList();
                _eventComboBox.SelectedIndex = -1;

            });
        }


        public void UpdateUI(Action action)
        {
            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private void LoadParticipants()
        {
            UpdateUI(() =>
            {
                _participantTable.Rows.Clear();
                foreach (var participant in server.GetAllParticipants())
                {
                    Log.Debug($"Loading participant: {participant.Name}");
                    _participantTable.Rows.Add(participant.Name, participant.Age, participant.Id);
                }
            });
        }

        private void LoadEvents()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadEvents));
                return;
            }

            _eventTable.Rows.Clear();
            var events = server.GetEventsWithParticipantsCount();
            foreach (var ev in events)
            {
                _eventTable.Rows.Add(ev.style, ev.distance, ev.participantsCount);
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            var selectedEvent = _eventComboBox.SelectedItem as Event;
            Log.Info($"Selected item: {selectedEvent}");
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
                    _searchResultsContainer.Visible = false;
                }
                else
                {
                    UpdateSearchResults(results);
                    _searchResultsContainer.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Search Error", ex.Message);
                _searchResultsContainer.Visible = false;
            }
        }

        private void UpdateSearchResults(IEnumerable<ParticipantDTO> results)
        {
            _searchResultsTable.Rows.Clear();
            foreach (var result in results)
            {
                _searchResultsTable.Rows.Add(result.name, result.age, result.eventCount);
            }
        }
        
        private void ShowSearchMessage(string message, bool isError)
        {
            _searchMessageLabel.Text = message;
            _searchMessageLabel.ForeColor = isError ? Color.DarkRed : Color.DarkGreen;
            _searchMessageLabel.Visible = true;
        }

        private void OnCloseSearchResults(object sender, EventArgs e)
        {
            _searchResultsContainer.Visible = false;
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var loginForm = new MainController(server);
            loginForm.Show();
            this.Close();
        }

        private void OnParticipantButtonClicked(object sender, EventArgs e)
        {
            var allEvents = server.GetAllEvents();
            var allParticipants = server.GetAllParticipants();
            using (var newParticipantForm = new NewParticipantController(server, this, null, allEvents, allParticipants))
            {
                if (newParticipantForm.ShowDialog() == DialogResult.OK)
                {
                    LoadParticipants();
                }

            }
        }

        private void OnNewEntryClicked(object sender, EventArgs e)
        {
            var allEvents = server.GetAllEvents();
            var allParticipants = server.GetAllParticipants();
            using (var eventEntriesForm = new EventEntriesController(server, this, null, allEvents, allParticipants))
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

        public void ParticipantAdded(Participant participant)
        {
            UpdateUI(() =>
            {
                _participantTable.Rows.Add(participant.Name, participant.Age); 
            });
        }

        public void EventEvntriesAdded(List<EventDTO> events)
        {
            UpdateUI(() => {
                _eventTable.Rows.Clear();
                foreach (var ev in events)
                {
                    _eventTable.Rows.Add(ev.style, ev.distance, ev.participantsCount);
                }
            });
        }

        public void SetLoggedInUser(User user)
        {
            this._currentUser = user;
            InitializeComponents();  

            _usernameLabel.Text = $"Logged in as: {_currentUser.UserName}";
            LoadEventComboBox();
            LoadParticipants();
            LoadEvents();
        }
    }
}
