using log4net;
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
        //private readonly IDictionary<string, string> properties;

        public MainController(IContestServices server)
        {
            //this.properties = props;
            this.server = server;
            InitializeComponents();
            this.FormClosing += MainController_FormClosing;
        }

private void InitializeComponents()
{
    this.Text = "Login - Swimming Competition";
    this.Size = new Size(800, 500);  
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.StartPosition = FormStartPosition.CenterScreen;
    this.BackColor = Color.White;
    this.Padding = new Padding(20);  

    int centerX = (this.ClientSize.Width - 300) / 2;
    label1 = new Label
    {
        Text = "SWIMMING COMPETITION",
        Font = new Font("Segoe UI", 15F, FontStyle.Bold),
        AutoSize = true,
        Location = new Point(centerX, 80),
        ForeColor = Color.FromArgb(0, 84, 147)  
    };

    label1.Location = new Point((this.ClientSize.Width - label1.Width) / 2, 80);

    label2 = new Label
    {
        Text = "Username:",
        Font = new Font("Segoe UI", 11F),
        Location = new Point(centerX, 150),
        AutoSize = true,
        ForeColor = Color.FromArgb(64, 64, 64)  
    };

    usernameTextField = new TextBox
    {
        Font = new Font("Segoe UI", 11F),
        Location = new Point(centerX, 175),
        Size = new Size(300, 34),  
        BackColor = Color.White,
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(0, 0, 0, 20) 
    };

    label3 = new Label
    {
        Text = "Password:",
        Font = new Font("Segoe UI", 11F),
        Location = new Point(centerX, 235),
        AutoSize = true,
        ForeColor = Color.FromArgb(64, 64, 64)
    };

    passwordTextField = new TextBox
    {
        Font = new Font("Segoe UI", 11F),
        Location = new Point(centerX, 260),
        Size = new Size(300, 34),
        BackColor = Color.White,
        BorderStyle = BorderStyle.FixedSingle,
        PasswordChar = '•' 
    };

    loginButton = new Button
    {
        Text = "LOGIN",
        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
        Location = new Point(centerX, 330),
        Size = new Size(300, 40),
        BackColor = Color.FromArgb(0, 120, 215), 
        FlatStyle = FlatStyle.Flat,
        ForeColor = Color.White,
        Cursor = Cursors.Hand
    };
    loginButton.FlatAppearance.BorderSize = 0;
    loginButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 100, 190); 
    loginButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 80, 170);  
    loginButton.Click += OnLoginButtonClick;

    errorLabel = new Label
    {
        ForeColor = Color.FromArgb(200, 0, 0), 
        Location = new Point(centerX, 380),
        Size = new Size(300, 25),
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 9F)
    };

    this.Controls.Add(label1);
    this.Controls.Add(label2);
    this.Controls.Add(label3);
    this.Controls.Add(usernameTextField);
    this.Controls.Add(passwordTextField);
    this.Controls.Add(loginButton);
    this.Controls.Add(errorLabel);

    this.DoubleBuffered = true; 
}

        private async void OnLoginButtonClick(object sender, EventArgs e)
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

            loginButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                var homeController = new HomeController(server);
                var user = await Task.Run(() => server.Login(username, password, homeController));

                this.BeginInvoke((MethodInvoker)delegate {
                    if (user != null)
                    {
                        log.Info($"Login successful for user: {user.UserName}");
                        homeController.SetLoggedInUser(user);
                        homeController.Show();
                        this.Hide();
                    }
                    else
                    {
                        errorLabel.Text = "Invalid username or password.";
                        log.Warn("Invalid login credentials.");
                    }

                    loginButton.Enabled = true;
                    Cursor.Current = Cursors.Default;
                });
            }
            catch (Exception ex)
            {
                log.Error("Login error", ex);
                this.BeginInvoke((MethodInvoker)delegate {
                    errorLabel.Text = "Error: " + ex.Message;
                    loginButton.Enabled = true;
                    Cursor.Current = Cursors.Default;
                });
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
