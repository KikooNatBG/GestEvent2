using BO;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ImageService
    {
        private readonly ImageRepository _imageRepository;


        public ImageService(ImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }

        public EventImage Get(int? id)
        {
            return _imageRepository.Get(id);
        }

        public void Delete(EventImage image)
        {
            if (File.Exists(image.Path))
            {
                File.Delete(image.Path);
            }

            _imageRepository.Delete(image);
        }
    }
}
