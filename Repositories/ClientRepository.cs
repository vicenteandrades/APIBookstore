using APIBookstore.Context;
using APIBookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Repositories;

public class ClientRepository : BaseRepository<Cliente>, IClientRepository
{
    public ClientRepository(BookstoreContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cliente>> FindNameAsync(string model)
    {
        if (string.IsNullOrEmpty(model))
        {
            throw new Exception("string não pode ser null ou vazia");
        }

        return await _context.Clientes.Where(x => x.Nome.ToLower().StartsWith(model.ToLower())).ToListAsync();
    }


    public async Task<IEnumerable<Cliente>> GetOrder()
    {
        return await _context.Clientes.Include(p => p.Pedidos).Where(x => x.Pedidos.Any()).ToListAsync();
    }

    public bool dataValidation(Cliente client)
    {
        return _context.Clientes.Any(c => c.Cpf.Equals(client.Cpf) || c.Email.Equals(client.Email));
    }
}