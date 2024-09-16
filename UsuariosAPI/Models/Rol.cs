using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosAPI.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; } // Este campo se generará automáticamente
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PermisoId { get; set; }
        public Permiso Permiso { get; set; }
    }
}
