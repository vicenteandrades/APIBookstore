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
        private readonly UnitOfWork _unitOfWork;

        public ProdutosController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Produto>> >Get() 
        {
            var produtos = await _unitOfWork.ProductRepository.GetAllAsync();

            return (produtos is null) ? NotFound("Não há produtos cadastradas") : Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutos")]
        public async Task<ActionResult<Produto>> Get([FromRoute]int id)
        {
            var produto = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            return (produto is null) ? NotFound($"Não uma produto cadastrado com o id {id}") : Ok(produto);
        }

        [HttpGet("Nome")]
        
        public async Task<ActionResult<Produto>> GetProdutoNomeAsync(string nome)
        {
            var produtos = await _unitOfWork.ProductRepository.FindNameAsync(nome);

            return (produtos is null) ? BadRequest($"Não há produto que contém o nome {nome}") : Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Não podemos cadastrar um produto nulo");
            }

            _unitOfWork.ProductRepository.Create(produto);
            _unitOfWork.Commit();
            return CreatedAtRoute("ObterProdutos", new  { id = produto.ProdutoId}, produto);
        }

        [HttpPut]
        public ActionResult Put(int id, Produto produto)
        {
            if (produto is null || produto.ProdutoId != id)
            {
                return NotFound("Verifique os dados");
            }

            _unitOfWork.ProductRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok($"Produto com o id {id} atualizado.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var produto = _unitOfWork.ProductRepository.GetProduct(id);

            if(produto is null)
            {
                return NotFound($"Não podemos remover, pois não há nenhuma produto com o id {id}");
            }

            _unitOfWork.ProductRepository.Delete(produto);
            _unitOfWork.Commit();

            return Ok($"O produto com o id {id} foi deletado com sucesso.");
        }


    }

}
