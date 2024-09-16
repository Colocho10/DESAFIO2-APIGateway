using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermisosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permiso>> GetPermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            return permiso;
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Rol>> PostPermiso([FromBody] Permiso permiso)
        {
            if (permiso == null)
            {
                return BadRequest("El permiso no puede ser nulo.");
            }

            var validationErrors = new List<string>();

            // Validar Nombre del permiso
            if (string.IsNullOrEmpty(permiso.Nombre) || permiso.Nombre.Length < 3 || permiso.Nombre.Length > 50)
            {
                validationErrors.Add("El campo 'Nombre' es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            // Validar Descripción (opcional)
            if (permiso.Descripcion != null && permiso.Descripcion.Length > 100)
            {
                validationErrors.Add("El campo 'Descripción', es opcional pero no debe exceder los 100 caracteres.");
            }

            // Si hay errores de validación, devolver un mensaje con todos los errores
            if (validationErrors.Count > 0)
            {
                return BadRequest(new { Errores = validationErrors });
            }

            // Agregar el nuevo rol
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPermiso), new { id = permiso.Id }, permiso);
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, [FromBody] Permiso permiso)
        {
            if (permiso == null)
            {
                return BadRequest("El campo permiso no puede ser nulo.");
            }

            if (id != permiso.Id)
            {
                return BadRequest("El ID del permiso no coincide.");
            }

            // Validar Nombre
            if (string.IsNullOrEmpty(permiso.Nombre) || permiso.Nombre.Length < 3 || permiso.Nombre.Length > 30)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 30 caracteres.");
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(id))
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
        public async Task<IActionResult> DeletePermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(r => r.Id == id);
        }
    }
}
