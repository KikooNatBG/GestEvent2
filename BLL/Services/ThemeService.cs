using BO;
using DAL.Repository;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ThemeService
    {
        private readonly ThemeRepository _themeRepository;


        public ThemeService(ThemeRepository themeRepository)
        {
            this._themeRepository = themeRepository;
        }

        public List<Theme> FindAll()
        {
            return _themeRepository.FindAll();
        }

        public Theme Get(int? id)
        {
            return _themeRepository.Get(id);
        }

        public void Create(Theme obj)
        {
            _themeRepository.Create(obj);
        }

        public virtual void Update(Theme obj)
        {
            _themeRepository.Update(obj);
        }

        public virtual void Delete(Theme obj)
        {
            _themeRepository.Delete(obj);
        }

    }
}
