using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository ClientRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IPedidoRepository OrderRepository { get; }
        void Commit();

    }
}
