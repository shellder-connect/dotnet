using Project.Controllers;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Project.Tests.IntegrationTests.Controllers
{
    public class AbrigoControllerTests
    {
        private readonly Mock<IAbrigoService> _mockAbrigoService;
        private readonly AbrigoController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public AbrigoControllerTests(ITestOutputHelper output)
        {
            _mockAbrigoService = new Mock<IAbrigoService>();
            _controller = new AbrigoController(_mockAbrigoService.Object);
            _output = output;

            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var settings = new Dictionary<string, string>
                        {
                            { "SomeConfigYouMightNeed", "value" }
                        };
                        config.AddInMemoryCollection(settings.Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value)));
                    });

                    builder.UseContentRoot(Directory.GetCurrentDirectory());
                });



            _client = factory.CreateClient();

        }

        /*************************************************************************************************
         * Cadastrar Abrigo - Controller
         * Teste que garante que o endpoint CadastrarAbrigo cria um novo abrigo corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Abrigo()
        {
            
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Abrigo\n");

                var abrigo = new Abrigo 
                { 
                    Descricao = "Vestuário", 
                
                };

                _mockAbrigoService.Setup(service => service.Criar(It.IsAny<Abrigo>())).ReturnsAsync(abrigo);

                var result = await _controller.CadastrarAbrigo(abrigo);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosAbrigos", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(abrigo.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(abrigo, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 Abrigo criado: Descricao={abrigo.Descricao}\n");
                _output.WriteLine($"📋 Código de status retornado: {createdAtActionResult.StatusCode}\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");

            }
            
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
        

        /*************************************************************************************************
         * Cadastrar Abrigo - Controller
         * Teste que garante que o endpoint CadastrarAbrigo retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarAbrigo_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarAbrigo_DeveRetornarBadRequest\n");

                var abrigo = new Abrigo 
                { 
                    Descricao = "", 
                   
                };

                _controller.ModelState.AddModelError("Descricao", "Descricao é obrigatório");

                var result = await _controller.CadastrarAbrigo(abrigo);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value);
                
                Assert.Equal(400, badRequestResult.StatusCode);

                    _output.WriteLine("❌ Dados inválidos fornecidos.\n");
                    _output.WriteLine($"📋 Código de status retornado: {badRequestResult.StatusCode}\n");
                    _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                    _output.WriteLine("*****************************************************\n");   
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }

        }
        
        
        /*************************************************************************************************
         * Consultar Todos Abrigos - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosAbrigos retorna todos os abrigos corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosAbrigos_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosAbrigos_DeveRetornarOk\n");

                // Arrange
                var abrigos = new List<Abrigo>
                {
                    new Abrigo { Descricao = "Medicamento"},
                    new Abrigo { Descricao = "Vestuário", }
                };

                _mockAbrigoService.Setup(service => service.ConsultarTodos()).ReturnsAsync(abrigos);

                // Act
                var result = await _controller.ConsultarTodosAbrigo();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(abrigos, okResult.Value);

                _output.WriteLine($"🎯 Total de abrigos retornados: {abrigos.Count}\n");
                _output.WriteLine($"📋 Código de status retornado: {okResult.StatusCode}\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
        


        /*************************************************************************************************
         * Consultar Abrigo por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarAbrigoPorId retorna o abrigo correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Abrigo_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Abrigo_Por_Id\n");

                var abrigoId = "67d1f58e08416bb1e4414600";
                var abrigo = new Abrigo 
                { 
                    Id = abrigoId,
                    Descricao = "Medicamento", 
                    
                };

                _mockAbrigoService.Setup(service => service.ConsultarId(abrigoId)).ReturnsAsync(abrigo);

                var result = await _controller.ConsultarAbrigoId(abrigoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(abrigo, okResult.Value);

                _output.WriteLine($"🎯 Abrigo retornado: Descricao={abrigo.Descricao}\n");
                _output.WriteLine($"📋 Código de status retornado: {okResult.StatusCode}\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
        
        
        
        /*************************************************************************************************
         * Atualizar Abrigo - Controller
         * Teste que garante que o endpoint AtualizarAbrigo modifica corretamente os dados de um abrigo existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Abrigo\n");

                // Arrange
                var abrigoId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do abrigo pelo ID
                _mockAbrigoService.Setup(service => service.ConsultarId(abrigoId)).ReturnsAsync(new Abrigo 
                { 
                    Id = abrigoId,
                    Descricao = "Claudio Silva Bispo", 
                    Documento = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                });

                var abrigoAtualizado = new Abrigo 
                { 
                    Id = abrigoId,
                    Descricao = "João Atualizado", 
                    Documento = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                };

                // Exibindo os dados do abrigo encontrado
                var abrigoExistente = await _mockAbrigoService.Object.ConsultarId(abrigoId);
                _output.WriteLine($"🔌 Abrigo encontrado: Id={abrigoExistente.Id}, Descricao={abrigoExistente.Descricao}, Documento={abrigoExistente.Documento}, Telefone={abrigoExistente.Telefone}, Email={abrigoExistente.Email}, Senha={abrigoExistente.Senha},\n");

                _mockAbrigoService.Setup(service => service.Atualizar(abrigoAtualizado)).ReturnsAsync(abrigoAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarAbrigo(abrigoId, abrigoAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(abrigoAtualizado, okResult.Value);

                _output.WriteLine($"🎯 Abrigo atualizado: Descricao={abrigoAtualizado.Descricao}, Documento={abrigoAtualizado.Documento}, Telefone={abrigoAtualizado.Telefone}, Email={abrigoAtualizado.Email}, Senha={abrigoAtualizado.Senha}, \n");
                _output.WriteLine($"📋 Código de status retornado: {okResult.StatusCode}\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }
        */

        
        /*************************************************************************************************
         * Excluir Abrigo - Controller
         * Teste que garante que o endpoint ExcluirAbrigo remove corretamente um abrigo existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Abrigo()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Abrigo\n");

                // Arrange
                var abrigoId = "68028ee8197f1d3d7bdbda61";

                _mockAbrigoService.Setup(service => service.ConsultarId(abrigoId)).ReturnsAsync(new Abrigo { Id = abrigoId });
                _mockAbrigoService.Setup(service => service.Excluir(abrigoId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirAbrigo(abrigoId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 Abrigo excluído: Id={abrigoId}\n");
                _output.WriteLine($"📋 Código de status retornado: {okObjectResult.StatusCode}\n");
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
