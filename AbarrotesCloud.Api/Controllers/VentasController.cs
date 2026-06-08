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
    public class VentasController : ControllerBase
    {

        private readonly AbarrotesDbContext _context;

        public VentasController(AbarrotesDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerVentas([FromQuery] Guid tiendaId)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

            var ventas = await _context.Ventas
                .Where(v => v.Tiendaid == tiendaId)
                .Include(v => v.Usuario)
                .Include(v => v.Clientecredito)
                .Select(v => new
                {
                    id = v.Id,
                    tiendaId = v.Tiendaid,
                    usuarioId = v.Usuarioid,
                    clienteCreditoId = v.Clientecreditoid,
                    total = v.Total,
                    metodoPago = v.Metodopago,
                    estatus = v.Estatus,
                    usuarioNombre = v.Usuario.Nombrecompleto,
                    clienteCreditoNombre = v.Clientecredito.Nombrecompleto
                })
                .ToListAsync();
            return Ok(ventas);

        }




        [HttpPost]
        public async Task<IActionResult> ProcesarVentaCompleta([FromBody] VentaCreateDTO ventaDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuevaVenta = await CrearVenta(ventaDTO);
                decimal totalReal = await ProcesarDetallesYStock(nuevaVenta.Id, ventaDTO);

                nuevaVenta.Total = totalReal;

                if (nuevaVenta.Metodopago == "Crédito" && nuevaVenta.Clientecreditoid.HasValue)
                {
                  
                    var cliente = await _context.Clientescreditos.FindAsync(nuevaVenta.Clientecreditoid.Value);

                    if (cliente == null) throw new Exception("El cliente de crédito no existe.");

                   
                    if (cliente.Saldoactual + totalReal > cliente.Limitecredito)
                    {
                        throw new Exception($"La venta supera el límite de crédito del cliente. Límite: ${cliente.Limitecredito}, Saldo actual: ${cliente.Saldoactual}, Intenta comprar: ${totalReal}");
                    }

                  
                    cliente.Saldoactual += totalReal;

                   
                    _context.Clientescreditos.Update(cliente);
                }
          

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Venta procesada con éxito", id = nuevaVenta.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"------- ERROR REAL DE BD: {errorReal}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Venta cancelada: {errorReal}");
            }
        }




        private async Task<Venta> CrearVenta(VentaCreateDTO dto)
        {
         
            var nuevaVenta = new Venta
            {
                Tiendaid = dto.TiendaId,
                Usuarioid = dto.UsuarioId,
                Clientecreditoid = dto.ClienteCreditoId,
                Total = 0,
                Metodopago = dto.MetodoPago,
         
            };
          
         


            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync(); 

            return nuevaVenta;
        }


    

        private async Task<decimal> ProcesarDetallesYStock(Guid ventaId, VentaCreateDTO dto)
        {
            decimal SumaTotal = 0;
            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null) throw new Exception($"El producto no existe.");
                if (producto.Stockactual < item.Cantidad) throw new Exception($"Stock insuficiente para {producto.Nombre}.");

                decimal subtotal = item.Cantidad * producto.Precioventa;
                SumaTotal += subtotal;

                var detalle = new Detalleventa
                {
                    Ventaid = ventaId,
                    Productoid = item.ProductoId,
                    Cantidad = item.Cantidad,
                    Preciounitario = producto.Precioventa,
                    Subtotal = item.Cantidad * producto.Precioventa
                };
                _context.Detalleventas.Add(detalle);

                //Descontar Stock
                producto.Stockactual -= item.Cantidad;
                _context.Productos.Update(producto);
            }

           
            await _context.SaveChangesAsync();
            return SumaTotal;
        }




    }
}
