using APIBookstore.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace APIBookstore.Models;
[Table("Pedidos")]
public class Pedido
{
    [Key]
    public int PedidoId { get; set; }
    [Required(ErrorMessage = "Precisamos do id do cliente para realizar o pedido!")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Precisamos do id do Produto para realizar o pedido!")]
    public int ProdutoId { get; set; }

    [Required]
    [Range(1,100, ErrorMessage = "Precisamos de ao menos 1 produto para realizar esse pedido!")]
    public int Quantidade { get; set; }

    [Required]
    public DateTime Date { get; set; }
    public string? Situacao { get; set; }
    public double Total { get;  set; }

    public Pedido()
    {
        Date = DateTime.Now;
    }

}

