using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using APIBookstore.Repositories;

namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProdutosController(ProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Produto>> >Get() 
        {
            var produtos = await _repository.GetAllAsync();

            return (produtos is null) ? NotFound("Não há produtos cadastradas") : Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutos")]
        public async Task<ActionResult<Produto>> Get([FromRoute]int id)
        {
            var produto = await _repository.GetByIdAsync(id);

            return (produto is null) ? NotFound($"Não uma produto cadastrado com o id {id}") : Ok(produto);
        }

        [HttpGet("Nome")]
        
        public async Task<ActionResult<Produto>> GetProdutoNomeAsync(string nome)
        {
            var produtos = await _repository.FindNameAsync(nome);

            return (produtos is null) ? BadRequest($"Não há produto que contém o nome {nome}") : Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Não podemos cadastrar um produto nulo");
            }

            _repository.Create(produto);
            return CreatedAtRoute("ObterProdutos", new  { id = produto.ProdutoId}, produto);
        }

        [HttpPut]
        public ActionResult Put(int id, Produto produto)
        {
            if (produto is null || produto.ProdutoId != id)
            {
                return NotFound("Verifique os dados");
            }

            _repository.Update(produto);

            return Ok($"Produto com o id {id} atualizado.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var produto = _repository.GetByIdAsync(id);

            if(produto is null)
            {
                return NotFound($"Não podemos remover, pois não há nenhuma produto com o id {id}");
            }

            _repository.Delete(id);

            return Ok($"O produto com o id {id} foi deletado com sucesso.");
        }


    }

}
