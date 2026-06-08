using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbarrotesCloud.Api.Data;
using AbarrotesCloud.Api.Models;
using AbarrotesCloud.Api.DTOs;

namespace AbarrotesCloud.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KardexController : ControllerBase
    {
        private readonly AbarrotesDbContext _context;

        public KardexController(AbarrotesDbContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetKardex([FromQuery] Guid tiendaId)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

          
            var movimientos = await _context.Kardices
                .Where(k => k.Tiendaid == tiendaId)
                .Include(k => k.Producto)
                .Include(k => k.Usuario)
                .Select(k => new
                {
                    id = k.Id,
                    tiendaId = k.Tiendaid,
                    productoId = k.Productoid,
                    tipoMovimiento = k.Tipomovimiento,
                    cantidad = k.Cantidad,
                    motivo = k.Motivo,
                    fechaRegistro = k.Fechamovimiento,
                    productoNombre = k.Producto.Nombre,
                    usuarioNombre = k.Usuario.Nombrecompleto

                })
                .ToListAsync();

            return Ok(movimientos);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] KardexCreateDTO kardexDTO)
        {
           
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
               
                var nuevoKardex = await ProcesarMovimientoInventario(kardexDTO);

              
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Inventario actualizado con éxito", kardexId = nuevoKardex.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

      
        private async Task<Kardex> ProcesarMovimientoInventario(KardexCreateDTO dto)
        {
           
            var producto = await _context.Productos.FindAsync(dto.ProductoId);

            if (producto == null)
            {
                throw new Exception("El producto no existe en la base de datos.");
            }

           
            if (dto.TipoMovimiento == "Entrada")
            {
                producto.Stockactual += dto.Cantidad; 
            }
            else if (dto.TipoMovimiento == "Salida")
            {
                
                if (producto.Stockactual < dto.Cantidad)
                {
                    throw new Exception($"Stock insuficiente. Solo tienes {producto.Stockactual} disponibles de {producto.Nombre}.");
                }
                producto.Stockactual -= dto.Cantidad;
            }
            else
            {
                throw new Exception("El tipo de movimiento solo puede ser 'Entrada' o 'Salida'.");
            }

        
            _context.Productos.Update(producto);

         
            var nuevoKardex = new Kardex
            {
                Tiendaid = dto.TiendaId,
                Productoid = dto.ProductoId,
                Usuarioid = dto.UsuarioId,
                Tipomovimiento = dto.TipoMovimiento,
                Motivo = dto.Motivo,
                Cantidad = dto.Cantidad

            };

            _context.Kardices.Add(nuevoKardex);

            await _context.SaveChangesAsync();

            return nuevoKardex;
        }
    }
}