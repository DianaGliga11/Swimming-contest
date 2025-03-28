using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace SwimmingCompetitionController
{
    public class OfficeController : Form
    {
        private EventService eventService;
        private ParticipantService participantService;
        private IDictionary<string,string> properties;
        private Participant currentParticipant;
        private User currentUser;

        private ComboBox participantComboBox;
        private CheckedListBox eventListView;
        private Button confirmButton;

        public OfficeController()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "Office Registration";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Participant ComboBox
            participantComboBox = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            participantComboBox.SelectedIndexChanged += OnParticipantSelected;

            // Events ListView (using CheckedListBox for selection)
            eventListView = new CheckedListBox
            {
                Location = new Point(20, 60),
                Size = new Size(350, 350),
                CheckOnClick = true,
                //SelectionMode = SelectionMode.MultiExtended
            };

            // Confirm Button
            confirmButton = new Button
            {
                Text = "Confirm Registration",
                Location = new Point(120, 420),
                Size = new Size(160, 30)
            };
            confirmButton.Click += OnConfirmClicked;

            // Add controls to form
            this.Controls.Add(participantComboBox);
            this.Controls.Add(eventListView);
            this.Controls.Add(confirmButton);
        }

        // Changed to remove Stage parameter since it's not needed in Windows Forms
        public void Init(IDictionary<string,string> properties, User currentUser)
        {
            this.properties = properties;
            this.currentUser = currentUser;

            EventDBRepository eventRepository = new EventDBRepository(properties);
            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            OfficeDBRepository officeRepository = new OfficeDBRepository(properties, participantRepository, eventRepository);

            eventService = new EventService(eventRepository, officeRepository);
            participantService = new ParticipantService(participantRepository);

            LoadParticipants();
            LoadEvents();
        }

        private void LoadParticipants()
        {
            try
            {
                participantComboBox.Items.Clear();
                var participants = participantService.getAll();
                foreach (var participant in participants)
                {
                    participantComboBox.Items.Add(participant);
                }

                if (participantComboBox.Items.Count > 0)
                {
                    participantComboBox.SelectedIndex = 0;
                    currentParticipant = (Participant)participantComboBox.SelectedItem;
                }
            }
            catch (EntityRepoException ex)
            {
                ShowAlert("Error", ex.Message);
            }
        }

        private void LoadEvents()
        {
            try
            {
                eventListView.Items.Clear();
                var events = eventService.getAll();
                foreach (var ev in events)
                {
                    eventListView.Items.Add(ev);
                }
            }
            catch (EntityRepoException ex)
            {
                ShowAlert("Error", ex.Message);
            }
        }

        private void OnParticipantSelected(object sender, EventArgs e)
        {
            currentParticipant = (Participant)participantComboBox.SelectedItem;
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                if (currentParticipant == null || currentParticipant.Id <= 0)
                {
                    ShowAlert("Error", "Please select a valid participant first");
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
                    ShowAlert("Error", "Please select at least one valid event");
                    return;
                }

                foreach (var ev in selectedEvents)
                {
                    // Create new Office with participant and event objects
                    var registration = new Office(currentParticipant, ev);
                    eventService.saveEventEntry(registration);
                }

                ShowAlert("Success", $"Registered participant for {selectedEvents.Count} events");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowAlert("Error", ex.Message);
            }
        }

        private void ShowAlert(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}