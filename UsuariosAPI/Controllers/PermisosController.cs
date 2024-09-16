using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UsuariosAPI.DTOs;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConnectionMultiplexer _redis;

        public PermisosController(AppDbContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Permisos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Permiso>> GetPermiso(int id)
        {
            var db = _redis.GetDatabase();
            string cacheKey = $"permiso_{id}";
            var permisoCache = await db.StringGetAsync(cacheKey);
            if (!permisoCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Permiso>(permisoCache);
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(permiso), TimeSpan.FromMinutes(10));
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

            var permiso = new Permiso
            {
                Nombre = permisoDTO.Nombre,
                Descripcion = permisoDTO.Descripcion
            };

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            // Invalidar cache de lista de permisos
            var db = _redis.GetDatabase();
            string cacheKeyList = "permisoList";
            await db.KeyDeleteAsync(cacheKeyList);

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

            permiso.Nombre = permisoDTO.Nombre;
            permiso.Descripcion = permisoDTO.Descripcion;

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Invalidar el cache para el permiso individual y la lista de permisos
                var db = _redis.GetDatabase();
                string cacheKeyPermiso = $"permiso_{id}";
                string cacheKeyList = "permisoList";
                await db.KeyDeleteAsync(cacheKeyPermiso);
                await db.KeyDeleteAsync(cacheKeyList);
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

            // Invalidar el cache del permiso y de la lista de permisos
            var db = _redis.GetDatabase();
            string cacheKeyPermiso = $"permiso_{id}";
            string cacheKeyList = "permisoList";
            await db.KeyDeleteAsync(cacheKeyPermiso);
            await db.KeyDeleteAsync(cacheKeyList);

            return NoContent();
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(p => p.Id == id);
        }
    }
}
