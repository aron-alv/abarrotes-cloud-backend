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
    public class TiendasController : ControllerBase
    {
        private readonly AbarrotesDbContext _context;

        public TiendasController(AbarrotesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerTiendas()
        {
            var tiendas = await _context.Tiendas.ToListAsync();
            return Ok(tiendas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTienda([FromBody] TiendaCreateDTO dto)
        { 
          var CrearTienda = new Tienda
          {
              Nombrenegocio = dto.NombreNegocio,
           
          };
            _context.Tiendas.Add(CrearTienda);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerTiendas), new { id = CrearTienda.Id }, CrearTienda);


        }

    }
}
