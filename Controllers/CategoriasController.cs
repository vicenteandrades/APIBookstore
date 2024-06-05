using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIBookstore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly BookstoreContext Context;

        public CategoriasController(BookstoreContext context)
        {
            Context = context;
        }

        [HttpGet("Produtos")]

        public async Task <ActionResult<IEnumerable<Categoria>>> GetAllProdutosAsync()
        {
            var categorias = await Context.Categorias.Include(c => c.Produtos).ToListAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            var categorias = await Context.Categorias.ToListAsync();

            return (categorias is null) ? NotFound("Não há categorias cadastradas") : Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {
            var produto = await Context.Categorias.SingleOrDefaultAsync(c => c.CategoriaId == id);

            return (produto is null) ? NotFound($"Não uma categoria cadastrado com o id {id}") : Ok (produto);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Não podemos cadastrar uma categoria nula");
                }

                Context.Categorias.Add(categoria);
                Context.SaveChanges();
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
                return NotFound("Verifique os dados");
            }

            Context.Entry(categoria).State = EntityState.Modified;
            Context.SaveChanges();

            return Ok("Alterado com sucesso.");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = Context.Categorias.SingleOrDefault(c => c.CategoriaId == id);

            if(categoria is null)
            {
                return NotFound("Não há essa categoria");
            }

            Context.Categorias.Remove(categoria);
            Context.SaveChanges();

            return Ok($"A categoria ({categoria.Nome}) e id {id} foi deletada com sucesso!");
        }

    }
}
