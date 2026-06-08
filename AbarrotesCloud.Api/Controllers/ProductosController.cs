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
    public class ProductosController : ControllerBase
    {
        private readonly AbarrotesDbContext _context;

        public ProductosController(AbarrotesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos([FromQuery] Guid tiendaId)
        {
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

            return await _context.Productos
                                 .Where(p => p.Tiendaid == tiendaId)
                                 .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoCreateDTO dto)
        {


            var nuevoProducto = new Producto
            {
                Tiendaid = dto.TiendaId,
                Categoriaid = dto.CategoriaId,
                Codigobarras = dto.CodigoBarras,
                Nombre = dto.Nombre,
                Preciocompra = dto.PrecioCompra,
                Precioventa = dto.PrecioVenta,
                Tipounidad = dto.TipoUnidad,
                Stockactual = 0, 
                Stockminimo = 0

            };
            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductos), new { id = nuevoProducto.Id }, nuevoProducto);
        }



        [HttpDelete("{productoId}")]
        public async Task<IActionResult> EliminarProducto(Guid productoId, [FromQuery] Guid tiendaId, [FromQuery] Guid usuarioId)
        {
            try
            {

                if (tiendaId == Guid.Empty || usuarioId == Guid.Empty)
                    return BadRequest("Faltan credenciales de seguridad.");

                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return Unauthorized("Usuario no reconocido.");

                if (usuario.Rol != "Admin")
                    return StatusCode(403, "Acceso denegado: Solo los administradores pueden borrar productos.");

             
                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null)
                    return NotFound("Producto no encontrado.");


                if (producto.Tiendaid != tiendaId)
                    return StatusCode(403, "No tienes permiso para borrar productos de otra tienda.");


                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"No se pudo eliminar ya que existen ventas con ese producto");
            }
        }

       
        [HttpPut("{productoId}")]
  
        public async Task<IActionResult> ActualizarProducto(Guid productoId, [FromQuery] Guid tiendaId, [FromQuery] Guid usuarioId, [FromBody] ProductoUpdateDTO dto)
        {
            try
            {
             
                if (tiendaId == Guid.Empty || usuarioId == Guid.Empty)
                    return BadRequest("Faltan credenciales de seguridad.");

                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return Unauthorized("Usuario no reconocido.");

                if (usuario.Rol != "Admin")
                    return StatusCode(403, "Acceso denegado: Solo los administradores pueden actualizar productos.");

                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null)
                    return NotFound("Producto no encontrado.");

                if (producto.Tiendaid != tiendaId)
                    return StatusCode(403, "No tienes permiso para actualizar productos de otra tienda.");

                
                producto.Categoriaid = dto.CategoriaId;
                
                producto.Codigobarras = string.IsNullOrWhiteSpace(dto.CodigoBarras) ? null : dto.CodigoBarras;
                producto.Nombre = dto.Nombre;
                producto.Preciocompra = dto.PrecioCompra;
                producto.Precioventa = dto.PrecioVenta;
                producto.Tipounidad = dto.TipoUnidad;
                producto.Stockminimo = dto.StockMinimo;

                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Producto actualizado correctamente", producto });
            }
            catch (Exception ex)
            {
               
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"------- ERROR REAL DE BD: {errorReal}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Venta cancelada: {errorReal}");
            }
        }
    }
}
