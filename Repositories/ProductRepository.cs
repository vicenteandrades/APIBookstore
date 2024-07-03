using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Pagination;
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

            var product = list.Where(x => x.Name.Contains(model));
            return product;
        }

        public async Task<PagedList<Produto>> GetProductPaginationAsync(QueryParameters parameters)
        {
            var list = await _context.Produtos.ToListAsync();

            return PagedList<Produto>.ToPagedList(list.AsQueryable(), parameters.Page, parameters.ItemsPerPage);

        }


        public Produto GetProduct(int id)
        {
            return _context.Produtos.Find(id);
        }
}