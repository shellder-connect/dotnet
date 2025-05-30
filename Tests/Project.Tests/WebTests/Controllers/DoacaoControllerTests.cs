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
    public class DoacaoControllerTests
    {
        private readonly Mock<IDoacaoService> _mockDoacaoService;
        private readonly DoacaoController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public DoacaoControllerTests(ITestOutputHelper output)
        {
            _mockDoacaoService = new Mock<IDoacaoService>();
            _controller = new DoacaoController(_mockDoacaoService.Object);
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
         * Cadastrar Doacao - Controller
         * Teste que garante que o endpoint CadastrarDoacao cria um novo doacao corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Doacao()
        {
            
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Doacao\n");

                var doacao = new Doacao 
                { 
                    Descricao = "Vestuário", 
                
                };

                _mockDoacaoService.Setup(service => service.Criar(It.IsAny<Doacao>())).ReturnsAsync(doacao);

                var result = await _controller.CadastrarDoacao(doacao);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosDoacaos", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(doacao.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(doacao, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 Doacao criado: Descricao={doacao.Descricao}\n");
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
         * Cadastrar Doacao - Controller
         * Teste que garante que o endpoint CadastrarDoacao retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarDoacao_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarDoacao_DeveRetornarBadRequest\n");

                var doacao = new Doacao 
                { 
                    Descricao = "", 
                   
                };

                _controller.ModelState.AddModelError("Descricao", "Descricao é obrigatório");

                var result = await _controller.CadastrarDoacao(doacao);

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
         * Consultar Todos Doacaos - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosDoacaos retorna todos os doacaos corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosDoacaos_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosDoacaos_DeveRetornarOk\n");

                // Arrange
                var doacaos = new List<Doacao>
                {
                    new Doacao { Descricao = "Medicamento"},
                    new Doacao { Descricao = "Vestuário", }
                };

                _mockDoacaoService.Setup(service => service.ConsultarTodos()).ReturnsAsync(doacaos);

                // Act
                var result = await _controller.ConsultarTodosDoacao();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(doacaos, okResult.Value);

                _output.WriteLine($"🎯 Total de doacaos retornados: {doacaos.Count}\n");
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
         * Consultar Doacao por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarDoacaoPorId retorna o doacao correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Doacao_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Doacao_Por_Id\n");

                var doacaoId = "67d1f58e08416bb1e4414600";
                var doacao = new Doacao 
                { 
                    Id = doacaoId,
                    Descricao = "Medicamento", 
                    
                };

                _mockDoacaoService.Setup(service => service.ConsultarId(doacaoId)).ReturnsAsync(doacao);

                var result = await _controller.ConsultarDoacaoId(doacaoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(doacao, okResult.Value);

                _output.WriteLine($"🎯 Doacao retornado: Descricao={doacao.Descricao}\n");
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
         * Atualizar Doacao - Controller
         * Teste que garante que o endpoint AtualizarDoacao modifica corretamente os dados de um doacao existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Doacao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Doacao\n");

                // Arrange
                var doacaoId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do doacao pelo ID
                _mockDoacaoService.Setup(service => service.ConsultarId(doacaoId)).ReturnsAsync(new Doacao 
                { 
                    Id = doacaoId,
                    Descricao = "Claudio Silva Bispo", 
                    Documento = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                });

                var doacaoAtualizado = new Doacao 
                { 
                    Id = doacaoId,
                    Descricao = "João Atualizado", 
                    Documento = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                };

                // Exibindo os dados do doacao encontrado
                var doacaoExistente = await _mockDoacaoService.Object.ConsultarId(doacaoId);
                _output.WriteLine($"🔌 Doacao encontrado: Id={doacaoExistente.Id}, Descricao={doacaoExistente.Descricao}, Documento={doacaoExistente.Documento}, Telefone={doacaoExistente.Telefone}, Email={doacaoExistente.Email}, Senha={doacaoExistente.Senha},\n");

                _mockDoacaoService.Setup(service => service.Atualizar(doacaoAtualizado)).ReturnsAsync(doacaoAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarDoacao(doacaoId, doacaoAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(doacaoAtualizado, okResult.Value);

                _output.WriteLine($"🎯 Doacao atualizado: Descricao={doacaoAtualizado.Descricao}, Documento={doacaoAtualizado.Documento}, Telefone={doacaoAtualizado.Telefone}, Email={doacaoAtualizado.Email}, Senha={doacaoAtualizado.Senha}, \n");
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
         * Excluir Doacao - Controller
         * Teste que garante que o endpoint ExcluirDoacao remove corretamente um doacao existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Doacao()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Doacao\n");

                // Arrange
                var doacaoId = "68028ee8197f1d3d7bdbda61";

                _mockDoacaoService.Setup(service => service.ConsultarId(doacaoId)).ReturnsAsync(new Doacao { Id = doacaoId });
                _mockDoacaoService.Setup(service => service.Excluir(doacaoId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirDoacao(doacaoId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 Doacao excluído: Id={doacaoId}\n");
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
