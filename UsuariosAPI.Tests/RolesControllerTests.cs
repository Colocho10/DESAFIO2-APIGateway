using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Controllers;
using UsuariosAPI.Models;
using UsuariosAPI.DTOs;

namespace UsuariosAPI.Tests
{
    public class RolesControllerTests
    {


        [Fact]
        public async Task GetRol_DevuelveNotFound_CuandoRolNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);

            // Act
            var result = await controller.GetRol(999); // ID que no debería existir en la DB

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostRol_CreaNuevoRol_CuandoRolEsValido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);
            var rolDTO = new RolDTO
            {
                Nombre = "Administrador",
                Descripcion = "Rol con todos los permisos",
                PermisoId = 1
            };

            // Act
            var result = await controller.PostRol(rolDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var rol = Assert.IsType<Rol>(createdResult.Value);
            Assert.Equal("Administrador", rol.Nombre);
        }

        [Fact]
        public async Task PostRol_DevuelveBadRequest_CuandoNombreEsInvalido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);
            var rolDTO = new RolDTO
            {
                Nombre = "Ad", // Nombre demasiado corto
                Descripcion = "Rol con permisos",
                PermisoId = 1
            };

            // Act
            var result = await controller.PostRol(rolDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public async Task PutRol_DevuelveNotFound_CuandoRolNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);
            var rolDTO = new RolDTO
            {
                Nombre = "Editor",
                Descripcion = "Rol con permisos limitados",
                PermisoId = 2
            };

            // Act
            var result = await controller.PutRol(999, rolDTO); // ID 999 no existe

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRol_EliminaRol_CuandoRolExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);

            // Act
            var result = await controller.DeleteRol(1); // Eliminamos el rol con ID 1

            // Assert
            Assert.IsType<NoContentResult>(result);
            var rolEliminado = await context.Roles.FindAsync(1);
            Assert.Null(rolEliminado);
        }

        [Fact]
        public async Task DeleteRol_DevuelveNotFound_CuandoRolNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new RolesController(context);

            // Act
            var result = await controller.DeleteRol(999); // ID 999 no existe

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
