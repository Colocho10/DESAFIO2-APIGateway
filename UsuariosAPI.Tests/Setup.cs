using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

namespace UsuariosAPI.Tests
{
    public static class Setup
    {
        public static AppDbContext GetInMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UsuariosTestDB")
                .Options;

            var context = new AppDbContext(options);

            // Agregar datos iniciales para pruebas de Usuario
            if (!context.Usuarios.Any())
            {
                context.Usuarios.Add(new Usuario
                {
                    Id = 1,
                    Nombre = "Usuario Test",
                    Email = "test@domain.com",
                    Contraseña = "12345678",
                    RolId = 1 // Asociado a un rol
                });
                context.SaveChanges();
            }

            // Agregar datos iniciales para pruebas de Rol
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Rol
                {
                    Id = 1,
                    Nombre = "Admin",
                    Descripcion = "Administrador del sistema",
                    PermisoId = 1
                });

                context.Roles.Add(new Rol
                {
                    Id = 2,
                    Nombre = "Editor",
                    Descripcion = "Editor de contenido",
                    PermisoId = 2
                });

                context.SaveChanges();
            }

            // Agregar datos iniciales para pruebas de Permiso
            if (!context.Permisos.Any())
            {
                context.Permisos.Add(new Permiso
                {
                    Id = 1,
                    Nombre = "Todos los permisos",
                    Descripcion = "Acceso completo al sistema"
                });

                context.Permisos.Add(new Permiso
                {
                    Id = 2,
                    Nombre = "Permisos limitados",
                    Descripcion = "Acceso restringido a ciertas funcionalidades"
                });

                context.SaveChanges();
            }

            return context;
        }
    }
}
