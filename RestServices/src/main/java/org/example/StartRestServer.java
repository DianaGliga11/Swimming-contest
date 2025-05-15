package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Primary;

import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

@ComponentScan({"org.example.client", "org.example.controller", "example.repo", "example.model"})
@SpringBootApplication
public class StartRestServer {
    private static final Logger logger = LogManager.getLogger();

    public static void main(String[] args) {
        SpringApplication.run(StartRestServer.class, args);
    }

    @Bean(name = "properties")
    @Primary
    public Properties getProperties() {
        Properties properties = new Properties();
        properties.setProperty("jdbc.url", "jdbc:sqlite:D:\\Java programs\\mpp-proiect-java-DianaGliga11-lab4\\mpp-proiect-java-DianaGliga11-lab4\\SwimingContest.db");
//        try{
//            properties.load(new FileReader("db.config"));
//        }catch(IOException e){
//            logger.error("Error in org.example.StartRestServer loading db.config" + e.getMessage());
//        }
        return properties;
    }
}
