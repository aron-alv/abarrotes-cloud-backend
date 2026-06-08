using AbarrotesCloud.Api.Data;
using AbarrotesCloud.Api.DTOs;
using AbarrotesCloud.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AbarrotesDbContext _context; 

    public UsuariosController(AbarrotesDbContext context)
    {
        _context = context;
    }

    [HttpPost("sincronizar")]
    public async Task<IActionResult> SincronizarUsuario([FromBody] UsuarioSyncDTO dto)
    {
        
        var existe = await _context.Usuarios.AnyAsync(u => u.Id == dto.Id);

       
        if (!existe)
        {
            var nuevoUsuario = new Usuario
            {
                Id = dto.Id,
                Tiendaid = Guid.Empty, 
                Nombrecompleto = dto.Email, 
                Rol = "Admin", 
                Pinventa = null 

            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();
        }

        return Ok(new { mensaje = "Usuario sincronizado con SQL Server" });
    }
}


public class UsuarioSyncDTO
{
    public Guid Id { get; set; } 
    public string Email { get; set; }
}