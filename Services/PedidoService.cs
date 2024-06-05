using APIBookstore.Context;
using APIBookstore.Models;

namespace APIBookstore.Services
{
    public class PedidoService
    {
        private readonly BookstoreContext _context;

        public PedidoService(BookstoreContext context)
        {
            _context = context;
        }

        public double CalcularTotal(Pedido pedido)
        {
            var produto = _context.Produtos.Find(pedido.ProdutoId);
            if (produto != null)
            {
                return pedido.Quantidade * produto.Preco;
            }
            return 0;
        }
    }

}
