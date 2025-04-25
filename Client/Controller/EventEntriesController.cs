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

        private readonly IContestServices proxy;
        private readonly List<Event> allEvents;
        private List<Participant> allParticipants;

    

        public EventEntriesController(IContestServices proxy, Participant participant, List<Event> events, List<Participant> allParticipants)
        {
            InitializeComponents();
            this.proxy = proxy;
            this.currentParticipant = participant;
            this.allEvents = events;
            this.allParticipants = allParticipants;
            LoadData();
        }


        /*
        private void InitializeServices()
        {
            EventDBRepository eventRepository = new EventDBRepository(properties);
            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            OfficeDBRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);
            
            eventService = new EventService(eventRepository, officeRepository);
            participantService = new ParticipantService(participantRepository);
        }
        */

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
                //DisplayMember = "Style" 
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
                // Actualizează UI-ul doar pe thread-ul principal
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        participantBox.DataSource = allParticipants;
                        participantBox.SelectedItem = allParticipants
                            .FirstOrDefault(p => p.Name == currentParticipant.Name && p.Age == currentParticipant.Age);

                        eventListView.Items.Clear();
                        foreach (var ev in allEvents)
                        {
                            eventListView.Items.Add(ev);
                        }
                    }));
                }
                else
                {
                    participantBox.DataSource = allParticipants;
                    participantBox.SelectedItem = allParticipants
                        .FirstOrDefault(p => p.Name == currentParticipant.Name && p.Age == currentParticipant.Age);

                    eventListView.Items.Clear();
                    foreach (var ev in allEvents)
                    {
                        eventListView.Items.Add(ev);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void OnParticipantSelected(object sender, EventArgs e)
        {
            currentParticipant = participantBox.SelectedItem as Participant;
        }
        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                if (currentParticipant == null || currentParticipant.Id <= 0)
                {
                    MessageBox.Show("Please select a valid participant first");
                    return;
                }

                var selectedEvents = eventListView.CheckedItems
                    .Cast<Event>()
                    .Where(ev => ev.Id > 0)
                    .ToList();

                if (!selectedEvents.Any())
                {
                    MessageBox.Show("Please select at least one valid event");
                    return;
                }

                var entries = selectedEvents
                    .Select(ev => new Office(currentParticipant, ev))
                    .ToList();

                // Execută pe un thread separat
                await Task.Run(() => proxy.saveEventsEntries(entries));

                // După ce datele au fost salvate, actualizează datele în interfață
                LoadData(); // Actualizează UI-ul cu noile date

                // Dacă nu a aruncat excepții, închidem fereastra
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}");
            }
        }

    }
}