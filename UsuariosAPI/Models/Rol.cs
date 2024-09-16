using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI.Models
{
    public class Rol
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        // Relación con Permisos
        public string Permisos { get; set; }
    }
}
