using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }

        // Clave foránea para el Rol
        [ForeignKey("Rol")]
        public int RolId { get; set; }

        // Propiedad de navegación
        public Rol Rol { get; set; }
    }
}
