using APIBookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Context;
public class BookstoreContext : DbContext
{
    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
    {

    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }


}

