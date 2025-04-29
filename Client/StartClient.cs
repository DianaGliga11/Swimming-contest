using Controller;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic.Logging;
using Networking;
using Service;

namespace Client;
public class StartClient
{
    private static readonly ILog log = LogManager.GetLogger(typeof(StartClient));
    [STAThread]
    private static void Main()
    {
        log.Info("Client started...");
        ApplicationConfiguration.Initialize();
        //IDictionary<string, string> properties = new SortedList<string, string>();
        XmlConfigurator.Configure(new FileInfo("client.config"));

        IContestServices server = new ServicesProxy("127.0.0.1", 56789);
        MainController controller = new MainController( server);
        Application.Run(controller);
    }
}