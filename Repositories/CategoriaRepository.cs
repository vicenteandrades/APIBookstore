using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoryBaseRepository
    {
        public CategoriaRepository(BookstoreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetProdutosDeCategoriaAsync()
        {
            return await _context.Categorias.Include(p => p.Produtos).ToListAsync();
        }
    }
}
