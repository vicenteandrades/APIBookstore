using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBookstore.Models;

[Table("Categorias")]
public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O campo de nome é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo de URL é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? ImagemUrl { get; set; }


    public ICollection<Produto>? Produtos { get; set; }


    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
}
    
