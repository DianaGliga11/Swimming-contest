package org.example;

import java.util.List;

public class EventImplementationService implements EventService {
    private final EventRepository eventRepository;

    public EventImplementationService(final EventRepository eventRepository) {
        this.eventRepository = eventRepository;
    }

    @Override
    public void add(Event entity) throws EntityRepoException {
        eventRepository.add(entity);
    }

    @Override
    public void remove(Long id) throws EntityRepoException {
        eventRepository.remove(id);
    }

    @Override
    public void update(Long id, Event entity) throws EntityRepoException {
        eventRepository.update(id, entity);
    }

    @Override
    public List<Event> getAll() throws EntityRepoException {
        return eventRepository.getAll();
    }

    @Override
    public Event findById(Long id) throws EntityRepoException {
        return eventRepository.findById(id);
    }
}
