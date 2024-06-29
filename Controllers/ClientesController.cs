using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using APIBookstore.DTOs;
using APIBookstore.Models;
using APIBookstore.Repositories;
using AutoMapper;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(UnitOfWork unitOfWork, IMapper mapper ,ILogger<ClientesController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAsync()
        {
            _logger.LogInformation($"======== GET CLIENTES ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var clientes = await _unitOfWork.ClientRepository.GetAllAsync();

            var clientesDto = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);

            return (clientesDto.Any() ) ? Ok(clientes) : NotFound("Não há clientes cadastrados.");
        }

        [HttpGet("Pedidos")]
        public async Task <ActionResult<IEnumerable<ClienteDTO>>> GetPedidosAsync()
        {

            _logger.LogInformation($"======== GET CLIENTES / PEDIDOS ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var clientsWithOrder = await _unitOfWork.ClientRepository.GetOrder();

            if (clientsWithOrder is null)
            {
                _logger.LogInformation($"======== PEDIDOS INDISPONIVEIS ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return NotFound("Não há pedidos realizados.");
            }

            var clientsWithOrderDto = _mapper.Map<IEnumerable<ClienteDTO>>(clientsWithOrder);

            return Ok(clientsWithOrderDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCliente")]
        public async Task <ActionResult<ClienteDTO>> GetAsync(int id)
        {
            _logger.LogInformation($"======== GET CLIENTES ID = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var cliente = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            var clienteDto = _mapper.Map<ClienteDTO>(cliente);

            return (clienteDto is null) ? NotFound($"Não há nenhum cliente com o id = {id}") : Ok(clienteDto);
        }

        [HttpGet("{name}")]

        public async Task <ActionResult<ClienteDTO>> GetAsyncName(string name)
        {
            _logger.LogInformation($"======== GET CLIENTES NAME = {name} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var clients = await _unitOfWork.ClientRepository.FindNameAsync(name);

            var clientsDto = _mapper.Map<CategoriaDTO>(clients);

            return (clientsDto is null) ? BadRequest($"Não há nenhum cliente cadastrado com o nome ou sobrenome {name}") : Ok(clientsDto);
        }

        [HttpPost]
        public ActionResult Post(ClienteDTO clientDto)
        {
            

            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"======== MODELO INVALIDO ============");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Verifique os dados");
            }


            _logger.LogInformation($"======== CLIENTE CADASTRADO! ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var client = _mapper.Map<Cliente>(clientDto);
            var dataValidation = _unitOfWork.ClientRepository.dataValidation(client);
            
            if (dataValidation)
            {
                return BadRequest("Há um usuario cadastro, favor verifique os dados.");
            }

            var clientCreated = _unitOfWork.ClientRepository.Create(client);
            _unitOfWork.Commit();
            
            var clientDtoCreated = _mapper.Map<ClienteDTO>(clientCreated);

            return CreatedAtRoute("ObterCliente", new { id = clientDtoCreated.ClienteId }, clientDtoCreated);
        }

        [HttpPut]
        public ActionResult Put(int id, ClienteDTO clientDto) 
        {

            _logger.LogInformation($"======== CLIENTE ATUALICADO! = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");
        
            if (clientDto == null || id != clientDto.ClienteId)
            {
                _logger.LogInformation($"======== CLIENTE COM PROBLEMA! = {id} ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Verifique os dados.");
            }

            var client = _mapper.Map<Cliente>(clientDto);
            _unitOfWork.ClientRepository.Update(client);
            _unitOfWork.Commit();
            
            return Ok("Cliente modoficado!!!");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"======== CLIENTE DELETADO, ID = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var cliente = _unitOfWork.ClientRepository.GetCliente(id);

            if(cliente == null)
            {
                _logger.LogInformation($"======== PROBLEMA COM ID DO CLIENTE ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Não podemos exluir, pois esse usuário não existe...");
            }

            _unitOfWork.ClientRepository.Delete(cliente);
            _unitOfWork.Commit();

            return Ok($"Cliente com id = {id} removido!");
        }


    }
}
