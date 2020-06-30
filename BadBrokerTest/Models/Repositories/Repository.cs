using BadBrokerTest.DataContext;
using BadBrokerTest.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace BadBrokerTest.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private TestDataContext dataContext;
        private IDbSet<T> entities;

        public Repository(TestDataContext context) {
            dataContext = context;
        }

        private IDbSet<T> Entities {
            get {
                if (entities == null) {
                    entities = dataContext.Set<T>();
                }
                return entities;
            }
        }

        public virtual IQueryable<T> GetAll() {
            return Entities;
        }

        public virtual T Get(int id) {
            return Entities.Find(id);
        }

        public virtual void Create(T entity) {
            Entities.Add(entity);
        }

        public virtual void Update(T entity) {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(Guid? id) {
            var entity = Entities.Find(id);
            if (entity != null)
                Entities.Remove(entity);
        }

        public virtual void SaveChanges() {
            dataContext.SaveChanges();
        }
    }
}