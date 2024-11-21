using AutoMapper;
using ServiClientes.Application.DTOs;
using ServiClientes.Model;

namespace ServiClientes.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<Cliente,ClienteDTO>().ReverseMap();
        }
    }
}
