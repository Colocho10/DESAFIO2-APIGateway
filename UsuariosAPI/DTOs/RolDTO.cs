namespace UsuariosAPI.DTOs
{
    public class RolDTO
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        // Relacion con Permiso (solo el ID del permiso)
        public int PermisoId { get; set; }
    }
}
