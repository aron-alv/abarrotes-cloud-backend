using AbarrotesCloud.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbarrotesCloud.Api.Data;
namespace AbarrotesCloud.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly AbarrotesDbContext _context;

        public ReportesController(AbarrotesDbContext context)
        {
            _context = context;
        }


        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen([FromQuery] Guid tiendaId, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");


            try
            {

                var query = _context.Ventas.Where(v => v.Tiendaid == tiendaId && v.Estatus != "Cancelada");


                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                   
                    var finDiaCompleto = fechaFin.Value.AddDays(1).AddTicks(-1);
                    query = query.Where(v => v.Fechaventa >= fechaInicio && v.Fechaventa <= finDiaCompleto);
                }


                var resumen = new
                {
                    TotalIngresos = await query.SumAsync(v => v.Total),
                    CantidadVentas = await query.CountAsync(),
                    TotalEfectivo = await query.Where(v => v.Metodopago == "Efectivo" || v.Metodopago == "EFECTIVO").SumAsync(v => v.Total),
                    TotalCredito = await query.Where(v => v.Metodopago == "Crédito" || v.Metodopago == "CREDITO").SumAsync(v => v.Total)
                };

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"------- ERROR REAL DE BD: {errorReal}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Venta cancelada: {errorReal}");
            }
        }
        
        [HttpGet("detalles")]
        public async Task<IActionResult> ObtenerDetallesVentas([FromQuery] Guid tiendaId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");


            var finDiaCompleto = fechaFin.AddDays(1).AddTicks(-1);

            var reporte = await _context.Ventas
                .Where(v => v.Tiendaid == tiendaId && v.Fechaventa >= fechaInicio && v.Fechaventa <= finDiaCompleto)
                            .Include(v => v.Usuario)
                .Include(v => v.Clientecredito)
                .OrderByDescending(v => v.Fechaventa) 
                .Select(v => new
                {
                    id = v.Id,
                    total = v.Total,
                    metodoPago = v.Metodopago,
                    estatus = v.Estatus,
                    fecha = v.Fechaventa,
                    usuarioNombre = v.Usuario.Nombrecompleto,
                    clienteCreditoNombre = v.Clientecredito != null ? v.Clientecredito.Nombrecompleto : "Público General"
                })
                .ToListAsync();

            return Ok(reporte);
        }
    }
}