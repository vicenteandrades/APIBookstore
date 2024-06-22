using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Categoria>
    {
        Task <IEnumerable<Categoria>> GetProdutosDeCategoriaAsync();
        Categoria GetCategoria(int id);
    }
}
