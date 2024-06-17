using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface IProductRepository
    {
        Produto GetProduct(int id);
    }
}
