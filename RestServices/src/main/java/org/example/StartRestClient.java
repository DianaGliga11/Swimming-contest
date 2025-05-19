package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import org.example.client.RestClient;
import example.model.Event;

import java.util.Arrays;
import java.util.List;

public class StartRestClient {
    private static RestClient restClient = new RestClient();
    public static Event updatedEvent = null;
    private static final Logger logger = (Logger) LogManager.getLogger();

    public static void main(String[] args) {
        Event newEvent = new Event("sub apa", 1500);
        newEvent.setId(newEvent.getId());
        try {
            show(() -> {
                logger.info("\n\nTest create:\n\n");
                try {
                    Event result = restClient.create(newEvent);
                    logger.info(result.toString());
                    setCreatedEvent(result);
                } catch (Exception e) {
                    logger.error(e.toString());
                }
            });
        }catch(Exception e) {
            logger.error("Error in Test create: " + e);
        }

        show(()-> {
            logger.info("\n\nTest get all:\n\n");
            try{
                List<Event> result = Arrays.stream(restClient.getAll()).toList();
                result.forEach(System.out::println);
            } catch (Exception e) {
                logger.error("Error in Test get all: " + e);
            }
        });

        show(()-> {
            logger.info("\n\nTest find by ID:\n\n");
            try{
                System.out.println(restClient.getById(updatedEvent.getId()));
            }catch(Exception e) {
                logger.error("Error in Test find by ID: " + e);
            }
        });

        show(()-> {
            logger.info("\n\nTest update:\n\n");
            try{
                updatedEvent.setDistance(200);
                restClient.update(updatedEvent);
                logger.info("Updated Event: " + updatedEvent.toString());
            }catch(Exception e) {
                logger.error("Error in Test update: " + e);
            }
        });

        show(()->{
            logger.info("\n\nTest delete:\n\n");
            try{
                logger.info("Deleted Event with ID: " + updatedEvent.getId());
                restClient.delete(updatedEvent.getId());
            }catch(Exception e) {
                logger.error("Error in Test delete: " + e);
            }
        });
    }

    private static void setCreatedEvent(Event result) {
        updatedEvent = result;
    }

    private static void show(Runnable task) {
        try {
            task.run();
        } catch (Exception e) {
            logger.error("Services exception: " + e);
        }

    }

}
