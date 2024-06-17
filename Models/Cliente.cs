

using APIBookstore.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBookstore.Models;

[Table("Clientes")]
public class Cliente
{
    [Key]
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
    [StringLength(200, ErrorMessage = "O campo comporta apenas 200 caracteres")]
    public string? Senha { get; set; }

    
    public ICollection<Pedido>? Pedidos { get; set; }

    public Cliente()
    {
        Pedidos = new Collection<Pedido>();
    }

}

