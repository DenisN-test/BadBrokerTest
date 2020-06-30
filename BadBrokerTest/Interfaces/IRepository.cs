using System;
using System.Linq;

namespace BadBrokerTest.Interfaces
{
    interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(Guid? id);

        void SaveChanges();
    }
}
