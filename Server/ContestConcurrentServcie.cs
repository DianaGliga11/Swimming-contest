using System.Net.Sockets;
using log4net;
using Service;
using Networking;

namespace Server;

public class ContestConcurrentServcie : ConcurrentServer
{
    private readonly IContestServices _server;
    private ClientWorker? _worker;
    private static readonly ILog Log = LogManager.GetLogger(typeof(ContestConcurrentServcie));
    
    public ContestConcurrentServcie(int port, string host, IContestServices server) : base(port, host)
    {
        this._server = server;
        Log.Info("Server started...");
    }

    protected override Thread CreateWorker(TcpClient client)
    {
        _worker = new ClientWorker(_server, client);
        return new Thread(new ThreadStart(_worker.Run));
    }
}