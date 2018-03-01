using BO;
using DAL.Repository;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace BLL.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly ImageRepository _imageRepository;

        public EventService(EventRepository eventRepository, ImageRepository imageRepository )
        {
            this._eventRepository = eventRepository;
            this._imageRepository = imageRepository;
        }

        public EventService(EventRepository eventRepository, ImageRepository imageRepository)
        {
            this._eventRepository = eventRepository;
            this._imageRepository = imageRepository;
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
            if (null != obj.Images)
            {
                for (int i = obj.Images.Count - 1; i >= 0; i--)
                {
                    EventImage image = _imageRepository.Get(obj.Images[i].Id);
                    _imageRepository.Delete(image);

                    if (File.Exists(image.Path))
                    {
                        File.Delete(image.Path);
                    }
                }

                obj.Images.Clear();
            }

            _eventRepository.Delete(obj);

        }

        public List<Double> GetGeolocalisation(string address)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0},France&key=AIzaSyBWueE2eJriSCMWTWlokZhu39wkf_4lbME", Uri.EscapeDataString(address));
            List<Double> LatLon = new List<double>();
            
            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            Event evenement = new Event();

            LatLon.Add(Convert.ToDouble(locationElement.Element("lat").Value.Replace(".",",")));
            LatLon.Add(Convert.ToDouble(locationElement.Element("lng").Value.Replace(".", ",")));

            return LatLon;
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

