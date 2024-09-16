using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsuariosAPI.Controllers;
using UsuariosAPI.DTOs;
using UsuariosAPI.Models;
using Xunit;

namespace UsuariosAPI.Tests
{
    public class UsuariosControllerTests
    {
        [Fact]
        public async Task PostUsuario_AgregaUsuario_CuandoUsuarioEsValido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var nuevoUsuario = new UsuarioDTO
            {
                Nombre = "Ana",
                Email = "ana@correo.com",
                Contraseña = "12345678",
                RolId = 1
            };

            // Act
            var result = await controller.PostUsuario(nuevoUsuario);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var usuario = Assert.IsType<Usuario>(createdResult.Value);
            Assert.Equal("Ana", usuario.Nombre);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoNombreEsInvalido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var nuevoUsuario = new UsuarioDTO
            {
                Nombre = "A",  // Nombre inválido
                Email = "test@domain.com",
                Contraseña = "12345678",
                RolId = 1
            };

            // Act
            var result = await controller.PostUsuario(nuevoUsuario);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoEmailEsInvalido()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var nuevoUsuario = new UsuarioDTO
            {
                Nombre = "Usuario Válido",
                Email = "invalid-email", // Email inválido
                Contraseña = "12345678",
                RolId = 1
            };

            // Act
            var result = await controller.PostUsuario(nuevoUsuario);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoContraseñaEsInvalida()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var nuevoUsuario = new UsuarioDTO
            {
                Nombre = "Usuario Válido",
                Email = "usuario@domain.com",
                Contraseña = "123",  // Contraseña inválida (menos de 8 caracteres)
                RolId = 1
            };

            // Act
            var result = await controller.PostUsuario(nuevoUsuario);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public async Task GetUsuario_NoDevuelveUsuario_CuandoUsuarioNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Act
            var result = await controller.GetUsuario(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutUsuario_ActualizaUsuario_CuandoDatosSonValidos()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var usuarioActualizado = new UsuarioDTO
            {
                Nombre = "Nombre Actualizado",
                Email = "actualizado@domain.com",
                Contraseña = "87654321",
                RolId = 1
            };

            // Act
            var result = await controller.PutUsuario(1, usuarioActualizado); // ID existente

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutUsuario_NoActualizaUsuario_CuandoUsuarioNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);
            var usuarioActualizado = new UsuarioDTO
            {
                Nombre = "Nombre Actualizado",
                Email = "actualizado@domain.com",
                Contraseña = "87654321",
                RolId = 1
            };

            // Act
            var result = await controller.PutUsuario(999, usuarioActualizado); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUsuario_EliminaUsuario_CuandoUsuarioExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Act
            var result = await controller.DeleteUsuario(1); // ID existente

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUsuario_NoEliminaUsuario_CuandoUsuarioNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Act
            var result = await controller.DeleteUsuario(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
