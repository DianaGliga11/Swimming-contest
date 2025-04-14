using System.Net.Sockets;
using log4net;
using Service;
using Networking;

namespace Server;

public class ContestConcurrentServcie : ConcurrentServer
{
    private readonly IContestServices server;
    private ClientWorker? worker;
    private static readonly ILog log = LogManager.GetLogger(typeof(ContestConcurrentServcie));
    
    public ContestConcurrentServcie(int port, string host, IContestServices server) : base(port, host)
    {
        this.server = server;
        log.Info("Server started...");
    }

    protected override Thread CreateWorker(TcpClient client)
    {
        worker = new ClientWorker(server, client);
        return new Thread(new ThreadStart(worker.Run));
    }
}