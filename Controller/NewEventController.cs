using System;
using System.Windows.Forms;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace SwimmingCompetitionController
{
    public class NewEventController : Form
    {
        private EventService eventService;
        private IDictionary<string,string> properties;
        private User currentUser;
        
        private TextBox styleField;
        private TextBox distanceField;
        private Button btnConfirm;
        
        public NewEventController()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "Add New Event";
            this.Size = new System.Drawing.Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Style Label
            Label styleLabel = new Label
            {
                Text = "Style:",
                Left = 20,
                Top = 20,
                Width = 80
            };

            // Style TextField
            styleField = new TextBox
            {
                Left = 110,
                Top = 20,
                Width = 150
            };

            // Distance Label
            Label distanceLabel = new Label
            {
                Text = "Distance:",
                Left = 20,
                Top = 50,
                Width = 80
            };

            // Distance TextField
            distanceField = new TextBox
            {
                Left = 110,
                Top = 50,
                Width = 150
            };

            // Confirm Button
            btnConfirm = new Button
            {
                Text = "Confirm",
                Left = 110,
                Top = 90,
                Width = 80
            };
            btnConfirm.Click += OnConfirmClicked;

            // Add controls to form
            this.Controls.Add(styleLabel);
            this.Controls.Add(styleField);
            this.Controls.Add(distanceLabel);
            this.Controls.Add(distanceField);
            this.Controls.Add(btnConfirm);
        }

        public void Init(IDictionary<string,string> properties, User currentUser)
        {
            this.properties = properties;
            this.currentUser = currentUser;
            ParticipantDBRepository participantRepository = new ParticipantDBRepository(properties);
            EventDBRepository eventRepository = new EventDBRepository(properties);
            OfficeDBRepository officeRepository = new OfficeDBRepository(properties,participantRepository , eventRepository);
            eventService = new EventService(eventRepository,officeRepository);
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                string style = styleField.Text;
                if (string.IsNullOrWhiteSpace(style))
                {
                    ShowAlert("Validation Error", "Please enter a style");
                    return;
                }

                if (!int.TryParse(distanceField.Text, out int distance))
                {
                    ShowAlert("Validation Error", "Please enter a valid distance (number)");
                    return;
                }

                eventService.add(new Event(style, distance));
                
                // Close this form and return to home
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (EntityRepoException ex)
            {
                ShowAlert("Database Error", ex.Message);
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