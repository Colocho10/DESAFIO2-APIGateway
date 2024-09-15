using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosAPI.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        // Clave foránea para Permiso
        [ForeignKey("Permiso")]
        public int PermisoId { get; set; }

        // Propiedad de navegación
        public Permiso Permiso { get; set; }

        // Relación uno a muchos con Usuarios
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
