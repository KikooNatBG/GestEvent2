using BO;
using DAL.Repository;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

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

        public Event GetGeolocalisation(string address)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBWueE2eJriSCMWTWlokZhu39wkf_4lbME", Uri.EscapeDataString(address));

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            Event evenement = new Event();
            
            evenement.Lagitude = Convert.ToDouble(locationElement.Element("lat").Value.Replace(".",","));
            evenement.Longitude = Convert.ToDouble(locationElement.Element("lng").Value.Replace(".", ","));

            return evenement;
        }


        public List<Event> GetEventByIDTheme(int pIDTheme)
        {
            if (pIDTheme != 0)
            {
                return _eventRepository.GetEventsByIDTheme(pIDTheme);
            }
            return new List<Event>();
        }
    }
}

