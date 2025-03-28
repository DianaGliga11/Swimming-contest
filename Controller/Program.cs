using System;
using System.Windows.Forms;
using DefaultNamespace.Properties;
using SwimmingCompetitionController;

static class Program
{
    [STAThread]
    static void Main()
    {
        IDictionary<string, string> properties = Config.DatabaseProperties;
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainController(properties));
    }
}