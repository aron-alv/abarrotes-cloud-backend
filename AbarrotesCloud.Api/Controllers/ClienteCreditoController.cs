using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbarrotesCloud.Api.Data;
using AbarrotesCloud.Api.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AbarrotesCloud.Api.DTOs;

namespace AbarrotesCloud.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteCreditoController : ControllerBase
    {

        private readonly AbarrotesDbContext _context;
        public ClienteCreditoController(AbarrotesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerClientesCreditos([FromQuery] Guid tiendaId)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

            var clientesCredito = await _context.Clientescreditos
                .Where(c => c.Tiendaid == tiendaId)
                .Select(c => new
                {
                    id = c.Id,
                    tiendaId = c.Tiendaid,
                    nombreCompleto = c.Nombrecompleto,
                    telefono = c.Telefono,
                    limiteCredito = c.Limitecredito,
                    saldoActual = c.Saldoactual
                })
                .ToListAsync();

            return Ok(clientesCredito);



        }


        [HttpPost]
        public async Task<IActionResult> CrearClienteCredito([FromBody] ClienteCreateDTO clienteCreditoDTO)
        {

            try
            {
                var nuevoCliente = new Clientescredito
                {
                    Tiendaid = clienteCreditoDTO.TiendaId,
                    Nombrecompleto = clienteCreditoDTO.NombreCompleto,
                    Telefono = clienteCreditoDTO.Telefono,
                    Limitecredito = clienteCreditoDTO.LimiteCredito,
                    Saldoactual = clienteCreditoDTO.SaldoActual
                };
                _context.Clientescreditos.Add(nuevoCliente);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(ObtenerClientesCreditos), new { id = nuevoCliente.Id }, nuevoCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());

            }

        }


        [HttpDelete("{clienteId}")]
        public async Task<IActionResult> EliminarClienteId(Guid clienteId, [FromQuery] Guid tiendaId, [FromQuery] Guid usuarioId)
        {
            try
            {
             
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return Unauthorized("Usuario no reconocido.");
                if (usuario.Rol != "Admin")
                    return StatusCode(403, "Acceso denegado: Solo los administradores pueden borrar productos.");
                if (usuario.Tiendaid != tiendaId)
                    return StatusCode(403, "No tienes permiso para borrar productos de otra tienda.");

                var cliente = await _context.Clientescreditos.FirstOrDefaultAsync(c => c.Id == clienteId && c.Tiendaid == tiendaId);
                if (cliente == null) return NotFound("Cliente no encontrado.");
                _context.Clientescreditos.Remove(cliente);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Cliente eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
