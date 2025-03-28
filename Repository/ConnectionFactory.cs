using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public abstract class ConnectionFactory
{
    protected ConnectionFactory()
    {
        
    }

    private static ConnectionFactory? _connectionFactory;

    public static ConnectionFactory? GetInstance()
    {
        if (_connectionFactory != null) return _connectionFactory;
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(ConnectionFactory)))
            {
                _connectionFactory = (ConnectionFactory?)Activator.CreateInstance(type);
            }
        }

        return _connectionFactory;
    }

    public abstract IDbConnection CreateConnection(IDictionary<string, string> props);
}