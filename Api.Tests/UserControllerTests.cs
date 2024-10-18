using API.Controllers;
using Data.DbContext_Conection;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Entidades;
using Moq;
using Moq.EntityFrameworkCore;

namespace Api.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        //private readonly ApplicationDbContext _context;
        private readonly UserController _controller;

        public UserControllerTests()
        {

            // Configuramos las opciones de DbContext para la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Creamos una instancia real de ApplicationDbContext usando las opciones
            var context = new ApplicationDbContext(options);

            // Inicializamos el mock del ApplicationDbContext
            _mockContext = new Mock<ApplicationDbContext>(options);

            // Inicializamos el controlador con el contexto simulado
            _controller = new UserController(context);

            // Sembramos datos de prueba
            SeedData(context);
        }

        private void SeedData(ApplicationDbContext context)
        {
            // Agregamos datos de prueba en el DbContext real usando la base de datos en memoria
            context.Users.AddRange(new List<User>
                {
                    new User { Id = 1, UserName = "User1" },
                    new User { Id = 2, UserName = "User2" }
                });

            context.SaveChanges();
        }


        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithAListOfUsers()
        {
            // Arrange
            var mockUsers = new List<User>
            {
                new User { Id = 1, UserName = "User1" },
                new User { Id = 2, UserName = "User2" }
            }.AsQueryable();

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica que sea un OkObjectResult
            var returnValue = Assert.IsType<List<User>>(okResult.Value); // Verifica que el valor devuelto es una lista de usuarios
            Assert.Equal(2, returnValue.Count); // Verifica que se devuelven 2 usuarios
        }

        [Fact]
        public async Task GetUser_ReturnsUser_Exist()
        {
            // Arrange
            var mockUsers = new User { Id = 1, UserName = "User1" };

            // Act
            var result = await _controller.GetUser(mockUsers.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            result.Should().NotBeNull();
        }


        [Fact]
        public async Task GetUser_ReturnsUser_NotExist()
        {
            // Arrange
            var userId = 5;
            var message = "Usuario no encontrado con ese id en la base de datos";

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var returnValue = Assert.IsType<NotFoundObjectResult>(result.Result); // Verifica que sea un NotFoundObjectResult
            Assert.Equal(message, returnValue.Value);
        }
    }
}