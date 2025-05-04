using System.Net;
using System.Net.Sockets;
namespace Server;
using log4net;

public abstract class IServer
{
    private readonly TcpListener _server;
    private readonly int _port;
    private readonly string _host;
    private static readonly ILog Log = LogManager.GetLogger(typeof(IServer));

    public IServer(int port, string host)
    {
        this._port = port;
        this._host = host;
        IPAddress adr = IPAddress.Parse(host);
        IPEndPoint endPoint = new IPEndPoint(adr, port);
        _server = new TcpListener(endPoint);
    }

    public void Start()
    {
        _server.Start();
        while (true)
        {
            Log.Info("Waiting for clients...");
            TcpClient client = _server.AcceptTcpClient();
            Log.Info("Client connected...");
            ProcessRequest(client);
        }
    }
    
    public abstract void ProcessRequest(TcpClient client);
}