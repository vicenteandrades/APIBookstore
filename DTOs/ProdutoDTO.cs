using System.ComponentModel.DataAnnotations;
using APIBookstore.Validation;

namespace APIBookstore.DTOs;

public class ProdutoDTO
{
    [Required]
    public int ProdutoId { get; set; }
    
    [Required(ErrorMessage = "O campo de nome é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O campo de Descrição é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? Descricao { get; set; }

    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O campo de URL é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? ImagemUrl { get; set; }

    [Required(ErrorMessage = "O campo de Preço é obrigatório", AllowEmptyStrings = false)]
    [PrecoMinino]
    public double Preco { get; set; }
}