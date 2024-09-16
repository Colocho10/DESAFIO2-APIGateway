using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsuariosAPI.Controllers;
using UsuariosAPI.DTOs;
using UsuariosAPI.Models;
using Xunit;

namespace UsuariosAPI.Tests
{
    public class PermisosControllerTests
    {
        [Fact]
        public async Task GetPermiso_DevuelvePermiso_CuandoPermisoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);

            // Act
            var result = await controller.GetPermiso(1); // ID que debería existir en la DB

            // Assert
            var actionResult = Assert.IsType<ActionResult<Permiso>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var permiso = Assert.IsType<Permiso>(okResult.Value);
            Assert.Equal(1, permiso.Id);
        }


        [Fact]
        public async Task GetPermiso_NoDevuelvePermiso_CuandoPermisoNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);

            // Act
            var result = await controller.GetPermiso(999); // ID que no existe en la DB

            // Assert
            var actionResult = Assert.IsType<ActionResult<Permiso>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostPermiso_CreaPermiso_CuandoPermisoEsValido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);
            var nuevoPermiso = new PermisoDTO
            {
                Nombre = "Permiso Test",
                Descripcion = "Descripción del permiso test"
            };

            // Act
            var result = await controller.PostPermiso(nuevoPermiso);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var permiso = Assert.IsType<Permiso>(createdResult.Value);
            Assert.Equal("Permiso Test", permiso.Nombre);
        }

        [Fact]
        public async Task PostPermiso_NoCreaPermiso_CuandoNombreEsInvalido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);
            var nuevoPermiso = new PermisoDTO
            {
                Nombre = "Te", // Nombre demasiado corto
                Descripcion = "Descripción del permiso test"
            };

            // Act
            var result = await controller.PostPermiso(nuevoPermiso);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutPermiso_ActualizaPermiso_CuandoPermisoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);
            var permisoActualizado = new PermisoDTO
            {
                Nombre = "Permiso Actualizado",
                Descripcion = "Descripción actualizada"
            };

            // Act
            var result = await controller.PutPermiso(1, permisoActualizado);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var permiso = await context.Permisos.FindAsync(1);
            Assert.Equal("Permiso Actualizado", permiso.Nombre);
        }

        [Fact]
        public async Task DeletePermiso_EliminaPermiso_CuandoPermisoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);

            // Act
            var result = await controller.DeletePermiso(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var permiso = await context.Permisos.FindAsync(1);
            Assert.Null(permiso);
        }

        [Fact]
        public async Task DeletePermiso_NoEliminaPermiso_CuandoPermisoNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PermisosController(context);

            // Act
            var result = await controller.DeletePermiso(999); // ID que no existe en la DB

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
