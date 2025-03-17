using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Data.Sqlite;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
    public class DbConnectionUtils
    {
        private static IDbConnection instance = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(DbConnectionUtils));


        public static IDbConnection getConnection(IDictionary<string, string> props)
        {
            string connectionString = props["ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                log.Error("Connection string is null or empty");
            }
            IDbConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        private static Type GetType(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(name);
                if (type != null)
                    return type;

            }

            return null;
        }

        private static IDbConnection getNewConnection(IDictionary<string, string> props)
        {
            Type connectionType = GetType(props["ConnectionType"]);

            if (connectionType == null)
            {
                throw new ArgumentException("Invalid connection type");
            }

            string connectionString = props["ConnectionString"];
            Console.WriteLine($"Se deschide o conexiune {connectionType} la  ... {connectionString}");
            return Activator.CreateInstance(connectionType, new object[] { connectionString })
                as IDbConnection;
        }

        static DbConnectionUtils()
        {
            MakeAssembliesVisible();
        }

        private static void MakeAssembliesVisible()
        {
            IDbConnection con = new SqliteConnection();
            Console.WriteLine(con.ToString().Substring(0, 0));
        }

    }
}