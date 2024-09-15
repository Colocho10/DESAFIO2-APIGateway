using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.DTOs;
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
            var rol = await _context.Roles.Include(r => r.Permiso).FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol([FromBody] RolDTO rolDTO)
        {
            if (rolDTO == null)
            {
                return BadRequest("El rol no puede ser nulo.");
            }

            // Validar Nombre
            if (string.IsNullOrEmpty(rolDTO.Nombre) || rolDTO.Nombre.Length < 3 || rolDTO.Nombre.Length > 30)
            {
                return BadRequest("El campo 'Nombre' es obligatorio y debe tener entre 3 y 30 caracteres.");
            }

            // Validar Descripción (opcional)
            if (rolDTO.Descripcion != null && rolDTO.Descripcion.Length > 100)
            {
                return BadRequest("El campo 'Descripción', si se proporciona, no debe exceder los 100 caracteres.");
            }

            // Crear el nuevo Rol basado en el DTO
            var rol = new Rol
            {
                Nombre = rolDTO.Nombre,
                Descripcion = rolDTO.Descripcion,
                PermisoId = rolDTO.PermisoId
            };

            // Agregar el nuevo rol
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, [FromBody] RolDTO rolDTO)
        {
            if (!RolExists(id))
            {
                return NotFound();
            }

            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            // Actualizar el rol con los datos del DTO
            rol.Nombre = rolDTO.Nombre;
            rol.Descripcion = rolDTO.Descripcion;
            rol.PermisoId = rolDTO.PermisoId;

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
