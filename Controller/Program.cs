using System;
using System.Windows.Forms;
using Controller;
using DefaultNamespace.Properties;
using log4net;

static class Program
{
    [STAThread]
    static void Main()
    {
        log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Application.ThreadException += Application_ThreadException;

        IDictionary<string, string> properties = Config.DatabaseProperties;
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainController(properties));
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;
        LogManager.GetLogger(typeof(Program)).Fatal("Error in (AppDomain)", ex);
        MessageBox.Show($"Critical Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        LogManager.GetLogger(typeof(Program)).Fatal("Error in (UI Thread)", e.Exception);
        MessageBox.Show($"Error: {e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
    }
}