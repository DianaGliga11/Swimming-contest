using System;
using System.Windows.Forms;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace SwimmingCompetitionController
{
    public class NewParticipantController : Form
    {
        private ParticipantService participantService;
        private Action onParticipantAdded;
        private IDictionary<string,string> properties;
        private User currentUser;

        private TextBox nameTextField;
        private TextBox ageTextField;
        private Button confirmButton;

        public NewParticipantController(IDictionary<string,string> properties)
        {
            this.properties = properties;
            Init(this.properties,currentUser,onParticipantAdded);
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "Add New Participant";
            this.Size = new System.Drawing.Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Name Label
            Label nameLabel = new Label
            {
                Text = "Name:",
                Left = 20,
                Top = 20,
                Width = 80
            };

            // Name TextField
            nameTextField = new TextBox
            {
                Left = 110,
                Top = 20,
                Width = 150
            };

            // Age Label
            Label ageLabel = new Label
            {
                Text = "Age:",
                Left = 20,
                Top = 50,
                Width = 80
            };

            // Age TextField
            ageTextField = new TextBox
            {
                Left = 110,
                Top = 50,
                Width = 150
            };

            // Confirm Button
            confirmButton = new Button
            {
                Text = "Confirm",
                Left = 110,
                Top = 90,
                Width = 80
            };
            confirmButton.Click += OnConfirmClicked;

            // Add controls to form
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextField);
            this.Controls.Add(ageLabel);
            this.Controls.Add(ageTextField);
            this.Controls.Add(confirmButton);
        }

        public void Init(IDictionary<string,string> properties, User currentUser, Action onParticipantAddedCallback)
        {
            this.properties = properties;
            this.currentUser = currentUser;
            this.onParticipantAdded = onParticipantAddedCallback;

            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            this.participantService = new ParticipantService(participantRepository);

            InitializeComponents();
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            try
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
                participantService.add(participant);

                // Execute the callback to notify the parent form
                onParticipantAdded?.Invoke();

                // Close the form
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