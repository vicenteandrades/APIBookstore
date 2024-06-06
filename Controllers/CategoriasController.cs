using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly BookstoreContext _context;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(BookstoreContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Produtos")]

        public async Task <ActionResult<IEnumerable<Categoria>>> GetAllProdutosAsync()
        {

            _logger.LogInformation("=====  Get/ Categoria /Produtos =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");


            var categorias = await _context.Categorias.Include(c => c.Produtos).ToListAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");

            var categorias = await _context.Categorias.ToListAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {

            _logger.LogInformation("========= GET CATEGORIAS =========");
            _logger.LogInformation($"========= ID = {id} =========");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");


            var produto = await _context.Categorias.SingleOrDefaultAsync(c => c.CategoriaId == id);

            return (produto is null) ? NotFound($"Não uma categoria cadastrado com o id {id}") : Ok (produto);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"============== Model State INVALIDO ==============");
                    _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                    return BadRequest("Não podemos cadastrar uma categoria nula");
                }

                _logger.LogInformation($"=====  Categoria Cadastrada!!! {categoria} =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");


                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return CreatedAtRoute("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

            } catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao cadastrar categoria, favor verifique os dados");
            }
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(categoria is null || categoria.CategoriaId != id)

            {
                _logger.LogInformation($"=====  VERIFICAÇÂO INVALIDA =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                return NotFound("Verifique os dados");
            }
            _logger.LogInformation($"=====  CATEGORIA ATUALIZADA =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok("Alterado com sucesso.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.SingleOrDefault(c => c.CategoriaId == id);

            if(categoria is null)
            {
                _logger.LogInformation($"=====  DELEÇÃO INVALIDA =====");
                _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
                return NotFound("Não há essa categoria");
            }
            _logger.LogInformation($"=====  DELEÇÂO DO ID {id} =====");
            _logger.LogInformation($"=====  Status Code: {HttpContext.Response.StatusCode} =====");
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok($"A categoria ({categoria.Nome}) e id {id} foi deletada com sucesso!");
        }

    }
}
