using APIBookstore.Models;

namespace APIBookstore.Repositories
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        Pedido GetPedido(int id);
    }
}
