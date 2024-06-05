using APIBookstore.Context;
using APIBookstore.Models;
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
        private readonly BookstoreContext Context;

        public PedidosController(BookstoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public async Task< ActionResult<IEnumerable<Pedido>>> GetAsync()
        {
            var pedidos = await Context.Pedidos.ToListAsync();

            return (pedidos.Count == 0) ? NotFound("Não há pedidos cadastrados") : Ok(pedidos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterPedido")]
        public ActionResult<Pedido> Get(int id)
        {
            var pedido = Context.Pedidos.SingleOrDefault(p => p.PedidoId == id);

            return (pedido is null) ? NotFound("Não há pedidos cadastrados") : Ok(pedido);
        }

        [HttpPost]
        public ActionResult Post(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("O pedido não pode ser nulo");
            }

            var produto = Context.Produtos.Find(pedido.ProdutoId);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            if (pedido.Quantidade > produto.Quantidade)
            {
                return BadRequest("Não possuímos a quantidade de itens suficiente");
            }

            var pedidoService = new PedidoService(Context);
            pedido.Total = pedidoService.CalcularTotal(pedido);

            produto.Quantidade -= pedido.Quantidade;
            Context.Produtos.Update(produto);
            

            Context.Pedidos.Add(pedido);
            Context.SaveChanges();

            return CreatedAtRoute("ObterPedido", new { id = pedido.PedidoId }, pedido);


        }

        [HttpPut]
        public ActionResult Put(int id, Pedido pedido)
        {
            if(id != pedido.PedidoId)
            {
                return BadRequest("Os ids do pedido tem que ser iguais");
            }

            Context.Entry(pedido).State = EntityState.Modified;
            Context.SaveChanges();

            return Ok("Pedido Alterado com sucesso");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var pedido = Context.Pedidos.SingleOrDefault(x => x.PedidoId == id);

            if (pedido == null)
            {
                return BadRequest("Não podemos exluir, pois esse pedido não existe...");
            }

            Context.Pedidos.Remove(pedido);
            Context.SaveChanges();

            return Ok($"Pedido com id = {id} removido!");
        }

    }
}
