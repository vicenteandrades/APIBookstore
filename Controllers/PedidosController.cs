using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Repositories;
using APIBookstore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IBaseRepository<Pedido> _repository;
        private readonly ProductRepository _repositoryProduct;

        public PedidosController(IBaseRepository<Pedido> repository, ProductRepository repositoryProduct)
        {
            _repository = repository;
            _repositoryProduct = repositoryProduct;
        }


        [HttpGet]
        public async Task< ActionResult<IEnumerable<Pedido>>> GetAsync()
        {
            var pedidos = await _repository.GetAllAsync();

            return (pedidos is null) ? NotFound("Não há pedidos cadastrados") : Ok(pedidos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterPedido")]
        public async Task <ActionResult<Pedido>> Get(int id)
        {
            var pedido = await _repository.GetByIdAsync(id);

            return (pedido is null) ? NotFound("Não há pedidos cadastrados") : Ok(pedido);
        }
        
        [HttpPost]
        public ActionResult Post(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("O pedido não pode ser nulo");
            }

            var produto = _repositoryProduct.GetProduct(pedido.ProdutoId);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            if (pedido.Quantidade > produto.Quantidade)
            {
                return BadRequest("Não possuímos a quantidade de itens suficiente");
            }

            var pedidoService = new PedidoService(_repositoryProduct);
            pedido.Total = pedidoService.CalcularTotal(pedido);

            produto.Quantidade -= pedido.Quantidade;
            _repositoryProduct.Update(produto);


            _repository.Create(pedido);

            return CreatedAtRoute("ObterPedido", new { id = pedido.PedidoId }, pedido);
        }
        
        [HttpPut]
        public ActionResult Put(int id, Pedido pedido)
        {
            if(id != pedido.PedidoId)
            {
                return BadRequest("Os ids do pedido tem que ser iguais");
            }

            _repository.Update(pedido);

            return Ok("Pedido Alterado com sucesso");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var pedido = _repository.GetByIdAsync(id);

            if (pedido == null)
            {
                return BadRequest("Não podemos exluir, pois esse pedido não existe...");
            }

            _repository.Delete(id);

            return Ok($"Pedido com id = {id} removido!");
        }

    }
}
