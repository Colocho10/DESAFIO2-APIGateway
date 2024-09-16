using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; } // Este campo se generará automáticamente
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
