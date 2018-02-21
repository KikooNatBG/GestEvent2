using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    class GenericService<T> where T : class
    {
        protected GenericRepository<T> genericRepo;

        public GenericService() {

        }

        public List<T> findAll()
        {
            return genericRepo.findAll();
        }

        public T get(int? id)
        {
            return genericRepo.get(id);
        }

        public void create(T obj)
        {
            genericRepo.create(obj);
        }

        public virtual void update(T obj)
        {
            genericRepo.update(obj);
        }

        public virtual void delete(T obj)
        {
            genericRepo.delete(obj);
        }
    }
}
