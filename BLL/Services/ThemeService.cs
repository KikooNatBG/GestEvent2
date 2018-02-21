using BO;
using DAL.Repository;
using System.Collections.Generic;

namespace BLL.Services
{
    class ThemeService
    {
        private readonly ThemeRepository themeRepository;


        public ThemeService(ThemeRepository themeRepository)
        {
            this.themeRepository = themeRepository;
        }

        public List<Theme> findAll()
        {
            return themeRepository.findAll();
        }

        public Theme get(int? id)
        {
            return themeRepository.get(id);
        }

        public void create(Theme obj)
        {
            themeRepository.create(obj);
        }

        public virtual void update(Theme obj)
        {
            themeRepository.update(obj);
        }

        public virtual void delete(Theme obj)
        {
            themeRepository.delete(obj);
        }
    }
}
