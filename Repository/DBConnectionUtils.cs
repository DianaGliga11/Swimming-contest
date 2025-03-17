using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
    public class DbConnectionUtils
    {
        private static IDbConnection instance = null;

        public static IDbConnection getConnection(IDictionary<string, string> props)
        {
            
            if (instance == null || instance.State == ConnectionState.Closed)
            {
                instance = getNewConnection(props);
                instance.Open();
            }

            return instance;
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
            string connectionString = props["ConnectionString"];
            return new SqliteConnection(connectionString);
        }


        private static void MakeAssembliesVisible()
        {
            IDbConnection con = new SqliteConnection();
            Console.WriteLine(con.ToString().Substring(0, 0));
        }

    }
}