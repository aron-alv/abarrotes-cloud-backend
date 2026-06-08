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

    }
}
