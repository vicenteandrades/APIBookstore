using APIBookstore.Models;
using APIBookstore.Pagination;

namespace APIBookstore.Repositories
{
    public interface IProductRepository : IBaseRepository<Produto>
    {
        Produto GetProduct(int id);
        Task<IEnumerable<Produto>> FindNameAsync(string name);
        Task<PagedList<Produto>> GetProductPaginationAsync(QueryParameters parameters);
    }
}
