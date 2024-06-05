using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly BookstoreContext Context;

        public ClientesController(BookstoreContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAsync()
        {
            var clientes = await Context.Clientes.ToListAsync();

            return (clientes is null) ? NotFound("Não há clientes cadastrados.") : Ok(clientes);
        }

        [HttpGet("Pedidos")]
        public async Task <ActionResult<IEnumerable<Cliente>>> GetPedidosAsync()
        {
            var clientesComPedidos = await Context.Clientes
                .Include(x => x.Pedidos)
                .Where(x => x.Pedidos.Any())
                .ToListAsync();

            if (clientesComPedidos.Count == 0)
            {
                return NotFound("Não há pedidos realizados.");
            }

            return Ok(clientesComPedidos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCliente")]
        public async Task <ActionResult<Cliente>> GetAsync(int id)
        {
            var cliente = await Context.Clientes.SingleOrDefaultAsync(c => c.ClienteId == id);

            return (cliente == null) ? NotFound($"Não há nenhum cliente com o id = {id}") : Ok(cliente);
        }

        [HttpGet("{name}")]

        public async Task <ActionResult<Cliente>> GetAsync(string name)
        {
            var list = await Context.Clientes.ToListAsync();

            var clientes = list
                           .Where(c => c.Nome.ToLower().Contains(name.ToLower()))
                           .ToList();

            return (clientes.Count == 0) ? BadRequest($"Não há nenhum cliente cadastrado com o nome ou sobrenome {name}") : Ok(clientes);
        }

        [HttpPost]
        public ActionResult Post(Cliente cliente)
        {   

            if(!ModelState.IsValid)
            {
                return BadRequest("Verifique os dados patrão");
            }

            
            try 
            {
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
            if (cliente == null || id != cliente.ClienteId)
            {
                return BadRequest("Verifique os dados.");
            }

            Context.Entry(cliente).State = EntityState.Modified;
            Context.SaveChanges();

            return Ok("Cliente modoficado!!!");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var cliente = Context.Clientes.SingleOrDefault(x => x.ClienteId == id);

            if(cliente == null)
            {
                return BadRequest("Não podemos exluir, pois esse usuário não existe...");
            }

            Context.Clientes.Remove(cliente);
            Context.SaveChanges();

            return Ok($"Cliente ({cliente.Nome}) com id = {id} removido!");
        }


    }
}
