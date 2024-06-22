using APIBookstore.Context;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly BookstoreContext _context;

        public BaseRepository(BookstoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public bool Update(T entity)
        {
            if (entity is not null)
            {
                _context.Set<T>().Update(entity);
                return true;
            }

            return false;
        }

        public bool Delete(T entity)
        {
            if (entity is not null)
            {
                _context.Set<T>().Remove(entity);
                return true;
            }

            return false;

        }
    }
}
