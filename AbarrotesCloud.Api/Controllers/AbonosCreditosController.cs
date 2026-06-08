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
    public class AbonosCreditosController : ControllerBase
    {

        private readonly AbarrotesDbContext _context;

        public AbonosCreditosController(AbarrotesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerAbonosCreditos([FromQuery] Guid tiendaId)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

            var abonosCreditos = await _context.Abonoscreditos
                .Where(a => a.Tiendaid == tiendaId)
                .Include(a => a.Cliente)
                .Include(a => a.Usuario)
                .Select(a => new
                {
                    id = a.Id,
                    tiendaId = a.Tiendaid,
                    clienteId = a.Clienteid,
                    usuarioId = a.Usuarioid,
                    montoAbonado = a.Montoabonado,
                    clienteNombre = a.Cliente.Nombrecompleto,
                    usuarioNombre = a.Usuario.Nombrecompleto,
                    fechaabono = a.Fechaabono
                })
                .ToListAsync();

            return Ok(abonosCreditos);
        }




        [HttpPost]
        public async Task<IActionResult> ProcesarAbonoCompleto([FromBody] AbonoCreditoCreateDTO abonoCreditoDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nuevoAbono = await CrearAbonoCredito(abonoCreditoDTO);
                await DescontarSaldoCliente(abonoCreditoDTO.ClienteId, abonoCreditoDTO.MontoAbonado);
                await transaction.CommitAsync();
                return Ok(new { mensaje = "Abono procesado con éxito", id = nuevoAbono.Id });

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Abono cancelado: {ex.Message}");

            }


        }


        private async Task<Abonoscredito> CrearAbonoCredito(AbonoCreditoCreateDTO dto)
        {
            var nuevoAbono = new Abonoscredito
            {
                Clienteid = dto.ClienteId,
                Tiendaid = dto.TiendaId,
                Usuarioid = dto.UsuarioId,
                Montoabonado = dto.MontoAbonado,
            };

            _context.Abonoscreditos.Add(nuevoAbono);
            await _context.SaveChangesAsync();
            return nuevoAbono;
        }

        private async Task DescontarSaldoCliente(Guid clienteId, decimal montoAbonado)
        {
          
            var cliente = await _context.Clientescreditos.FindAsync(clienteId);

            if (cliente == null)
            {
                throw new Exception("El cliente no existe.");
            }

           
            if (montoAbonado > cliente.Saldoactual)
            {
                throw new Exception($"El cliente solo debe ${cliente.Saldoactual}, no puedes registrar un abono de ${montoAbonado}.");
            }

        
            cliente.Saldoactual -= montoAbonado;

           
            _context.Clientescreditos.Update(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
