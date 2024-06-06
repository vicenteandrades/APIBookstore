using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly BookstoreContext Context;

        public ProdutosController(BookstoreContext context)
        {
            Context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get() 
        {
            var produtos = Context.Produtos.ToList();

            return (produtos is null) ? NotFound("Não há produtos cadastradas") : Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutos")]
        public ActionResult<Categoria> Get([FromRoute]int id)
        {
            var produto = Context.Produtos.SingleOrDefault(p => p.ProdutoId == id);

            return (produto is null) ? NotFound($"Não uma produto cadastrado com o id {id}") : Ok(produto);
        }

        [HttpGet("Nome")]
        
        public async Task<ActionResult<Produto>> GetProdutoNomeAsync(string nome)
        {
            var list = await Context.Produtos.ToListAsync();

            var produtos = list
                           .Where(x => x.Name.ToLower().Contains(nome.ToLower()))
                           .ToList();

            return (produtos.Count == 0) ? BadRequest($"Não há produto que contém o nome {nome}") : Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Não podemos cadastrar um produto nulo");
            }

            Context.Produtos.Add(produto);
            Context.SaveChanges();
            return CreatedAtRoute("ObterProdutos", new  { id = produto.ProdutoId}, produto);
        }

        [HttpPut]
        public ActionResult Put(int id, Produto produto)
        {
            if (produto is null || produto.ProdutoId != id)
            {
                return NotFound("Verifique os dados");
            }

            Context.Entry(produto).State = EntityState.Modified;
            Context.SaveChanges();

            return Ok($"Produto com o id {id} atualizado.");
        }

        [HttpDelete]
        public ActionResult Delete(int id) 
        {
            var produto = Context.Produtos.SingleOrDefault(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound($"Não podemos remover, pois não há nenhuma produto com o id {id}");
            }
            Context.Produtos.Remove(produto);
            Context.SaveChanges();

            return Ok($"O produto {produto.Name} com o id {id} foi deletado com sucesso.");
        }


    }

}
