namespace APIBookstore.DTOs;

public class ProdutoDTOUpdateResponse
{
    public int ProdutoId { get; set; }
    public string? Name { get; set; }
    public string? Descricao { get; set; }
    public string? ImagemUrl { get; set; }
    public double Preco { get; set; }
    public int Quantidade { get; set; }
}