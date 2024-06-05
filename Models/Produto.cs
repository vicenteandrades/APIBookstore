using APIBookstore.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBookstore.Models;
[Table("Produtos")]
public class Produto
{
    [Key]
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

    [Required(ErrorMessage = "O campo de Quantidade é obrigatório")]
    [ValorMinimoEstoque]
    public int Quantidade { get; set; }


}

