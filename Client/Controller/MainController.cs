using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;
using Service;
using System.Drawing;
using System.Windows.Forms;
using Networking.Request;
using Service;

namespace Controller
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

        private static readonly ILog log = LogManager.GetLogger(typeof(MainController));
        private readonly IContestServices server;
        private readonly IDictionary<string, string> properties;

        // Constructor
        public MainController(IDictionary<string, string> props, IContestServices server)
        {
            this.properties = props;
            this.server = server;
            InitializeComponents();
            this.FormClosing += MainController_FormClosing;
        }

        // Initializarea componentelor formularului
        private void InitializeComponents()
        {
            this.Text = "Login - Swimming Competition";
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            label1 = new Label
            {
                Text = "SWIMMING COMPETITION",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 50)
            };

            label2 = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 120),
                AutoSize = true
            };

            usernameTextField = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 150),
                Size = new Size(200, 30),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle
            };

            label3 = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 190),
                AutoSize = true
            };

            passwordTextField = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 220),
                Size = new Size(200, 30),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*'
            };

            loginButton = new Button
            {
                Text = "LOGIN",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(150, 270),
                Size = new Size(200, 35),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += OnLoginButtonClick;

            errorLabel = new Label
            {
                ForeColor = Color.Red,
                Location = new Point(150, 320),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(usernameTextField);
            this.Controls.Add(passwordTextField);
            this.Controls.Add(loginButton);
            this.Controls.Add(errorLabel);
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            log.Debug("OnLoginButtonClick...");
            string username = usernameTextField.Text.Trim();
            string password = passwordTextField.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                errorLabel.Text = "Username and password cannot be empty.";
                log.Warn("Login attempt with empty username or password.");
                return;
            }

            try
            {
                log.Debug($"Login attempt with username: {username}");

                var request = new LoginRequest(username, password);
                // Cream deja instanța de HomeController ca observer
                var homeController = new HomeController(properties, null, server);
                log.Debug($"LoginRequest created - Username: {request.Username}, Password: [PROTECTED]");
                // Încercăm autentificarea și trimitem observer-ul (clientul)
                User user = server.Login(username, password, homeController);

                if (user != null)
                {
                    log.Info($"Login successful for user: {user.UserName}");

                    // Acum setăm utilizatorul în controller după login
                    homeController.SetLoggedInUser(user);

                    this.Hide();
                    homeController.Show();
                }
                else
                {
                    errorLabel.Text = "Invalid username or password.";
                    log.Warn("Invalid login credentials.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Login error", ex);
                errorLabel.Text = "Error: " + ex.Message;
            }
        }

        private void MainController_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Info("App is closing...");
            LogManager.Shutdown();
            Application.Exit();
        }
    }
}
