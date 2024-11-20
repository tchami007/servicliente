using ServiClientes.Model;

namespace ServiClientes.Infraestructure.Repository
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<bool> EsNumeroDocumentoUnicoAsync(string tipoDocumento, string numeroDocumento, int clienteId);
    }
}
