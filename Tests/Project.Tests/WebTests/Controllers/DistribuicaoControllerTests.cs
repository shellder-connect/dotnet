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
    public class DistribuicaoControllerTests
    {
        private readonly Mock<IDistribuicaoService> _mockDistribuicaoService;
        private readonly DistribuicaoController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public DistribuicaoControllerTests(ITestOutputHelper output)
        {
            _mockDistribuicaoService = new Mock<IDistribuicaoService>();
            _controller = new DistribuicaoController(_mockDistribuicaoService.Object);
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
         * Cadastrar Distribuicao - Controller
         * Teste que garante que o endpoint CadastrarDistribuicao cria uma nova distribuicao corretamente e retorna a resposta HTTP.
         **************************************************************************************************/
        [Fact]
        public async Task Cadastrar_Distribuicao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Distribuicao\n");

                var distribuicao = new Distribuicao
                {
                    QuantidadeDestinada = 10,
                    DataDestinada = DateTime.UtcNow,
                    IdDoacao = "507f1f77bcf86cd799439011",
                    IdPessoaAtendida = "507f1f77bcf86cd799439012"
                };

                _mockDistribuicaoService.Setup(service => service.Criar(It.IsAny<Distribuicao>())).ReturnsAsync(distribuicao);

                var result = await _controller.CadastrarDistribuicao(distribuicao);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosDistribuicao", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(distribuicao.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(distribuicao, createdAtActionResult.Value);

                _output.WriteLine($"🎯 Distribuicao criada: QuantidadeDestinada={distribuicao.QuantidadeDestinada}\n");
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
         * Cadastrar Distribuicao - Controller
         * Teste que garante que o endpoint retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        [Fact]
        public async Task CadastrarDistribuicao_DeveRetornarBadRequest()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarDistribuicao_DeveRetornarBadRequest\n");

                var distribuicao = new Distribuicao
                {
                    QuantidadeDestinada = 0, // inválido
                    DataDestinada = DateTime.MinValue,
                    IdDoacao = "",
                    IdPessoaAtendida = ""
                };

                _controller.ModelState.AddModelError("QuantidadeDestinada", "QuantidadeDestinada é obrigatório e deve ser maior que zero");
                _controller.ModelState.AddModelError("IdDoacao", "IdDoacao é obrigatório");
                _controller.ModelState.AddModelError("IdPessoaAtendida", "IdPessoaAtendida é obrigatório");

                var result = await _controller.CadastrarDistribuicao(distribuicao);

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
         * Consultar Todas Distribuicoes - Controller - Simulação
         * Teste que garante que o endpoint retorna todas as distribuições corretamente.
         **************************************************************************************************/
        [Fact]
        public async Task ConsultarTodasDistribuicoes_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodasDistribuicoes_DeveRetornarOk\n");

                // Arrange
                var distribuições = new List<Distribuicao>
                {
                    new Distribuicao
                    {
                        QuantidadeDestinada = 10,
                        DataDestinada = DateTime.UtcNow,
                        IdDoacao = "507f1f77bcf86cd799439011",
                        IdPessoaAtendida = "507f1f77bcf86cd799439012"
                    },
                    new Distribuicao
                    {
                        QuantidadeDestinada = 5,
                        DataDestinada = DateTime.UtcNow.AddDays(-1),
                        IdDoacao = "507f1f77bcf86cd799439013",
                        IdPessoaAtendida = "507f1f77bcf86cd799439014"
                    }
                };

                _mockDistribuicaoService.Setup(service => service.ConsultarTodos()).ReturnsAsync(distribuições);

                // Act
                var result = await _controller.ConsultarTodosDistribuicao();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(distribuições, okResult.Value);

                _output.WriteLine($"🎯 Total de distribuições retornadas: {distribuições.Count}\n");
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
         * Consultar Distribuicao por ID - Controller - Simulação
         * Teste que garante que o endpoint retorna a distribuicao correta pelo ID.
         **************************************************************************************************/
        [Fact]
        public async Task Consultar_Distribuicao_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Distribuicao_Por_Id\n");

                var distribuicaoId = "67d1f58e08416bb1e4414600";
                var distribuicao = new Distribuicao
                {
                    Id = distribuicaoId,
                    QuantidadeDestinada = 15,
                    DataDestinada = DateTime.UtcNow,
                    IdDoacao = "507f1f77bcf86cd799439011",
                    IdPessoaAtendida = "507f1f77bcf86cd799439012"
                };

                _mockDistribuicaoService.Setup(service => service.ConsultarId(distribuicaoId)).ReturnsAsync(distribuicao);

                var result = await _controller.ConsultarDistribuicaoId(distribuicaoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(distribuicao, okResult.Value);

                _output.WriteLine($"🎯 Distribuição retornada: QuantidadeDestinada={distribuicao.QuantidadeDestinada}\n");
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
         * Atualizar Distribuicao - Controller
         * Teste que garante que o endpoint AtualizarDistribuicao modifica os dados corretamente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Distribuicao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Distribuicao\n");

                var distribuicaoId = "683a34fe6333f0107e656ff2";

                _mockDistribuicaoService.Setup(service => service.ConsultarId(distribuicaoId)).ReturnsAsync(new Distribuicao
                {
                    Id = distribuicaoId,
                    QuantidadeDestinada = 20,
                    DataDestinada = DateTime.UtcNow.AddDays(-2),
                    IdDoacao = "665af8b13fae4c001fcf6d12",
                    IdPessoaAtendida = "665af9a03fae4c001fcf6d15"
                });

                var distribuicaoAtualizada = new Distribuicao
                {
                    Id = distribuicaoId,
                    QuantidadeDestinada = 25,
                    DataDestinada = DateTime.UtcNow,
                    IdDoacao = "665af8b13fae4c001fcf6d12",
                    IdPessoaAtendida = "665af9a03fae4c001fcf6d15"
                };

                var distribuicaoExistente = await _mockDistribuicaoService.Object.ConsultarId(distribuicaoId);
                _output.WriteLine($"🔌 Distribuicao encontrada: Id={distribuicaoExistente.Id}, QuantidadeDestinada={distribuicaoExistente.QuantidadeDestinada}, DataDestinada={distribuicaoExistente.DataDestinada}\n");

                _mockDistribuicaoService.Setup(service => service.Atualizar(distribuicaoAtualizada)).ReturnsAsync(distribuicaoAtualizada);

                _controller.ModelState.Clear();

                var result = await _controller.AtualizarDistribuicao(distribuicaoId, distribuicaoAtualizada);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(distribuicaoAtualizada, okResult.Value);

                _output.WriteLine($"🎯 Distribuição atualizada: QuantidadeDestinada={distribuicaoAtualizada.QuantidadeDestinada}\n");
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
         * Excluir Distribuicao - Controller
         * Teste que garante que o endpoint ExcluirDistribuicao remove a distribuicao corretamente.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_Distribuicao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Distribuicao\n");

                var distribuicaoId = "68028ee8197f1d3d7bdbda61";

                _mockDistribuicaoService.Setup(service => service.ConsultarId(distribuicaoId)).ReturnsAsync(new Distribuicao { Id = distribuicaoId });
                _mockDistribuicaoService.Setup(service => service.Excluir(distribuicaoId)).Returns(Task.CompletedTask);

                var result = await _controller.ExcluirDistribuicao(distribuicaoId);

                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                _output.WriteLine($"🎯 Distribuição excluída: Id={distribuicaoId}\n");
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
