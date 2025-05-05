package example.example;

import contestUtils.IContestServices;
import protocolBuffers.ProtocolBufferWorker;

import java.net.Socket;

public class ProtocolBufferServer extends ConcurrentServer {
    private final IContestServices contestServices;

    public ProtocolBufferServer(int port, IContestServices contestServices) {
        super(port);
        this.contestServices = contestServices;
    }

    @Override
    protected Thread createWorker(Socket client) throws Exception {
        ProtocolBufferWorker worker = new ProtocolBufferWorker(contestServices,client);
        return new Thread(worker);
    }
}
