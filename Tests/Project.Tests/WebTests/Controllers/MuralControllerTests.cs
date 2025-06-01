using Project.Models;
using Project.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Project.Tests.IntegrationTests.Controllers
{
    public class MuralControllerTests
    {
        private readonly Mock<IMuralService> _mockMuralService;
        private readonly MuralController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public MuralControllerTests(ITestOutputHelper output)
        {
            _mockMuralService = new Mock<IMuralService>();
            _controller = new MuralController(_mockMuralService.Object);
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
         * Cadastrar Post no Mural - Controller
         * Teste que garante que o endpoint CadastrarMural cria um novo post corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Mural\n");

                var mural = new Mural 
                { 
                    IdUsuario = "60a1b2c3d4e5f6789012345a",
                    Titulo = "Doação de Roupas de Inverno", 
                    Comentario = "Estamos arrecadando roupas de inverno para famílias necessitadas",
                    TipoPost = "Anuncio",
                    Data = DateTime.Now
                };

                _mockMuralService.Setup(service => service.Criar(It.IsAny<Mural>())).ReturnsAsync(mural);

                var result = await _controller.CadastrarMural(mural);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosMurals", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(mural.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(mural, createdAtActionResult.Value);

                _output.WriteLine($"🎯 Post criado: Titulo={mural.Titulo}, TipoPost={mural.TipoPost}\n");
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
         * Cadastrar Post no Mural - Controller
         * Teste que garante que o endpoint CadastrarMural retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarMural_DeveRetornarBadRequest()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarMural_DeveRetornarBadRequest\n");

                var mural = new Mural 
                { 
                    IdUsuario = "",
                    Titulo = "", 
                    Comentario = "",
                    TipoPost = "",
                    Data = DateTime.Now
                };

                _controller.ModelState.AddModelError("Titulo", "Titulo é obrigatório");
                _controller.ModelState.AddModelError("IdUsuario", "IdUsuario é obrigatório");

                var result = await _controller.CadastrarMural(mural);

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
         * Consultar Todos Posts do Mural - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosMural retorna todos os posts corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosMurais_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosMurais_DeveRetornarOk\n");

                // Arrange
                var murais = new List<Mural>
                {
                    new Mural 
                    { 
                        IdUsuario = "60a1b2c3d4e5f6789012345a",
                        Titulo = "Campanha de Alimentos",
                        TipoPost = "Evento",
                        Data = DateTime.Now
                    },
                    new Mural 
                    { 
                        IdUsuario = "60a1b2c3d4e5f6789012345b",
                        Titulo = "Doação de Livros",
                        TipoPost = "Anuncio",
                        Data = DateTime.Now
                    }
                };

                _mockMuralService.Setup(service => service.ConsultarTodos()).ReturnsAsync(murais);

                // Act
                var result = await _controller.ConsultarTodosMurals();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(murais, okResult.Value);

                _output.WriteLine($"🎯 Total de posts retornados: {murais.Count}\n");
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
         * Consultar Post do Mural por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarMuralPorId retorna o post correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Mural_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Mural_Por_Id\n");

                var muralId = "67d1f58e08416bb1e4414600";
                var mural = new Mural 
                { 
                    Id = muralId,
                    IdUsuario = "60a1b2c3d4e5f6789012345a",
                    Titulo = "Evento Beneficente de Natal", 
                    Comentario = "Grande evento para arrecadação de presentes de Natal",
                    TipoPost = "Evento",
                    Data = DateTime.Now
                };

                _mockMuralService.Setup(service => service.ConsultarId(muralId)).ReturnsAsync(mural);

                var result = await _controller.ConsultarMuralId(muralId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(mural, okResult.Value);

                _output.WriteLine($"🎯 Post retornado: Titulo={mural.Titulo}, TipoPost={mural.TipoPost}\n");
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
         * Atualizar Post do Mural - Controller
         * Teste que garante que o endpoint AtualizarMural modifica corretamente os dados de um post existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Atualizar_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Mural\n");

                // Arrange
                var muralId = "683c343e2609b1c19bad6926";

                // Simulando a consulta do post pelo ID
                _mockMuralService.Setup(service => service.ConsultarId(muralId)).ReturnsAsync(new Mural 
                { 
                    Id = muralId,
                    IdUsuario = "60a1b2c3d4e5f6789012345a",
                    Titulo = "Evento Original", 
                    Comentario = "Comentário original do evento", 
                    TipoPost = "Evento", 
                    Data = DateTime.Now.AddDays(-1)
                });

                var muralAtualizado = new Mural 
                { 
                    Id = muralId,
                    IdUsuario = "60a1b2c3d4e5f6789012345a",
                    Titulo = "Evento Atualizado", 
                    Comentario = "Comentário atualizado do evento", 
                    TipoPost = "Anuncio", 
                    Data = DateTime.Now
                };

                // Exibindo os dados do post encontrado
                var muralExistente = await _mockMuralService.Object.ConsultarId(muralId);
                _output.WriteLine($"🔌 Post encontrado: Id={muralExistente.Id}, Titulo={muralExistente.Titulo}, TipoPost={muralExistente.TipoPost}\n");

                _mockMuralService.Setup(service => service.Atualizar(muralAtualizado)).ReturnsAsync(muralAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarMural(muralId, muralAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                //Assert.Equal(muralAtualizado, okResult.Value);

                var muralRetornado = Assert.IsType<Mural>(okResult.Value);
                Assert.Equal(muralAtualizado.Titulo, muralRetornado.Titulo);
                Assert.Equal(muralAtualizado.TipoPost, muralRetornado.TipoPost);
                Assert.Equal(muralAtualizado.Comentario, muralRetornado.Comentario);

                _output.WriteLine($"🎯 Post atualizado: Titulo={muralAtualizado.Titulo}, TipoPost={muralAtualizado.TipoPost}\n");
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
         * Excluir Post do Mural - Controller
         * Teste que garante que o endpoint ExcluirMural remove corretamente um post existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Mural\n");

                // Arrange
                var muralId = "68028ee8197f1d3d7bdbda61";

                _mockMuralService.Setup(service => service.ConsultarId(muralId)).ReturnsAsync(new Mural { Id = muralId });
                _mockMuralService.Setup(service => service.Excluir(muralId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirMural(muralId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                _output.WriteLine($"🎯 Post excluído: Id={muralId}\n");
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