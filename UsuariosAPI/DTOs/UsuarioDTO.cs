namespace UsuariosAPI.DTOs
{
    public class UsuarioDTO
    {
        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        // Relacion con Rol (solo el ID del rol)
        public int RolId { get; set; }
    }
}
