using System.Text.Json;
using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using APIBookstore.DTOs;
using APIBookstore.Pagination;
using APIBookstore.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutosController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<ProdutoDTO>> >Get() 
        {
            var product = await _unitOfWork.ProductRepository.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProdutoDTO>>(product);

            return (productsDto.Any()) ? Ok(productsDto) : NotFound("Não há produtos cadastradas");
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutos")]
        public async Task<ActionResult<ProdutoDTO>> Get([FromRoute]int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            var productDto = _mapper.Map<ProdutoDTO>(product);

            return (productDto is null) ? NotFound($"Não uma produto cadastrado com o id {id}") : Ok(productDto);
        }

        [HttpGet("Nome")]
        
        public async Task<ActionResult<ProdutoDTO>> GetProdutoNomeAsync(string nome)
        {
            var products = await _unitOfWork.ProductRepository.FindNameAsync(nome);

            var productsDto = _mapper.Map<IEnumerable<ProdutoDTO>>(products);
            
            return (productsDto.Any()) ? Ok(productsDto) : BadRequest($"Não há produto que contém o nome {nome}");
        }

        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetPaginationAsync([FromQuery] QueryParameters parameters)
        {
            var productsPagination = await _unitOfWork.ProductRepository.GetProductPaginationAsync(parameters);

            var metadata = new
            {
                productsPagination.CurrentPage,
                productsPagination.TotalCount,
                productsPagination.TotalPages,
                productsPagination.ItemsPerPage,
                productsPagination.HasPrevious,
                productsPagination.HasNext
            };

            Response.Headers.Append("X-Products", JsonSerializer.Serialize(metadata));

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(productsPagination));
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Não podemos cadastrar um produto nulo");
            }

            var product = _mapper.Map<Produto>(productDto);

            var productCreated = _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            var productCreatedDto = _mapper.Map<ProdutoDTO>(productCreated);
            return CreatedAtRoute("ObterProdutos", new  { id = productCreatedDto.ProdutoId}, productCreatedDto);
        }

        [HttpPut]
        public ActionResult Put(int id, ProdutoDTO productDto)
        {
            if (productDto is null || productDto.ProdutoId != id)
            {
                return NotFound("Verifique os dados");
            }

            var product = _mapper.Map<Produto>(productDto);

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();
            
            return Ok($"Produto com o id {id} atualizado.");
        }

        [HttpPatch("{id}")]
        public ActionResult<ProdutoDTOUpdateResponse> Pacth(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
  
        {
            if(patchProdutoDto is null || id <= 0)
            {
                return BadRequest("Dados invalidos");
            }

            var produto = _unitOfWork.ProductRepository.GetProduct(id);

            if (produto is null)
            {
                return NotFound("Produto null");
            }

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

            if(!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdateRequest, produto);

            _unitOfWork.ProductRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));

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
