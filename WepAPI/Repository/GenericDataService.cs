using Microsoft.EntityFrameworkCore.ChangeTracking;
using WepAPI.Models;
using WepAPI.Services;

namespace WepAPI.Repository
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly СlinicReceptionDbContext _context;

        public GenericDataService(СlinicReceptionDbContext contextFactory)
        {
            _context = contextFactory;
        }

        public T Create(T entity)
        {
            EntityEntry<T> createdEntity = _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return createdEntity.Entity;
        }

        public bool Delete(int id)
        {
            T entity = _context.Set<T>().FirstOrDefault((e) => e.Id == id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public T Get(int id)
        {
            T entity = _context.Set<T>().FirstOrDefault((e) => e.Id == id);
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> entities = _context.Set<T>().ToArray();
            return entities;
        }

        public T Update(int id, T entity)
        {
            entity.Id = id;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
