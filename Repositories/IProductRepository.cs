using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface IProductRepository : IBaseRepository<Produto>
    {
        Produto GetProduct(int id);
        Task<IEnumerable<Produto>> FindNameAsync(string name);
    }
}
