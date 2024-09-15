using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.DTOs;
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

        // POST: api/Permisos
        [HttpPost]
        public async Task<ActionResult<Permiso>> PostPermiso([FromBody] PermisoDTO permisoDTO)
        {
            if (permisoDTO == null)
            {
                return BadRequest("El permiso no puede ser nulo.");
            }

            // Validar Nombre
            if (string.IsNullOrEmpty(permisoDTO.Nombre) || permisoDTO.Nombre.Length < 3 || permisoDTO.Nombre.Length > 50)
            {
                return BadRequest("El campo 'Nombre' es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            // Validar Descripción (opcional)
            if (permisoDTO.Descripcion != null && permisoDTO.Descripcion.Length > 100)
            {
                return BadRequest("El campo 'Descripción' no debe exceder los 100 caracteres.");
            }

            // Crear el nuevo Permiso basado en el DTO
            var permiso = new Permiso
            {
                Nombre = permisoDTO.Nombre,
                Descripcion = permisoDTO.Descripcion
            };

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPermiso), new { id = permiso.Id }, permiso);
        }

        // PUT: api/Permisos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, [FromBody] PermisoDTO permisoDTO)
        {
            if (!PermisoExists(id))
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            // Actualizar el permiso con los datos del DTO
            permiso.Nombre = permisoDTO.Nombre;
            permiso.Descripcion = permisoDTO.Descripcion;

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

        // DELETE: api/Permisos/{id}
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
            return _context.Permisos.Any(p => p.Id == id);
        }
    }
}
