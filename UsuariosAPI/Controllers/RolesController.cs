using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol([FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest("El rol no puede ser nulo.");
            }

            var validationErrors = new List<string>();

            // Validar Nombre
            if (string.IsNullOrEmpty(rol.Nombre) || rol.Nombre.Length < 3 || rol.Nombre.Length > 30)
            {
                validationErrors.Add("El campo 'Nombre' es obligatorio y debe tener entre 3 y 30 caracteres.");
            }

            // Validar Descripción (opcional)
            if (rol.Descripcion != null && rol.Descripcion.Length > 100)
            {
                validationErrors.Add("El campo 'Descripción', si se proporciona, no debe exceder los 100 caracteres.");
            }

            // Si hay errores de validación, devolver un mensaje con todos los errores
            if (validationErrors.Count > 0)
            {
                return BadRequest(new { Errores = validationErrors });
            }

            // Agregar el nuevo rol
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, [FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest("El rol no puede ser nulo.");
            }

            if (id != rol.Id)
            {
                return BadRequest("El ID del rol no coincide.");
            }

            // Validar Nombre
            if (string.IsNullOrEmpty(rol.Nombre) || rol.Nombre.Length < 3 || rol.Nombre.Length > 30)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 30 caracteres.");
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // DELETE: api/Roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }
    }
}
