using API.Data;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class CursosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CursosControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing"); // 👈 esto activa el entorno de test

                builder.ConfigureServices(services =>
                {
                    // Buscar y eliminar configuración previa de SQL Server
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Configurar EF Core con base de datos en memoria
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    // Crear una instancia del servicio para pre poblar la DB
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    // Agregar datos de prueba
                    var docente = new Docente
                    {
                        Id = 1,
                        Nombres = "Juan",
                        Apellidos = "Pérez",
                        Profesion = "Matemático",
                        Correo = "juan.perez@ejemplo.com"
                    };
                    db.Docentes.Add(docente);

                    db.Cursos.Add(new Curso
                    {
                        Id = 1,
                        Nombre = "Matemáticas",
                        Creditos = 4,
                        HorasSemanal = 5,
                        Ciclo = "I",
                        IdDocente = 1
                    });

                    db.SaveChanges();
                });
            });
        }

        [Fact]
        public async Task GetCursos_DeberiaRetornarListaCursos()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/cursos");

            // Assert
            response.EnsureSuccessStatusCode();

            var cursos = await response.Content.ReadFromJsonAsync<List<object>>();
            Assert.NotNull(cursos);
            Assert.NotEmpty(cursos);
        }
    }
}
