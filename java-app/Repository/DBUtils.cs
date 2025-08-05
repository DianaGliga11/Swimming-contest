using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class DBUtils
{
    private static IDbConnection instance = null;

    public static IDbConnection GetConnection(IDictionary<string, string> props)
    {
            
        if (instance == null || instance.State == ConnectionState.Closed)
        {
            instance = GetNewConnection(props);
            instance.Open();
        }

        return instance;
    }
    
    private static IDbConnection GetNewConnection(IDictionary<string, string> props)
    {
        string connectionString = props["ConnectionString"];
        return new SqliteConnection(connectionString);
    }
}