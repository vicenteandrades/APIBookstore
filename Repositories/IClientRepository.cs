

using APIBookstore.Models;

namespace APIBookstore.Repositories;

public interface IClientRepository : IBaseRepository<Cliente>
{
    Cliente GetCliente(int id);
    Task<IEnumerable<Cliente>> FindNameAsync(string name);
    Task<IEnumerable<Cliente>> GetOrder();
    bool dataValidation(Cliente client);
}