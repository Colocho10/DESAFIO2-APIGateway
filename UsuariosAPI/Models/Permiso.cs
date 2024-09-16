using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; } // Este campo se generará automáticamente
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
