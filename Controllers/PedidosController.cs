using APIBookstore.Context;
using APIBookstore.DTOs;
using APIBookstore.Models;
using APIBookstore.Repositories;
using APIBookstore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ProductRepository _repositoryProduct;
        public PedidosController(UnitOfWork unitOfWork,IMapper mapper ,ProductRepository repositoryProduct)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repositoryProduct = repositoryProduct;
        }


        [HttpGet]
        public async Task< ActionResult<IEnumerable<PedidoDTO>>> GetAsync()
        {
            var order = await _unitOfWork.OrderRepository.GetAllAsync();
            var orderDto = _mapper.Map<IEnumerable<PedidoDTO>>(order);
            return (orderDto.Any()) ? Ok(orderDto): NotFound("Não há pedidos cadastrados");
        }

        [HttpGet("{id:int:min(1)}", Name = "GetOrder")]
        public async Task <ActionResult<PedidoDTO>> Get(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            var orderDto = _mapper.Map<PedidoDTO>(order);
            
            return (orderDto is null) ? NotFound("Não há pedidos cadastrados") : Ok(orderDto);
        }
        
        [HttpPost]
        public ActionResult Post(PedidoDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("O pedido não pode ser nulo");
            }
            var product = _unitOfWork.ProductRepository.GetProduct(orderDto.ProdutoId);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }

            if (orderDto.Quantidade > product.Quantidade)
            {
                return BadRequest("Não possuímos a quantidade de itens suficiente");
            }

            var order = _mapper.Map<Pedido>(orderDto);

            var pedidoService = new PedidoService(_repositoryProduct);
            order.Total = pedidoService.CalcularTotal(order);
            if (orderDto.Situacao.Equals("Aprovado") || orderDto.Situacao.Equals("Aproved"))
            {
                product.Quantidade -= orderDto.Quantidade;
                _unitOfWork.ProductRepository.Update(product);
                
            }


            var orderCreated = _unitOfWork.OrderRepository.Create(order);

            _unitOfWork.Commit();

            var orderCreatedDto = _mapper.Map<PedidoDTO>(orderCreated);
            
            return CreatedAtRoute("GetOrder", new { id = orderCreatedDto.PedidoId }, orderCreatedDto);
        }
        
        [HttpPut]
        public ActionResult Put(int id, PedidoDTO orderDto)
        {
            if(id != orderDto.PedidoId)
            {
                return BadRequest("Os ids do pedido tem que ser iguais");
            }

            var order = _mapper.Map<Pedido>(orderDto);

            _unitOfWork.OrderRepository.Update(order);
            _unitOfWork.Commit();

            return Ok("Pedido Alterado com sucesso");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var pedido = _unitOfWork.OrderRepository.GetPedido(id);

            if (pedido == null)
            {
                return BadRequest("Não podemos exluir, pois esse pedido não existe...");
            }

            _unitOfWork.OrderRepository.Delete(pedido);
            _unitOfWork.Commit();

            return Ok($"Pedido com id = {id} removido!");
        }

    }
}
