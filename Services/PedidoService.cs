using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Repositories;

namespace APIBookstore.Services
{
    public class PedidoService
    {
        private readonly ProductRepository _repositoryProduct;

        public PedidoService(ProductRepository repositoryProduct)
        {
            _repositoryProduct = repositoryProduct;
        }

        public double CalcularTotal(Pedido pedido)
        {
            var produto = _repositoryProduct.GetProduct(pedido.ProdutoId);
            if (produto is not null)
            {
                return pedido.Quantidade * produto.Preco;
            }
            return 0;
        }
    }

}
