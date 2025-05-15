package org.example.client;

import example.model.Event;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.web.client.RestTemplate;

import java.util.concurrent.Callable;

public class RestClient {
    public static final String URL = "http://localhost:8080/contest/events";
    private final RestTemplate restTemplate = new RestTemplate();
    private static final Logger logger = LogManager.getLogger(RestClient.class);

    private <T> T execute(Callable<T> callable) throws Exception {
        try{
            return callable.call();
        }catch(Exception e){
            logger.error("Error in RestClient.execute() " ,e);
            throw new Exception(e);
        }
    }

    public Event[] getAll() throws Exception {
        return execute(() -> restTemplate.getForObject(URL, Event[].class));
    }

    public Event getById(Long id) throws Exception {
        return execute(() -> restTemplate.getForObject(String.format("%s/%s", URL, id), Event.class));
    }

    public Event create(Event event) throws Exception {
        return execute(() -> restTemplate.postForObject(URL, event, Event.class));
    }

    public void update(Event event) throws Exception {
        execute(() -> {
            restTemplate.put(String.format("%s/%s", URL, event.getId()), event);
            return null;
        });
    }

    public void delete(Long id) throws Exception {
        execute(() -> {
            restTemplate.delete(String.format("%s/%s", URL, id));
            return null;
        });
    }
}
