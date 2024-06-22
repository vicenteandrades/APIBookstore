using APIBookstore.Context;
using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public class OrderRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        public OrderRepository(BookstoreContext context) : base(context)
        {
        }

        public Pedido GetPedido(int id)
        {
            return _context.Pedidos.Find(id);
        }
    }
}
