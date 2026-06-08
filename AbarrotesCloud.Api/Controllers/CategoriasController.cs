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
    public class CategoriasController : ControllerBase
    {

        private readonly AbarrotesDbContext _context;

        public CategoriasController(AbarrotesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias([FromQuery] Guid tiendaId)
        {
            
            if (tiendaId == Guid.Empty) return BadRequest("Se requiere el ID de la tienda.");

           
            return await _context.Categorias
                                 .Where(c => c.Tiendaid == tiendaId)
                                 .ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> CrearCategorias([FromBody] CategoriaCreateDTO dto)
        {
            var CrearCategoria = new Categoria
            {
                Tiendaid = dto.TiendaId,
                Nombre = dto.Nombre
            };
            _context.Categorias.Add(CrearCategoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategorias), new { id = CrearCategoria.Id }, CrearCategoria);


        }



        [HttpDelete("{categoriaId}")]
        public async Task<IActionResult> EliminarCategoria(Guid categoriaId, [FromQuery] Guid tiendaId, [FromQuery] Guid usuarioId)
        {
            try
            {
                if (tiendaId == Guid.Empty || usuarioId == Guid.Empty)
                    return BadRequest("Faltan credenciales de seguridad.");
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == categoriaId && c.Tiendaid == tiendaId);
                if (categoria == null) return NotFound("Categoría no encontrada.");
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría eliminada" });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"------- ERROR REAL DE BD: {errorReal}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar categoría: {errorReal}");
            }
        }

     
        [HttpPut("{categoriaId}")]
        public async Task<IActionResult> ActualizarCategorias(Guid categoriaId, [FromBody] CategoriaUpdateDTO dto, [FromQuery] Guid tiendaId, [FromQuery] Guid usuarioId)
        {
            try
            {
                if (tiendaId == Guid.Empty || usuarioId == Guid.Empty)
                    return BadRequest("Faltan credenciales de seguridad.");
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == categoriaId && c.Tiendaid == tiendaId);
                if (categoria == null) return NotFound("Categoría no encontrada.");
                categoria.Nombre = dto.Nombre;
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría actualizada", categoria });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"------- ERROR REAL DE BD: {errorReal}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar categoría: {errorReal}");
            }



        }

    }

}
