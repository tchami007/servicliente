using System.ComponentModel.DataAnnotations;

namespace ServiClientes.Application.DTOs
{
    public class ClienteDTO
    {
        [Key]
        public int IdCliente { get; set; }
        [Required]
        public string TipoDocumento { get; set; }
        [Required]
        public string NumeroDocumento { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Genero { get; set; }

    }

}
