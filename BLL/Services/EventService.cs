using BO;
using DAL.Repository;
using System.Collections.Generic;

namespace BLL.Services
{
    class EventService
    {
        private readonly EventRepository eventRepository;

        public EventService(EventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public List<Event> findAll()
        {
            return eventRepository.findAll();
        }

        public Event get(int? id)
        {
            return eventRepository.get(id);
        }

        public void create(Event obj)
        {
            eventRepository.create(obj);
        }

        public virtual void update(Event obj)
        {
            eventRepository.update(obj);
        }

        public virtual void delete(Event obj)
        {
            eventRepository.delete(obj);
        }
    }
}
