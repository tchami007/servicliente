using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiClientes.Application.DTOs;
using ServiClientes.Application.Services;
using ServiClientes.Shared;

namespace ServiClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetClienteById(int id)
        {
            var resultado = await _service.GetClienteById(id);

            if (!resultado._success)
            {
                BadRequest($"Error en la consulta : {resultado._errorMessage}");
            }

            // Devuelve 404 si no existe
            if (resultado._value == null)
            {
                return NotFound();
            }

            return Ok(resultado._value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes(
        [FromQuery] ClienteFilterDTO filter,
        [FromQuery] string order = "apellido_nombre",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 0)
        {
            var resultado = await _service.GetAllCliente(filter, order, page, pageSize);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> AddCliente([FromBody] ClienteDTO cliente)
        {
            var resultado = await _service.AddCliente(cliente);

            if (!resultado._success)
            {
                return BadRequest($"Error en el registro :{resultado._errorMessages[0]}");
            }

            return CreatedAtAction("GetClienteById", new { id = cliente.IdCliente }, resultado._value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDTO>> UpdateCliente(int id, [FromBody] ClienteDTO cliente)
        {
            var resultado = await _service.UpdateCliente(id, cliente);

            if (!resultado._success)
            {
                return BadRequest($"Error en la actualizacion : {resultado._errorMessages[0]}");
            }

            // Devuelve 404 si no existe
            if (resultado._value == null)
            {
                return NotFound();
            }

            // Devuelve 204 si fue modificado
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteDTO>> DeleteCliente(int id)
        {
            var resultado = await _service.DeleteCliente(id);

            if (!resultado._success)
            {
                return BadRequest($"Error en la eliminación :{resultado._errorMessage}");
            }

            // Devuelve 404 si no existe
            if (resultado._value == null)
            {
                return NotFound();
            }

            // Devuelve 204 si fue eliminado
            return NoContent();
        }
    }
}
