using APIBookstore.Context;
using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public BookstoreContext _context;

        public UnitOfWork(BookstoreContext context)
        {
            _context = context;
        }

        private IClientRepository _clientRepository;

        public IClientRepository ClientRepository
                => _clientRepository = _clientRepository ?? new ClientRepository(_context);

        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
            => _productRepository = _productRepository ?? new ProductRepository(_context);

        private ICategoryRepository _categoryRepository;
        public ICategoryRepository CategoryRepository
            => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        private IPedidoRepository _orderRepository;
        public IPedidoRepository OrderRepository
            => _orderRepository = _orderRepository ?? new OrderRepository (_context);

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
