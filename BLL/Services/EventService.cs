using BO;
using DAL.Repository;
using System.Collections.Generic;
using System;
using System.Linq;

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

        public List<Event> GetByThemeId(int? id)
        {
            return _eventRepository.FindAll().Where(e => e.Theme.Id == id).ToList();
        }

        public void Create(Event obj)
        {
            _eventRepository.Create(obj);
        }

        public void Update(Event obj)
        {
            _eventRepository.Update(obj);
        }

        public void Delete(Event obj)
        {
            _eventRepository.Delete(obj);
        }
    }
}
