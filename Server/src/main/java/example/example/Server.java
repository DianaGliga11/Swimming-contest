package example.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.rmi.ServerException;

public abstract class Server {
    private final int port;
    private ServerSocket serverSocket = null;
    private static final Logger logger = LogManager.getLogger();

    public Server(int port) {
        this.port = port;
    }

    public void start() throws ServerException {
        try{
            serverSocket = new ServerSocket(port);
            while(true){
                logger.info("Waiting for a client...");
                Socket client = serverSocket.accept();
                logger.info("Client Connected");
                processRequest(client);
            }
        }catch(IOException e){
            throw new ServerException("Start server failed : " + e.getMessage());
        } catch (Exception e) {
            throw new RuntimeException(e);
        } finally{
            stop();
        }
    }

    protected abstract void processRequest(Socket client) throws Exception;

    public void stop() throws ServerException {
        try{
            serverSocket.close();
        }catch(IOException e){
            throw new ServerException("Close server failed: " + e.getMessage());
        }
    }

}
