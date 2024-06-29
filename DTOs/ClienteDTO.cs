using System.ComponentModel.DataAnnotations;
using APIBookstore.Validation;


namespace APIBookstore.DTOs;

public class ClienteDTO
{
    [Required]
    public int ClienteId { get; set; }
    
    [Required(ErrorMessage = "Campo de nome é obrigatório!", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "O campo comporta apenas 200 caracteres")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "Campo de CPF é obrigatório!", AllowEmptyStrings = false)]
    [DigitosCpf]
    public string? Cpf { get; set; }

    [Required(ErrorMessage = "Campo de Email é obrigatório!", AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "O campo comporta apenas 200 caracteres")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Campo de Senha é obrigatório!", AllowEmptyStrings = false)]
    [StringLength(80, ErrorMessage = "O campo comporta apenas 200 caracteres")]
    public string? Senha { get; set; }
    
}