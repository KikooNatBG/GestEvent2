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

        public Event GetGeolocalisation(string address)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0},%2035000%20Rennes,%20France&key=AIzaSyBWueE2eJriSCMWTWlokZhu39wkf_4lbME", Uri.EscapeDataString(address));

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            Event evenement = new Event();
            
            evenement.longitude = Convert.ToDouble(locationElement.Element("lng").Value.Replace(".", ","));
            evenement.latitude = Convert.ToDouble(locationElement.Element("lat").Value.Replace(".", ","));
            
            return evenement;
        }
    }
}
