using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaRepository _repository;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(CategoriaRepository repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("Produtos")]

        public async Task <ActionResult<IEnumerable<Categoria>>> GetAllProdutosAsync()
        {

            _logger.LogInformation("=====  Get/ Categoria /Produtos =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");



            var categorias = await _repository.GetProdutosDeCategoriaAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }


        [HttpGet]
        public async Task <ActionResult<IEnumerable<Categoria>>> GetAsyncCategorias()
        {
            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var categorias = await _repository.GetAllAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetIdAsync(int id)
        {

            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"========= ID = {id} =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");



            var produto = await _repository.GetByIdAsync(id);

            return (produto is null) ? NotFound($"Não uma categoria cadastrado com o id {id}") : Ok (produto);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"============== Model State INVALIDO ==============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Não podemos cadastrar uma categoria nula");
            }

            _logger.LogInformation($"=====  Categoria Cadastrada!!! {categoria} =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var categoriaCriada = _repository.Create(categoria);
            
            return CreatedAtRoute("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(categoria is null || categoria.CategoriaId != id)

            {
                _logger.LogInformation($"=====  VERIFICAÇÂO INVALIDA =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return NotFound("Verifique os dados");
            }
            _logger.LogInformation($"=====  CATEGORIA ATUALIZADA =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            _repository.Update(categoria);

            return Ok("Alterado com sucesso.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetByIdAsync(id);

            if(categoria is null)
            {
                _logger.LogInformation($"=====  DELEÇÃO INVALIDA =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return NotFound("Não há essa categoria");
            }
            _logger.LogInformation($"=====  DELEÇÂO DO ID {id} =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            _repository.Delete(id);

            return Ok($"A categoria id {id} foi deletada com sucesso!");
        }

    }
}
