namespace APIBookstore.Repositories
{
    public interface IBaseRepository <T> 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task <T> GetByIdAsync (int id);
        T Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}
