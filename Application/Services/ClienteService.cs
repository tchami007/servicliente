using AutoMapper;
using ServiClientes.Application.DTOs;
using ServiClientes.Application.Validators;
using ServiClientes.Infraestructure.Repository;
using ServiClientes.Model;
using ServiClientes.Shared;

namespace ServiClientes.Application.Services
{
    public interface IClienteService 
    {
        Task<Result<ClienteDTO>> AddCliente(ClienteDTO item);
        Task<Result<ClienteDTO>> UpdateCliente(int id, ClienteDTO item);
        Task<Result<ClienteDTO>> DeleteCliente(int id);
        Task<Result<ClienteDTO>> GetClienteById(int id);
        Task<IEnumerable<ClienteDTO>> GetAllCliente(ClienteFilterDTO filter, string order, int page, int pageSize);
    }
    public class ClienteService :IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;
        private readonly ClienteValidator _validator;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper, ClienteValidator validator)
        { 
            _repository = clienteRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<ClienteDTO>> AddCliente(ClienteDTO item)
        {
            var validationResult = await _validator.ValidateAsync(item);
            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);

                return Result<ClienteDTO>.Failure(errors);
            }

            Cliente nuevoCliente = _mapper.Map<Cliente>(item);

            var resultado = await _repository.AddItem(nuevoCliente);

            if (!resultado._success)
            {
                var error = resultado._errorMessage;
                return Result<ClienteDTO>.Failure($"Error de Base de datos: {error}");
            }
            
            item = _mapper.Map<ClienteDTO>(nuevoCliente);

            return Result<ClienteDTO>.Success(item);
        }

        public async Task<Result<ClienteDTO>> DeleteCliente(int id)
        {
            ClienteDTO clienteConsulta = new ClienteDTO();
            var resultado = await _repository.GetIteById(id);

            if (!resultado._success)
            {
                return Result<ClienteDTO>.Failure(resultado._errorMessage);
            }

            resultado = await _repository.DeleteItem(id);

            if (!resultado._success)
            {
                var error = resultado._errorMessage;
                return Result<ClienteDTO>.Failure(error);
            }

            return Result<ClienteDTO>.Success(clienteConsulta);
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllCliente(ClienteFilterDTO filter, string order, int page, int pageSize)
        {
            var retorno = await _repository.GetAll(filter, order, page, pageSize);

            var resultado = _mapper.Map<IEnumerable<ClienteDTO>>(retorno);

            return resultado;
        }

        public async Task<Result<ClienteDTO>> GetClienteById(int id)
        {
            var retorno = await _repository.GetIteById(id);

            if (!retorno._success)
            {
                return Result<ClienteDTO>.Failure(retorno._errorMessage);
            }

            var resultado = _mapper.Map<ClienteDTO>(retorno._value);

            return Result<ClienteDTO>.Success(resultado);
        }

        public async Task<Result<ClienteDTO>> UpdateCliente(int id, ClienteDTO item)
        {
            if (id != item.IdCliente)
            {
                return Result<ClienteDTO>.Failure("El identificador del item a modificador no se corresponde.");
            }

            var validationResult = _validator.Validate(item);
            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);

                return Result<ClienteDTO>.Failure(errors);
            }

            Cliente aModificar = new Cliente();
            aModificar = _mapper.Map<Cliente>(item);

            var resultado = await _repository.UpdateItem(aModificar);

            if (!resultado._success) 
            {
                return Result<ClienteDTO>.Failure(resultado._errorMessage);
            }

            return Result<ClienteDTO>.Success(_mapper.Map<ClienteDTO>(resultado._value));

        }
    }
}
