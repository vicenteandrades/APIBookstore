using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using APIBookstore.Models;
using APIBookstore.Repositories;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(UnitOfWork unitOfWork, ILogger<ClientesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAsync()
        {
            _logger.LogInformation($"======== GET CLIENTES ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var clientes = await _unitOfWork.ClientRepository.GetAllAsync();

            return (clientes is null) ? NotFound("Não há clientes cadastrados.") : Ok(clientes);
        }

        [HttpGet("Pedidos")]
        public async Task <ActionResult<IEnumerable<Cliente>>> GetPedidosAsync()
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

            return Ok(clientsWithOrder);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCliente")]
        public async Task <ActionResult<Cliente>> GetAsync(int id)
        {
            _logger.LogInformation($"======== GET CLIENTES ID = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");


            var cliente = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            return (cliente == null) ? NotFound($"Não há nenhum cliente com o id = {id}") : Ok(cliente);
        }

        [HttpGet("{name}")]

        public async Task <ActionResult<Cliente>> GetAsyncName(string name)
        {
            _logger.LogInformation($"======== GET CLIENTES NAME = {name} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            var clients = await _unitOfWork.ClientRepository.FindNameAsync(name);

            return (clients is null) ? BadRequest($"Não há nenhum cliente cadastrado com o nome ou sobrenome {name}") : Ok(clients);
        }

        [HttpPost]
        public ActionResult Post(Cliente cliente)
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


            var dataValidation = _unitOfWork.ClientRepository.dataValidation(cliente);

            if (dataValidation)
            {
                return BadRequest("Há um usuario cadastro, favor verifique os dados.");
            }

            _unitOfWork.ClientRepository.Create(cliente);
            _unitOfWork.Commit();

            return CreatedAtRoute("ObterCliente", new { id = cliente.ClienteId }, cliente);
        }

        [HttpPut]
        public ActionResult Put(int id, Cliente cliente) 
        {

            _logger.LogInformation($"======== CLIENTE ATUALICADO! = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _logger.LogInformation($"Horário: {DateTime.Now}");

            if (cliente == null || id != cliente.ClienteId)
            {
                _logger.LogInformation($"======== CLIENTE COM PROBLEMA! = {id} ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                _logger.LogInformation($"Horário: {DateTime.Now}");

                return BadRequest("Verifique os dados.");
            }

            _unitOfWork.ClientRepository.Update(cliente);
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
