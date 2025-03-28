using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace Controller
{
    public class EventEntriesController : Form
    {
        private I_EventService eventService;
        private I_ParticipantService participantService;
        private IDictionary<string, string> properties;
        private Participant currentParticipant;
        private User currentUser;

        private ComboBox participantBox;
        private CheckedListBox eventListView;
        private Button confirmButton;

        public EventEntriesController(IDictionary<string, string> properties, User currentUser)
        {
            this.properties = properties;
            this.currentUser = currentUser;
            InitializeServices();
            InitializeComponents();
            LoadData();
        }

        private void InitializeServices()
        {
            EventDBRepository eventRepository = new EventDBRepository(properties);
            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            OfficeDBRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);
            
            eventService = new EventService(eventRepository, officeRepository);
            participantService = new ParticipantService(participantRepository);
        }

        private void InitializeComponents()
        {
            this.Text = "Event Registration";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            Label participantLabel = new Label
            {
                Text = "Participant:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            participantBox = new ComboBox
            {
                Location = new Point(120, 20),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Name" 
            };
            participantBox.SelectedIndexChanged += OnParticipantSelected;

            Label eventsLabel = new Label
            {
                Text = "Available Events:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            eventListView = new CheckedListBox
            {
                Location = new Point(20, 90),
                Size = new Size(350, 300),
                CheckOnClick = true,
                DisplayMember = "Style" 
            };

            confirmButton = new Button
            {
                Text = "Confirm Registration",
                Location = new Point(120, 400),
                Size = new Size(150, 30)
            };
            confirmButton.Click += OnConfirmClicked;

            this.Controls.Add(participantLabel);
            this.Controls.Add(participantBox);
            this.Controls.Add(eventsLabel);
            this.Controls.Add(eventListView);
            this.Controls.Add(confirmButton);
        }

        private void LoadData()
        {
            try
            {
                var participants = participantService.getAll();
                participantBox.DataSource = participants;
                participantBox.SelectedIndex = participants.Any() ? 0 : -1;

                var events = eventService.getAll();
                eventListView.DataSource = events;
            }
            catch (EntityRepoException ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnParticipantSelected(object sender, EventArgs e)
        {
            currentParticipant = participantBox.SelectedItem as Participant;
        }
        private void OnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                if (currentParticipant == null || currentParticipant.Id <= 0)
                {
                    MessageBox.Show("Please select a valid participant first", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedEvents = new List<Event>();
                foreach (var item in eventListView.CheckedItems)
                {
                    if (item is Event ev && ev.Id > 0)
                    {
                        selectedEvents.Add(ev);
                    }
                }

                if (selectedEvents.Count == 0)
                {
                    MessageBox.Show("Please select at least one valid event", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var ev in selectedEvents)
                {
                    var registration = new Office(currentParticipant, ev);
                    eventService.saveEventEntry(registration);
                }

                MessageBox.Show($"Successfully registered for {selectedEvents.Count} events!", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}