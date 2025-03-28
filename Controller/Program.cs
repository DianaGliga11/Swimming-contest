using System;
using System.Windows.Forms;
using SwimmingCompetitionController;

static class Program
{
    [STAThread]
    static void Main()
    {
        IDictionary<string, string> properties = new Dictionary<string, string>
        {
            { "ConnectionString", "Data Source=SwimingContest.db" }
        };
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainController(properties));
    }
}