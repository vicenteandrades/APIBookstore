using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Repositories
{
    public class CategoryRepository : BaseRepository<Categoria>, ICategoryRepository
    {
        public CategoryRepository(BookstoreContext context) : base(context)
        {
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.Find(id);
        }

        public async Task<IEnumerable<Categoria>> GetProdutosDeCategoriaAsync()
        {
            return await _context.Categorias.Include(p => p.Produtos).ToListAsync();
        }
    }
}
