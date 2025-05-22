package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Primary;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
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
        //properties.setProperty("jdbc.url", "jdbc:sqlite:D:\\Java programs\\mpp-proiect-java-DianaGliga11-lab4\\mpp-proiect-java-DianaGliga11-lab4\\SwimingContest.db");
        try (InputStream input = getClass().getClassLoader().getResourceAsStream("bd.config")) {
            if (input == null) {
                logger.error("bd.config not found in classpath!");
            } else {
                properties.load(input);
            }
        } catch (IOException e) {
            logger.error("Error loading bd.config: " + e.getMessage(), e);
        }

        return properties;
    }

    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/contest/events/greeting-javaconfig")
                        .allowedOrigins("http://localhost:8080");
                registry.addMapping("/contest/events-javaconfig").allowedOrigins("http://localhost:8080")
                        .allowedMethods("GET", "POST", "PUT", "DELETE");
            }
        };
    }
}

