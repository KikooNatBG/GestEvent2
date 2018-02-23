using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class GenericRepository<T> where T : class
    {
        protected Context Context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Context context)
        {
            this.Context = context;
            _dbSet = this.Context.Set<T>();
        }

        public List<T> FindAll()
        {
            return _dbSet.ToList();
        }

        public T Get(int? id)
        {
            return _dbSet.Find(id);
        }

        public void Create(T obj)
        {
            _dbSet.Add(obj);
            Context.SaveChanges();
        }

        public virtual void Update(T obj)
        {
            Context.Entry(obj).State = EntityState.Modified;

            Context.SaveChanges();
        }

        public virtual void Delete(T obj)
        {
            _dbSet.Remove(obj);
            Context.SaveChanges();
        }
    }
}
