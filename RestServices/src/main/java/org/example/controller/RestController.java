package org.example.controller;

import example.repo.EntityRepoException;
import example.model.Event;
import example.repo.EventDBRepository;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;

@org.springframework.web.bind.annotation.RestController
@RequestMapping("contest/events")
public class RestController {
    private static final String template = "Hello, %s!";
    private static final Logger logger = LogManager.getLogger();

    @Autowired
    private EventDBRepository eventDBRepository;

    @GetMapping("/greeting")
    public String greeting(@RequestParam(value = "name", defaultValue = "Hello") String name){
        return String.format(template, name);
    }

    @GetMapping
    public List<Event> getAll() throws EntityRepoException {
        logger.info("Getting all events");
        return eventDBRepository.getAll().stream().toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById (@PathVariable String id) throws EntityRepoException {
        logger.info("Getting event with id {}", id);
        Event event = eventDBRepository.findById(Long.parseLong(id));
        if(event == null) {
            return new ResponseEntity<>("Event not found ", HttpStatus.NOT_FOUND);
        }
        else{
            event.setId(Long.parseLong(id));
            return new ResponseEntity<>(event, HttpStatus.OK);
        }
    }

    @PostMapping
    public Event create(@RequestBody Event event) throws EntityRepoException {
        logger.info("Creating event {}", event);
        eventDBRepository.add(event);
        return event;
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> update(@PathVariable String id, @RequestBody Event event) throws EntityRepoException {
        logger.info("Updating event {}", event);
        eventDBRepository.update(Long.parseLong(id), event);
        return new ResponseEntity<>(event, HttpStatus.OK);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable String id) throws EntityRepoException {
        logger.info("Deleting event {}", id);
        eventDBRepository.remove(Long.parseLong(id));
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @ExceptionHandler(Exception.class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    public String eventError(Exception ex){
        return ex.getMessage();
    }
}
