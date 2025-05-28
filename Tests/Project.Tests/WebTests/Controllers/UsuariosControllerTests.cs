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
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly UsuarioController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public UsuarioControllerTests(ITestOutputHelper output)
        {
            _mockUsuarioService = new Mock<IUsuarioService>();
            _controller = new UsuarioController(_mockUsuarioService.Object);
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
         * Cadastrar Usuário - Controller
         * Teste que garante que o endpoint CadastrarUsuario cria um novo usuário corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Usuario()
        {
            
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Usuario\n");

                var usuario = new Usuario 
                { 
                    Nome = "Claudio Dois", 
                    Email = "claudio_cssp@test.com", 
                    Senha = "senha", 
                    IdTipoUsuario = "Adm",
                    Telefone = "950556066", 
                    IdEndereco = "1",
                    DataNascimento = "1993-05-10",
                    Documento = "40830740000", 
                    Status = "Ativo"
                };

                _mockUsuarioService.Setup(service => service.Criar(It.IsAny<Usuario>())).ReturnsAsync(usuario);

                var result = await _controller.CadastrarUsuario(usuario);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosUsuarios", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(usuario.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(usuario, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 Usuário criado: Nome={usuario.Nome}, Email={usuario.Email}\n");
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
         * Cadastrar Usuário - Controller
         * Teste que garante que o endpoint CadastrarUsuario retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarUsuario_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarUsuario_DeveRetornarBadRequest\n");

                var usuario = new Usuario 
                { 
                    Nome = "", 
                    Email = "claudio_cssp@test.com", 
                    Senha = "senha", 
                    IdTipoUsuario = "Adm",
                    Telefone = "950556066", 
                    IdEndereco = "1",
                    DataNascimento = "1993-05-10",
                    Documento = "40830740000", 
                    Status = "Ativo"
                };

                _controller.ModelState.AddModelError("Nome", "Nome é obrigatório");

                var result = await _controller.CadastrarUsuario(usuario);

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
         * Consultar Todos Usuários - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosUsuarios retorna todos os usuários corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosUsuarios_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosUsuarios_DeveRetornarOk\n");

                // Arrange
                var usuarios = new List<Usuario>
                {
                    new Usuario { Nome = "João", CPF = "12345678910", Telefone = "11975776758", Email = "joao@exemplo.com",     Senha = "senha123", Perfil = "Comum" },
                    new Usuario { Nome = "Maria", CPF = "10987654321", Telefone = "11975776759", Email = "maria@exemplo.com", Senha = "senha456", Perfil = "Comum" }
                };

                _mockUsuarioService.Setup(service => service.ConsultarTodos()).ReturnsAsync(usuarios);

                // Act
                var result = await _controller.ConsultarTodosUsuarios();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(usuarios, okResult.Value);

                _output.WriteLine($"🎯 Total de usuários retornados: {usuarios.Count}\n");
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
         * Consultar Usuário por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarUsuarioPorId retorna o usuário correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Usuario_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Usuario_Por_Id\n");

                var usuarioId = "67d1f58e08416bb1e4414600";
                var usuario = new Usuario 
                { 
                    Id = usuarioId,
                    Nome = "Claudio Dois", 
                    Email = "claudio_cssp@test.com", 
                    Senha = "senha", 
                    IdTipoUsuario = "Adm",
                    Telefone = "950556066", 
                    IdEndereco = "1",
                    DataNascimento = "1993-05-10",
                    Documento = "40830740000", 
                    Status = "Ativo"
                };

                _mockUsuarioService.Setup(service => service.ConsultarId(usuarioId)).ReturnsAsync(usuario);

                var result = await _controller.ConsultarUsuarioId(usuarioId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(usuario, okResult.Value);

                _output.WriteLine($"🎯 Usuário retornado: Nome={usuario.Nome}, Email={usuario.Email}\n");
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
         * Atualizar Usuário - Controller
         * Teste que garante que o endpoint AtualizarUsuario modifica corretamente os dados de um usuário existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Usuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Usuario\n");

                // Arrange
                var usuarioId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do usuário pelo ID
                _mockUsuarioService.Setup(service => service.ConsultarId(usuarioId)).ReturnsAsync(new Usuario 
                { 
                    Id = usuarioId,
                    Nome = "Claudio Silva Bispo", 
                    CPF = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                    Perfil = "Comum" 
                });

                var usuarioAtualizado = new Usuario 
                { 
                    Id = usuarioId,
                    Nome = "João Atualizado", 
                    CPF = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                    Perfil = "Admin" 
                };

                // Exibindo os dados do usuário encontrado
                var usuarioExistente = await _mockUsuarioService.Object.ConsultarId(usuarioId);
                _output.WriteLine($"🔌 Usuário encontrado: Id={usuarioExistente.Id}, Nome={usuarioExistente.Nome}, CPF={usuarioExistente.CPF}, Telefone={usuarioExistente.Telefone}, Email={usuarioExistente.Email}, Senha={usuarioExistente.Senha}, Perfil={usuarioExistente.Perfil}\n");

                _mockUsuarioService.Setup(service => service.Atualizar(usuarioAtualizado)).ReturnsAsync(usuarioAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarUsuario(usuarioId, usuarioAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(usuarioAtualizado, okResult.Value);

                _output.WriteLine($"🎯 Usuário atualizado: Nome={usuarioAtualizado.Nome}, CPF={usuarioAtualizado.CPF}, Telefone={usuarioAtualizado.Telefone}, Email={usuarioAtualizado.Email}, Senha={usuarioAtualizado.Senha}, Perfil={usuarioAtualizado.Perfil}\n");
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
         * Excluir Usuário - Controller
         * Teste que garante que o endpoint ExcluirUsuario remove corretamente um usuário existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Usuario()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Usuario\n");

                // Arrange
                var usuarioId = "68028ee8197f1d3d7bdbda61";

                _mockUsuarioService.Setup(service => service.ConsultarId(usuarioId)).ReturnsAsync(new Usuario { Id = usuarioId });
                _mockUsuarioService.Setup(service => service.Excluir(usuarioId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirUsuario(usuarioId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 Usuário excluído: Id={usuarioId}\n");
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
