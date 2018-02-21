using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class GenericRepository<T> where T : class
    {
        protected Context context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(Context context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }

        public List<T> findAll()
        {
            return dbSet.ToList();
        }

        public T get(int? id)
        {
            return dbSet.Find(id);
        }

        public void create(T obj)
        {
            dbSet.Add(obj);
            context.SaveChanges();
        }

        public virtual void update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;

            context.SaveChanges();
        }

        public virtual void delete(T obj)
        {
            dbSet.Remove(obj);
            context.SaveChanges();
        }
    }
}
