package example.example;

import java.net.Socket;

public abstract class ConcurrentServer extends Server{
    public ConcurrentServer(int port) {
        super(port);
    }

    protected void processRequest(Socket client) throws Exception {
        Thread worker = createWorker(client);
        worker.start();
    }

    protected abstract Thread createWorker(Socket client) throws Exception;
}
