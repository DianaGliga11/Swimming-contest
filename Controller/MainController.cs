using System;
using System.Windows.Forms;
using System.Drawing;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace SwimmingCompetitionController
{
    public class MainController : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox usernameTextField;
        private TextBox passwordTextField;
        private Button loginButton;
        private Label errorLabel;

        private UserService userService;
        private IDictionary<string, string> properties;

        public MainController(IDictionary<string, string> props)
        {
            this.properties = props;
            InitializeComponents();
            InitializeServices();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "Login - Swimming Competition";
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Title Label
            label1 = new Label
            {
                Text = "SWIMMING COMPETITION",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 50)
            };

            // Username Label
            label2 = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 120),
                AutoSize = true
            };

            // Username TextField
            usernameTextField = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 150),
                Size = new Size(200, 30),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Password Label
            label3 = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 190),
                AutoSize = true
            };

            // Password TextField
            passwordTextField = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 220),
                Size = new Size(200, 30),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*'
            };

            // Login Button
            loginButton = new Button
            {
                Text = "LOGIN",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                Location = new Point(150, 270),
                Size = new Size(200, 35),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += OnLoginButtonClick;

            // Error Label
            errorLabel = new Label
            {
                ForeColor = Color.Red,
                Location = new Point(150, 320),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add controls to form
            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(usernameTextField);
            this.Controls.Add(passwordTextField);
            this.Controls.Add(loginButton);
            this.Controls.Add(errorLabel);
        }

        private void InitializeServices()
        {
            UserDBRepository userRepository = new UserDBRepository(properties);
            userService = new UserService(userRepository);
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            string username = usernameTextField.Text;
            string password = passwordTextField.Text;

            try
            {
                User user = userService.getLogin(username, password);
                if (user != null)
                {
                    // Close current form and open main form (HomeController)
                    this.Hide();
                    var homeController = new HomeController(properties, user);
                    homeController.Show();
                }
                else
                {
                    errorLabel.Text = "Invalid username or password";
                }
            }
            catch (EntityRepoException ex)
            {
                errorLabel.Text = "Database error: " + ex.Message;
            }
        }
    }
}