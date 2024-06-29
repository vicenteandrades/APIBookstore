using System.ComponentModel.DataAnnotations;
using APIBookstore.Validation;

namespace APIBookstore.DTOs;

public class ProdutoDTOUpdateRequest
{
    [Required(ErrorMessage = "O campo de Quantidade é obrigatório")]
    [ValorMinimoEstoque]
    public int Quantidade { get; set; }
}