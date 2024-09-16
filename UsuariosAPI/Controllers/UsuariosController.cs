using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Validar Nombre
            if (string.IsNullOrEmpty(usuario.Nombre) || usuario.Nombre.Length < 3 || usuario.Nombre.Length > 50)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            // Validar Email
            var emailValidator = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(usuario.Email) || !emailValidator.IsValid(usuario.Email))
            {
                return BadRequest("El email es obligatorio y debe tener un formato válido.");
            }

            // Validar Contraseña
            if (string.IsNullOrEmpty(usuario.Contraseña) || usuario.Contraseña.Length < 8)
            {
                return BadRequest("La contraseña es obligatoria y debe tener al menos 8 caracteres.");
            }

            // Agregar usuario a la base de datos
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // GET: api/Usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("El ID del usuario no coincide.");
            }

            // Validar datos de la entidad actualizada (opcional)
            if (string.IsNullOrEmpty(usuario.Nombre) || usuario.Nombre.Length < 3 || usuario.Nombre.Length > 50)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            var emailValidator = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(usuario.Email) || !emailValidator.IsValid(usuario.Email))
            {
                return BadRequest("El email es obligatorio y debe tener un formato válido.");
            }

            if (string.IsNullOrEmpty(usuario.Contraseña) || usuario.Contraseña.Length < 8)
            {
                return BadRequest("La contraseña es obligatoria y debe tener al menos 8 caracteres.");
            }

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
