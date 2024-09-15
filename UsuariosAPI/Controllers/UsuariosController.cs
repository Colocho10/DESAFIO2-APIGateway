using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UsuariosAPI.DTOs;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            // Validar Nombre
            if (string.IsNullOrEmpty(usuarioDTO.Nombre) || usuarioDTO.Nombre.Length < 3 || usuarioDTO.Nombre.Length > 50)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            // Validar Email
            var emailValidator = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(usuarioDTO.Email) || !emailValidator.IsValid(usuarioDTO.Email))
            {
                return BadRequest("El email es obligatorio y debe tener un formato válido.");
            }

            // Validar Contraseña
            if (string.IsNullOrEmpty(usuarioDTO.Contraseña) || usuarioDTO.Contraseña.Length < 8)
            {
                return BadRequest("La contraseña es obligatoria y debe tener al menos 8 caracteres.");
            }

            // Crear el nuevo Usuario basado en el DTO
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Email = usuarioDTO.Email,
                Contraseña = usuarioDTO.Contraseña,
                RolId = usuarioDTO.RolId
            };

            // Agregar usuario a la base de datos
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // GET: api/Usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Actualizar el usuario con los datos del DTO
            usuario.Nombre = usuarioDTO.Nombre;
            usuario.Email = usuarioDTO.Email;
            usuario.Contraseña = usuarioDTO.Contraseña;
            usuario.RolId = usuarioDTO.RolId;

            // Marcar la entidad como modificada
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
