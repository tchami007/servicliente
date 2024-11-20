using FluentValidation;
using ServiClientes.Application.DTOs;
using ServiClientes.Application.Validators.Enums;
using ServiClientes.Infraestructure.Repository;

namespace ServiClientes.Application.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteValidator(IClienteRepository clienteRepository) 
        {

            _clienteRepository = clienteRepository;

            string caracteresValidos = "^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+(\\s[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+)*$";

            // Control de Vacios

            RuleFor(x => x.TipoDocumento).NotEmpty().WithMessage("El tipo de documento no puede ser vacio");
            RuleFor(x => x.NumeroDocumento).NotEmpty().WithMessage("El Numero de documento no puede ser vacio");
            RuleFor(x => x.Apellidos).NotEmpty().WithMessage("El/los Apellido/s no puede ser vacio");
            RuleFor(x => x.Nombres).NotEmpty().WithMessage("El/los Nombre/s no puede ser vacio");
            RuleFor(x => x.FechaNacimiento ).NotEmpty().WithMessage("La fecha de nacimiento no puede ser vacio");
            RuleFor(x => x.Genero).NotEmpty().WithMessage("El genero no puede ser vacio");

            // Control de valores validos

            RuleFor(x => x.Apellidos)
                .Matches(caracteresValidos)
                .WithMessage("Apellidos solo acepta caracteres validos del alfabeto latino");

            RuleFor(x => x.Nombres)
                .Matches(caracteresValidos)
                .WithMessage("Nombres solo acepta caracteres validos del alfabeto latino");

            RuleFor(x => x.TipoDocumento)
                .Must(Tipo => Enum.TryParse<TipoDocumentoValidos>(Tipo, true, out _))
                .WithMessage("El tipo de documento debe ser uno de los tipos validos");

            RuleFor(x => x.NumeroDocumento)
                .Matches("^[0-9]*$")
                .WithMessage("El numero de documento debe conformarse de digitos del 0 al 9");

            RuleFor(x => x.FechaNacimiento)
                .Must(fecha => fecha <= DateTime.Today.AddYears(-18))
                .WithMessage("El cliente debe ser mayor de 18 años")
                .Must(fecha => fecha >= DateTime.Today.AddYears(-65))
                .WithMessage("El cliente debe ser menor de 65 años");

            RuleFor(x => x.Genero)
                .Must(genero => Enum.TryParse<GeneroValidos>(genero, true, out _))
                .WithMessage("El genero debe ser uno de los admitidos: M, F, O");

            // Control de longitudes

            RuleFor(x => x.NumeroDocumento).MaximumLength(11).WithMessage("El numero de documento adminite hasta 11 digitos");
            RuleFor(x=> x.Apellidos).MaximumLength(100).WithMessage("Apellicos adminite hasta 100 caracteres");
            RuleFor(x => x.Nombres).MaximumLength(100).WithMessage("Nombres adminite hasta 100 caracteres");
            RuleFor(x => x.Genero).MaximumLength(1).WithMessage("Genero admite 1 caracter");

            // Control de NumeroDocumento Duplicado
            RuleFor(x => x.NumeroDocumento)
                .MustAsync(async (cliente, numeroDocumento, cancellation) =>
                     await clienteRepository.EsNumeroDocumentoUnicoAsync(cliente.TipoDocumento, numeroDocumento, cliente.IdCliente))
                .WithMessage("El numero de documento ya esta registrado para ese tipo de documento");


        }
    }
}
