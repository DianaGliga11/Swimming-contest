package org.example;

import Controller.MainController;
import Networking.ServicesProxy;
import contestUtils.IContestServices;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import javafx.application.Application;
import javafx.stage.Stage;
import protocolBuffers.ProtocolBufferServicesProxy;

import java.io.IOException;
import java.util.Properties;

public class StartClient extends Application {
    private static final int DEFAULT_PORT = 56789;
    private static final String DEFAULT_SERVER = "localhost";
    private static final Logger logger = LogManager.getLogger();

    @Override
    public void start(Stage stage){
        final Properties clientProperties = new Properties();
        try {
            clientProperties.load(StartClient.class.getResourceAsStream("/client.properties"));
            logger.info("Client properties loaded");
            clientProperties.list(System.out);
        } catch (IOException e) {
            logger.error("Cannot find file client.properties " + e);
            return;
        }

        String serverIP = clientProperties.getProperty("server.host", DEFAULT_SERVER);
        int port = DEFAULT_PORT;
        try {
            port = Integer.parseInt(clientProperties.getProperty("server.port"));
        } catch (NumberFormatException e) {
            logger.error("Wrong port number: " + e.getMessage());
            logger.info("Using default port number: " + DEFAULT_PORT);
        }

        logger.info("Using IP: " + serverIP + " on port " + port);
        IContestServices server = new ProtocolBufferServicesProxy(serverIP, port);
        try {
            FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/views/main-view.fxml"));
            Scene scene = new Scene(fxmlLoader.load());
            MainController controller = fxmlLoader.getController();
            controller.init(server, stage);
            stage.setScene(scene);
            stage.show();
        } catch (IOException e) {
            logger.error("Cannot start application " + e.getMessage());
        }

    }

    public static void main(String[] args) {
        Application.launch(args);
    }

}
