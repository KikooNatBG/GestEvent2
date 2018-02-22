using BO;
using DAL.Repository;
using System.Collections.Generic;
using System;

namespace BLL.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;

        public EventService(EventRepository eventRepository)
        {
            this._eventRepository = eventRepository;
        }

        public List<Event> FindAll()
        {
            return _eventRepository.FindAll();
        }

        public Event Get(int? id)
        {
            return _eventRepository.Get(id);
        }

        public void Create(Event obj)
        {
            _eventRepository.Create(obj);
        }

        public virtual void Update(Event obj)
        {
            _eventRepository.Update(obj);
        }

        public virtual void Delete(Event obj)
        {
            _eventRepository.Delete(obj);
        }
    }
}
