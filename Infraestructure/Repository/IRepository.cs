using ServiClientes.Application.DTOs;
using ServiClientes.Shared;

namespace ServiClientes.Infraestructure.Repository
{
    public interface IRepository<T>
    {
        Task<Result<T>> AddItem (T item);
        Task<Result<T>> UpdateItem (T item);
        Task<Result<T>> DeleteItem(int id);
        Task<Result<T>> GetIteById(int id);
        Task<IEnumerable<T>> GetAll(ClienteFilterDTO filters, string order, int page, int pageSize);
    }
}
