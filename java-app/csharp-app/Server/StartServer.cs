using System.Configuration;
using log4net.Config;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Repository;
using Service;

namespace Server;

public class StartServer
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(IServer));

    private static void Main(string[] args)
    {
        
        IDictionary<string,string> props = new SortedList<string,string>();
        XmlConfigurator.Configure(new FileInfo("app.config"));
        string? connectionString = GetConnectionStringByName("SwimingContest");
        if (connectionString == null)
        {
            Log.Error("Null connection string");
            return;
        }
        props.Add("ConnectionString", connectionString);

        I_UserDBRepository userRepository = new UserDBRepository(props);
        I_EventDBRepository eventRepository = new EventDBRepository(props);
        I_ParticipantDBRepository participantRepository = new ParticipantDBRepository(props);
        I_OfficeDBRepository officeRepository = new OfficeDBRepository(props, participantRepository, eventRepository);

        I_UserService userService = new UserService(userRepository);
        I_EventService eventService = new EventService(eventRepository, officeRepository);
        I_ParticipantService participantService = new ParticipantService(participantRepository);

        IContestServices server = new ContestServices(userService, participantService, eventService);

        IServer scs = new ProtocolBuffersServer(56789, "127.0.0.1", server);
        scs.Start();
        Log.Info("Server started...");
        Console.ReadKey();
    }

    private static string? GetConnectionStringByName(string name)
    {
        string? connectionString = null;
        var stringSettings = ConfigurationManager.ConnectionStrings[name];
        if (stringSettings != null)
        {
            connectionString = stringSettings.ConnectionString;
        }
        return connectionString;
    }
}