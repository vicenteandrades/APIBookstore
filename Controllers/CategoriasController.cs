using APIBookstore.Context;
using APIBookstore.Models;
using APIBookstore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using APIBookstore.DTOs;
using APIBookstore.DTOs.Mappings;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(UnitOfWork unitOfWork, ILogger<CategoriasController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("Produtos")]

        public async Task <ActionResult<IEnumerable<CategoriaDTO>>> GetAllProdutosAsync()
        {

            _logger.LogInformation("=====  Get/ Categoria /Produtos =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");
            
            var categorias = await _unitOfWork.CategoryRepository.GetProdutosDeCategoriaAsync();

            var categoriasDto = CategoriaMappingExtensions.ToCategoriaDtoList(categorias);

            return (categoriasDto is null) ? NotFound("Não há categorias cadastradas") : Ok(categoriasDto);
        }


        [HttpGet]
        public async Task <ActionResult<IEnumerable<CategoriaDTO>>> GetAsyncCategorias()
        {
            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");
            
            var categorias = await _unitOfWork.CategoryRepository.GetAllAsync();

            var categoriasDto = CategoriaMappingExtensions.ToCategoriaDtoList(categorias);

            return (categoriasDto is null) ? NotFound("Não há categorias cadastradas") : Ok(categoriasDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetIdAsync(int id)
        {

            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"========= ID = {id} =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var categoria = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            var categoriaDTO = CategoriaMappingExtensions.ToCategoriaDTO(categoria);

            return (categoriaDTO is null) ? NotFound($"Não uma categoria cadastrado com o id {id}") : Ok (categoriaDTO);
        }

        [HttpPost]
        public ActionResult Post(CategoriaDTO categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"============== Model State INVALIDO ==============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Não podemos cadastrar uma categoria nula");
            }

            _logger.LogInformation($"=====  Categoria Cadastrada!!! {categoriaDto} =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var categoria = CategoriaMappingExtensions.ToCategoria(categoriaDto);
            
            var categoriaCriada = _unitOfWork.CategoryRepository.Create(categoria);
            _unitOfWork.Commit();

            var categoriaCriadaDto = CategoriaMappingExtensions.ToCategoriaDTO(categoriaCriada);
            
            return CreatedAtRoute("ObterCategoria", new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
        }

        [HttpPut]
        public ActionResult Put(int id, CategoriaDTO categoriaDto)
        {
            if(categoriaDto is null || categoriaDto.CategoriaId != id)

            {
                _logger.LogInformation($"=====  VERIFICAÇÂO INVALIDA =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return NotFound("Verifique os dados");
            }
            _logger.LogInformation($"=====  CATEGORIA ATUALIZADA =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var categoria = CategoriaMappingExtensions.ToCategoria(categoriaDto);
            
            _unitOfWork.CategoryRepository.Update(categoria);
            _unitOfWork.Commit();

            return Ok("Alterado com sucesso.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = _unitOfWork.CategoryRepository.GetCategoria(id);

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

            _unitOfWork.CategoryRepository.Delete(categoria);
            _unitOfWork.Commit();

            return Ok($"A categoria id {id} foi deletada com sucesso!");
        }

    }
}
