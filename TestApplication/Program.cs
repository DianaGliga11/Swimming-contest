using System;
using System.Collections.Generic;
using System.Data;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace DefaultNamespace;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

class Program
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    static void Main(string[] args)
    {
        
       /*ar logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        log.Info("App started");
        var eventRepository = new EventDBRepository(Config.DatabaseProperties);
        var participantRepository = new ParticipantDBRepository(Config.DatabaseProperties);
        var userRepository = new UserDBRepository(Config.DatabaseProperties);
        var officeRepository = new OfficeDBRepository(Config.DatabaseProperties,participantRepository , eventRepository);
      
        //eventRepository.Add(new Event("mixt",1500));
        //eventRepository.Add(new Event("fluture",150));
        foreach (var ev in eventRepository.getAll())
        {
            Console.WriteLine(ev.ToString());
        }
        */
    }

}
