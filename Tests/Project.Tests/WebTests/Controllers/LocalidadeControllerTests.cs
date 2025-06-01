using Project.Controllers;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.IntegrationTests.Controllers
{
    public class LocalidadeControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public LocalidadeControllerTests(ITestOutputHelper output)
        {
            _output = output;

            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var settings = new Dictionary<string, string>
                        {
                            { "ConfigMongoDb:ConnectionString", "mongodb://localhost:27017" },
                            { "ConfigMongoDb:DatabaseName", "TestsDb" }
                        };
                        config.AddInMemoryCollection(settings.Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value)));
                    });

                    builder.UseContentRoot(Directory.GetCurrentDirectory());
                });

            _client = factory.CreateClient();
        }

        /*************************************************************************************************
         * Index Localidades - Controller - Simulação
         * Teste que garante que o endpoint Index retorna a view com localidades corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Index_DeveRetornarViewComLocalidades()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Index_DeveRetornarViewComLocalidades\n");

                var settings = Options.Create(new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb"
                });

                var controller = new LocalidadeController(settings);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Localidade>>(viewResult.Model);

                _output.WriteLine($"🎯 View retornada com {model.Count} localidades\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
    }
}