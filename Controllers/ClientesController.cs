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
    public class ClientesController : ControllerBase
    {
        private readonly BookstoreContext Context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(BookstoreContext context, ILogger<ClientesController> logger)
        {
            Context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAsync()
        {
            _logger.LogInformation($"======== GET CLIENTES ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");


            var clientes = await Context.Clientes.ToListAsync();

            return (clientes is null) ? NotFound("Não há clientes cadastrados.") : Ok(clientes);
        }

        [HttpGet("Pedidos")]
        public async Task <ActionResult<IEnumerable<Cliente>>> GetPedidosAsync()
        {

            _logger.LogInformation($"======== GET CLIENTES / PEDIDOS ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

            var clientesComPedidos = await Context.Clientes
                .Include(x => x.Pedidos)
                .Where(x => x.Pedidos.Any())
                .ToListAsync();

            if (clientesComPedidos.Count == 0)
            {
                _logger.LogInformation($"======== PEDIDOS INDISPONIVEIS ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");


                return NotFound("Não há pedidos realizados.");
            }

            return Ok(clientesComPedidos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCliente")]
        public async Task <ActionResult<Cliente>> GetAsync(int id)
        {
            _logger.LogInformation($"======== GET CLIENTES ID = {id} ============");

            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

            var cliente = await Context.Clientes.SingleOrDefaultAsync(c => c.ClienteId == id);

            return (cliente == null) ? NotFound($"Não há nenhum cliente com o id = {id}") : Ok(cliente);
        }

        [HttpGet("{name}")]

        public async Task <ActionResult<Cliente>> GetAsync(string name)
        {
            _logger.LogInformation($"======== GET CLIENTES NAME = {name} ============");

            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

            var list = await Context.Clientes.ToListAsync();

            var clientes = list
                           .Where(c => c.Nome.ToLower().Contains(name.ToLower()))
                           .ToList();

            return (clientes.Count == 0) ? BadRequest($"Não há nenhum cliente cadastrado com o nome ou sobrenome {name}") : Ok(clientes);
        }

        [HttpPost]
        public ActionResult Post(Cliente cliente)
        {
            

            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"======== MODELO INVALIDO ============");
                return BadRequest("Verifique os dados");
            }

            
            try 
            {

                _logger.LogInformation($"======== CLIENTE CADASTRADO! ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

                var validacaoDeDados = Context
                                        .Clientes
                                        .Any(c => c.Cpf.Equals(cliente.Cpf) || c.Email.Equals(cliente.Email));

                if (validacaoDeDados)
                {
                    return BadRequest("Há um usuario cadastro, favor verifique os dados.");
                }

                Context.Clientes.Add(cliente);
                Context.SaveChanges();

                return CreatedAtRoute("ObterCliente", new { id = cliente.ClienteId }, cliente);

            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao processar a requisição: {ex.Message}");
                return StatusCode(500, "Erro ao processar a requisição. Tente novamente mais tarde.");
            }
        }

        [HttpPut]
        public ActionResult Put(int id, Cliente cliente) 
        {

            _logger.LogInformation($"======== CLIENTE ATUALICADO! = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

            if (cliente == null || id != cliente.ClienteId)
            {
                _logger.LogInformation($"======== CLIENTE COM PROBLEMA! = {id} ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                return BadRequest("Verifique os dados.");
            }

            Context.Entry(cliente).State = EntityState.Modified;
            Context.SaveChanges();

            return Ok("Cliente modoficado!!!");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"======== CLIENTE DELETADO, ID = {id} ============");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            var cliente = Context.Clientes.SingleOrDefault(x => x.ClienteId == id);

            if(cliente == null)
            {
                _logger.LogInformation($"======== PROBLEMA COM ID DO CLIENTE ============");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                return BadRequest("Não podemos exluir, pois esse usuário não existe...");
            }

            Context.Clientes.Remove(cliente);
            Context.SaveChanges();

            return Ok($"Cliente ({cliente.Nome}) com id = {id} removido!");
        }


    }
}
