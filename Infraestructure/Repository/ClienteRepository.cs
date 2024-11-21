using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiClientes.Application.DTOs;
using ServiClientes.Data;
using ServiClientes.Model;
using ServiClientes.Shared;

namespace ServiClientes.Infraestructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDBContext _context;
        private readonly PaginationSettings _pagination;

        public ClienteRepository(AppDBContext context, IOptions<PaginationSettings> pagination) 
        { 
            _context = context;
            _pagination = pagination.Value;
        }

        public async Task<Result<Cliente>> AddItem(Cliente item)
        {
            await _context.Clientes.AddAsync(item);
            await _context.SaveChangesAsync();
            return Result<Cliente>.Success(item);
        }

        public async Task<Result<Cliente>> DeleteItem(int id)
        {
            var resultado = await _context.Clientes.FirstOrDefaultAsync(x=>x.IdCliente==id);
            if (resultado != null)
            {
                _context.Clientes.Remove(resultado);
                await _context.SaveChangesAsync();
                return Result<Cliente>.Success(resultado);
            }
            return Result<Cliente>.Failure("Error - Cliente no identificado");
        }

        public async Task<bool> EsNumeroDocumentoUnicoAsync(string tipoDocumento, string numeroDocumento, int IdCliente)
        {
            return !await _context.Clientes
                .AnyAsync(c => c.TipoDocumento == tipoDocumento
                               && c.NumeroDocumento == numeroDocumento
                               && c.IdCliente != IdCliente);
        }

        public async Task<IEnumerable<Cliente>> GetAll(ClienteFilterDTO filters, string order, int page, int pageSize)
        {

            // Tamaño de pagina
            if (pageSize <= 0)
            { 
                pageSize = _pagination.DefaultPageSize;
            }

            // Recuperacion del query
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(filters.TipoDocumento))
            {
                query = query.Where(x => x.TipoDocumento == filters.TipoDocumento);
            }

            // filtro por NumeroDocumento
            if (!string.IsNullOrEmpty(filters.NumeroDocumento))
            {
                query = query.Where(x => x.NumeroDocumento == filters.NumeroDocumento);
            }
            // filtro por Apellidos que comienzan con cadena (like)
            if (!string.IsNullOrEmpty(filters.Apellidos))
            {
                query = query.Where(x => x.Apellidos.StartsWith(filters.Apellidos));
            }

            // filtro por Genero
            if (!string.IsNullOrEmpty(filters.Genero)) 
            { 
                query = query.Where(x=>x.Genero==filters.Genero);
            }

            // Ordenamiento
            if (order == "apellido_nombre")
            {
                query = query.OrderBy(c => c.Apellidos).ThenBy(c => c.Nombres);
            }
            else if (order == "numero_documento")
            {
                query = query.OrderBy(c => c.NumeroDocumento);
            }
            else if(order =="fecha_nacimiento")
            {
                query = query.OrderBy(c=>c.FechaNacimiento);
            }

            var resultado = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c=> new Cliente
                {
                    IdCliente = c.IdCliente,
                    TipoDocumento = c.TipoDocumento,
                    NumeroDocumento = c.NumeroDocumento,
                    Apellidos = c.Apellidos,
                    Nombres = c.Nombres,
                    FechaNacimiento = c.FechaNacimiento,
                    Genero = c.Genero
                })
                .ToListAsync();

            return resultado;

        }
        public async Task<Result<Cliente>> GetIteById(int id)
        {
            var resultado = await _context.Clientes.FirstOrDefaultAsync(x=>x.IdCliente == id);
            return Result<Cliente>.Success(resultado);
        }

        public async Task<Result<Cliente>> UpdateItem(Cliente item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result<Cliente>.Success(item);
        }
    }
}
