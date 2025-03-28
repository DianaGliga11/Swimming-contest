using System.Collections.Generic;
using System.Configuration;

namespace DefaultNamespace.Properties
{
    public class Config
    {
        public static string GetConnectionStringByName(string name)
            => ConfigurationManager.ConnectionStrings[name]?.ConnectionString;

        public static string ConnectionType
            => ConfigurationManager.AppSettings.Get("ConnectionType");

        public static readonly IDictionary<string, string> DatabaseProperties = new Dictionary<string, string>
        {
            { "ConnectionString",  GetConnectionStringByName("SwimingContest") },
            { "ConnectionType", ConnectionType}
        };
    }
}