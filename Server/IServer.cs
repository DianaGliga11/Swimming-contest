using System.Net;
using System.Net.Sockets;
namespace Server;
using log4net;

public abstract class IServer
{
    private readonly TcpListener server;
    private readonly int port;
    private readonly string host;
    private static readonly ILog log = LogManager.GetLogger(typeof(IServer));

    public IServer(int port, string host)
    {
        this.port = port;
        this.host = host;
        IPAddress adr = IPAddress.Parse(host);
        IPEndPoint endPoint = new IPEndPoint(adr, port);
        server = new TcpListener(endPoint);
    }

    public void Start()
    {
        server.Start();
        while (true)
        {
            log.Info("Waiting for clients...");
            TcpClient client = server.AcceptTcpClient();
            log.Info("Client connected...");
            ProcessRequest(client);
        }
    }
    
    public abstract void ProcessRequest(TcpClient client);
}