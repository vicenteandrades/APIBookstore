using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface ICategoryBaseRepository : IBaseRepository<Categoria>
    {
        Task <IEnumerable<Categoria>> GetProdutosDeCategoriaAsync();
    }
}
