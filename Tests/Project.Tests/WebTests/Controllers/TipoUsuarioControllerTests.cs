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
    public class TipoUsuarioControllerTests
    {
        private readonly Mock<ITipoUsuarioService> _mockTipoUsuarioService;
        private readonly TipoUsuarioController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public TipoUsuarioControllerTests(ITestOutputHelper output)
        {
            _mockTipoUsuarioService = new Mock<ITipoUsuarioService>();
            _controller = new TipoUsuarioController(_mockTipoUsuarioService.Object);
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
         * Cadastrar TipoUsuario - Controller
         * Teste que garante que o endpoint CadastrarTipoUsuario cria um novo tipoUsuario corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_TipoUsuario()
        {
            
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_TipoUsuario\n");

                var tipoUsuario = new TipoUsuario 
                { 
                    Descricao = "Vestuário", 
                
                };

                _mockTipoUsuarioService.Setup(service => service.Criar(It.IsAny<TipoUsuario>())).ReturnsAsync(tipoUsuario);

                var result = await _controller.CadastrarTipoUsuario(tipoUsuario);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosTiposUsuario", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(tipoUsuario.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(tipoUsuario, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 TipoUsuario criado: Descricao={tipoUsuario.Descricao}\n");
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
         * Cadastrar TipoUsuario - Controller
         * Teste que garante que o endpoint CadastrarTipoUsuario retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarTipoUsuario_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarTipoUsuario_DeveRetornarBadRequest\n");

                var tipoUsuario = new TipoUsuario 
                { 
                    Descricao = "", 
                   
                };

                _controller.ModelState.AddModelError("Descricao", "Descricao é obrigatório");

                var result = await _controller.CadastrarTipoUsuario(tipoUsuario);

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
         * Consultar Todos TipoUsuarios - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosTipoUsuarios retorna todos os tipoUsuarios corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosTipoUsuarios_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosTipoUsuarios_DeveRetornarOk\n");

                // Arrange
                var tipoUsuarios = new List<TipoUsuario>
                {
                    new TipoUsuario { Descricao = "Medicamento"},
                    new TipoUsuario { Descricao = "Vestuário", }
                };

                _mockTipoUsuarioService.Setup(service => service.ConsultarTodos()).ReturnsAsync(tipoUsuarios);

                // Act
                var result = await _controller.ConsultarTodosTiposUsuario();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(tipoUsuarios, okResult.Value);

                _output.WriteLine($"🎯 Total de tipoUsuarios retornados: {tipoUsuarios.Count}\n");
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
         * Consultar TipoUsuario por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTipoUsuarioPorId retorna o tipoUsuario correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_TipoUsuario_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_TipoUsuario_Por_Id\n");

                var tipoUsuarioId = "67d1f58e08416bb1e4414600";
                var tipoUsuario = new TipoUsuario 
                { 
                    Id = tipoUsuarioId,
                    Descricao = "Medicamento", 
                    
                };

                _mockTipoUsuarioService.Setup(service => service.ConsultarId(tipoUsuarioId)).ReturnsAsync(tipoUsuario);

                var result = await _controller.ConsultarTipoUsuarioId(tipoUsuarioId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(tipoUsuario, okResult.Value);

                _output.WriteLine($"🎯 TipoUsuario retornado: Descricao={tipoUsuario.Descricao}\n");
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
         * Atualizar TipoUsuario - Controller
         * Teste que garante que o endpoint AtualizarTipoUsuario modifica corretamente os dados de um tipoUsuario existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_TipoUsuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_TipoUsuario\n");

                // Arrange
                var tipoUsuarioId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do tipoUsuario pelo ID
                _mockTipoUsuarioService.Setup(service => service.ConsultarId(tipoUsuarioId)).ReturnsAsync(new TipoUsuario 
                { 
                    Id = tipoUsuarioId,
                    Descricao = "Claudio Silva Bispo", 
                    Documento = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                });

                var tipoUsuarioAtualizado = new TipoUsuario 
                { 
                    Id = tipoUsuarioId,
                    Descricao = "João Atualizado", 
                    Documento = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                };

                // Exibindo os dados do tipoUsuario encontrado
                var tipoUsuarioExistente = await _mockTipoUsuarioService.Object.ConsultarId(tipoUsuarioId);
                _output.WriteLine($"🔌 TipoUsuario encontrado: Id={tipoUsuarioExistente.Id}, Descricao={tipoUsuarioExistente.Descricao}, Documento={tipoUsuarioExistente.Documento}, Telefone={tipoUsuarioExistente.Telefone}, Email={tipoUsuarioExistente.Email}, Senha={tipoUsuarioExistente.Senha},\n");

                _mockTipoUsuarioService.Setup(service => service.Atualizar(tipoUsuarioAtualizado)).ReturnsAsync(tipoUsuarioAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarTipoUsuario(tipoUsuarioId, tipoUsuarioAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(tipoUsuarioAtualizado, okResult.Value);

                _output.WriteLine($"🎯 TipoUsuario atualizado: Descricao={tipoUsuarioAtualizado.Descricao}, Documento={tipoUsuarioAtualizado.Documento}, Telefone={tipoUsuarioAtualizado.Telefone}, Email={tipoUsuarioAtualizado.Email}, Senha={tipoUsuarioAtualizado.Senha}, \n");
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
         * Excluir TipoUsuario - Controller
         * Teste que garante que o endpoint ExcluirTipoUsuario remove corretamente um tipoUsuario existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_TipoUsuario()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_TipoUsuario\n");

                // Arrange
                var tipoUsuarioId = "68028ee8197f1d3d7bdbda61";

                _mockTipoUsuarioService.Setup(service => service.ConsultarId(tipoUsuarioId)).ReturnsAsync(new TipoUsuario { Id = tipoUsuarioId });
                _mockTipoUsuarioService.Setup(service => service.Excluir(tipoUsuarioId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirTipoUsuario(tipoUsuarioId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 TipoUsuario excluído: Id={tipoUsuarioId}\n");
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
