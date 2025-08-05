using Microsoft.EntityFrameworkCore.Diagnostics;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace Controller
{
    public class NewParticipantController : Form
    {
        //private ParticipantService participantService;
        //private Action onParticipantAdded;
        //private IDictionary<string,string> properties;
        private User currentUser;
        private Participant participant;
        private List<Participant> participants;
        private List<Event> events;
        private readonly IMainObserver observer;

        private TextBox nameTextField;
        private TextBox ageTextField;
        private Button confirmButton;

        private readonly IContestServices proxy;

        public NewParticipantController(IContestServices proxy, IMainObserver observer, Participant participant, List<Event> events, List<Participant> participants)
        {
            InitializeComponents();
            this.proxy = proxy;
            this.participant = participant;
            this.events = events;
            this.participants = participants;
            this.observer = observer;
            LoadData();
        }

        private void InitializeComponents()
        {
            this.Text = "Add New Participant";
            this.Size = new System.Drawing.Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            Label nameLabel = new Label
            {
                Text = "Name:",
                Left = 20,
                Top = 20,
                Width = 80
            };

            nameTextField = new TextBox
            {
                Left = 110,
                Top = 20,
                Width = 150
            };

            Label ageLabel = new Label
            {
                Text = "Age:",
                Left = 20,
                Top = 50,
                Width = 80
            };

            ageTextField = new TextBox
            {
                Left = 110,
                Top = 50,
                Width = 150
            };

            confirmButton = new Button
            {
                Text = "Confirm",
                Left = 110,
                Top = 90,
                Width = 80
            };
            confirmButton.Click += OnConfirmClicked;

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextField);
            this.Controls.Add(ageLabel);
            this.Controls.Add(ageTextField);
            this.Controls.Add(confirmButton);
        }
        
        private void LoadData()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(LoadData));
                    return;
                }

                if (participant != null)
                {
                    nameTextField.Text = participant.Name;
                    ageTextField.Text = participant.Age.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading participant data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            string name = nameTextField.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowAlert("Error", "Name cannot be empty!");
                return;
            }

            if (!int.TryParse(ageTextField.Text, out int age))
            {
                ShowAlert("Error", "Please enter a valid age!");
                return;
            }

            Participant participant = new Participant(name, age);

            try
            {
                await Task.Run(() => proxy.SaveParticipant(participant, observer));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowAlert("Error adding participant", ex.Message);
            }
        }


        private void ShowAlert(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}