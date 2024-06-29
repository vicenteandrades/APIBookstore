using System.ComponentModel.DataAnnotations;

namespace APIBookstore.DTOs;

public class CategoriaDTO
{
    [Required]
    public int CategoriaId { get; set; }
    
    [Required(ErrorMessage = "O campo de nome é obrigatório", AllowEmptyStrings = false)]
    [StringLength(80, ErrorMessage = "Comportamos apenas 80 caracteres")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "O campo de URL é obrigatório", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Comportamos apenas 200 caracteres")]
    public string? ImagemUrl { get; set; }
}