using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Services;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Repositories;

public class ProductRepository : BaseRepository<Produto>,IFindService<Produto>, IProductRepository
{
    public ProductRepository(BookstoreContext context) : base(context)
    {
    }

        public async Task<IEnumerable<Produto>> FindNameAsync(string model)
        {
            var list = await _context.Produtos.ToListAsync();

            var product = list.Where(x => x.Name.ToLower().Contains(model.ToLower()));
            return product;
        }

        public Produto GetProduct(int id)
        {
            return _context.Produtos.Find(id);
        }
}