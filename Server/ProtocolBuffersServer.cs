using System.Net.Sockets;
using Networking.ProtocolBuffers;
using Service;
namespace Server;

public class ProtocolBuffersServer: ConcurrentServer
{
    private readonly IContestServices _server;
    private ProtocolBufferWorker _worker;
    public ProtocolBuffersServer(int port, string host, IContestServices server) : base(port, host)
    {
        this._server = server;
    }

    protected override Thread CreateWorker(TcpClient client)
    {
        _worker = new ProtocolBufferWorker(_server, client);
        return new Thread(new ThreadStart(_worker.Run));
    }
}