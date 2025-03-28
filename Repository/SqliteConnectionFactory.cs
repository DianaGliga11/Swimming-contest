using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class SqliteConnectionFactory: ConnectionFactory
{
    public override IDbConnection CreateConnection(IDictionary<string, string> props)
    {
        var connectionString = props["ConnectionString"];
        Console.WriteLine("SQLite ---Opening connection at ... {0}", connectionString);
        return new SqliteConnection(connectionString);
    }
}