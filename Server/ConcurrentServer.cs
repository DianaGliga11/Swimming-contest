using System.Net.Sockets;

namespace Server;

public abstract class ConcurrentServer : IServer
{
    protected ConcurrentServer(int port, string host) : base(port, host)
    { }

    public override void ProcessRequest(TcpClient client)
    {
        Thread thread = CreateWorker(client);
        thread.Start();
    }
    
    protected abstract Thread CreateWorker(TcpClient client);
}