package org.example;
import contestUtils.IContestServices;
import Networking.ClientWorker;
import java.net.Socket;

public class ContestConcurrentServer  extends ConcurrentServer{
    private final IContestServices contestServices;

    public ContestConcurrentServer(int port, IContestServices contestServices){
        super(port);
        this.contestServices = contestServices;
    }


    @Override
    protected Thread createWorker(Socket client) {
        ClientWorker worker = new ClientWorker(contestServices,client);
        return new Thread(worker);
    }
}
