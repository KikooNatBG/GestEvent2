using BO;
using DAL.Repository;
using System.Collections.Generic;
using System;
using System.Net;
using System.Xml.Linq;
using System.Net.Http;

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

        public void GetGeolocalisation(string address)
        {
            string req = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0},%2035000%20Rennes,%20France&key=AIzaSyBWueE2eJriSCMWTWlokZhu39wkf_4lbME", address);
            string re = new HttpClient().GetStringAsync(req).Result;
            Event events = Newtonsoft.Json.JsonConvert.DeserializeObject<Event>(re);
        }
    }
}
