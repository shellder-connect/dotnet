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
    public class EnderecoAbrigoControllerTests
    {
        private readonly Mock<IEnderecoAbrigoService> _mockEnderecoAbrigoService;
        private readonly Mock<ICepService> _mockCepService;
        private readonly EnderecoAbrigoController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public EnderecoAbrigoControllerTests(ITestOutputHelper output)
        {
            _mockEnderecoAbrigoService = new Mock<IEnderecoAbrigoService>();
            _mockCepService = new Mock<ICepService>();
            _controller = new EnderecoAbrigoController(_mockEnderecoAbrigoService.Object, _mockCepService.Object);
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
         * Cadastrar EnderecoAbrigo - Controller
         * Teste que garante que o endpoint CadastrarEnderecoAbrigo cria um novo enderecoAbrigo corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_EnderecoAbrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_EnderecoAbrigo\n");

                // Configurar um ID fixo para o teste
         
                var enderecoAbrigo = new EnderecoAbrigo 
                { 
  
                    IdAbrigo = "683a44470f3c2967f9c6ebb3",
                    CEP = "05763290", 
                    Estado = "São Paulo", 
                    Cidade = "Adm",
                    Bairro = "Centro", 
                    Rua = "Teste",
                };

                // Configurar o mock para retornar o endereço com ID
                _mockEnderecoAbrigoService.Setup(service => service.Criar(It.IsAny<EnderecoAbrigo>()))
                                .ReturnsAsync(enderecoAbrigo);

                var enderecoAbrigoDto = new EnderecoAbrigoDTO
                {
                    IdAbrigo = enderecoAbrigo.IdAbrigo,
                    CEP = enderecoAbrigo.CEP,
                    Estado = enderecoAbrigo.Estado,
                    Cidade = enderecoAbrigo.Cidade,
                    Bairro = enderecoAbrigo.Bairro,
                    Rua = enderecoAbrigo.Rua,
                };

                var result = await _controller.CadastrarEnderecoAbrigo(enderecoAbrigoDto);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("Consultar", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                
                // Comparar propriedades individualmente
                var returnedEnderecoAbrigo = Assert.IsType<EnderecoAbrigo>(createdAtActionResult.Value);
                Assert.Equal(enderecoAbrigo.Id, returnedEnderecoAbrigo.Id);
                Assert.Equal(enderecoAbrigo.CEP, returnedEnderecoAbrigo.CEP);
                Assert.Equal(enderecoAbrigo.Estado, returnedEnderecoAbrigo.Estado);
                // ... comparar outras propriedades relevantes
                
                _output.WriteLine($"🎯 EnderecoAbrigo criado: CEP={enderecoAbrigo.CEP}\n");
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
         * Cadastrar EnderecoAbrigo - Controller
         * Teste que garante que o endpoint CadastrarEnderecoAbrigo retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarEnderecoAbrigo_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarEnderecoAbrigo_DeveRetornarBadRequest\n");

                var enderecoAbrigoDto = new EnderecoAbrigoDTO
                {
                    CEP = "",
                    // Map other properties as needed
                };

                _controller.ModelState.AddModelError("CEP", "CEP é obrigatório");

                var result = await _controller.CadastrarEnderecoAbrigo(enderecoAbrigoDto);

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
         * Consultar Todos EnderecoAbrigos - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosEnderecoAbrigos retorna todos os enderecoAbrigos corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosEnderecoAbrigos_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosEnderecoAbrigos_DeveRetornarOk\n");

                // Arrange
                var enderecoAbrigos = new List<EnderecoAbrigo>
                {
                    new EnderecoAbrigo { CEP = "0576290"},
                    new EnderecoAbrigo { Estado = "Santa Catarina", }
                };

                _mockEnderecoAbrigoService.Setup(service => service.ConsultarTodos()).ReturnsAsync(enderecoAbrigos);

                // Act
                var result = await _controller.ConsultarTodosEnderecoAbrigo();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(enderecoAbrigos, okResult.Value);

                _output.WriteLine($"🎯 Total de enderecoAbrigos retornados: {enderecoAbrigos.Count}\n");
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
         * Consultar EnderecoAbrigo por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarEnderecoAbrigoPorId retorna o enderecoAbrigo correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_EnderecoAbrigo_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_EnderecoAbrigo_Por_Id\n");

                var enderecoAbrigoId = "683c3ccb5a59cb66d3037992";
                var enderecoAbrigo = new EnderecoAbrigo 
                { 
                    Id = enderecoAbrigoId,
                    CEP = "01234-567", 
                    
                };

                _mockEnderecoAbrigoService.Setup(service => service.ConsultarId(enderecoAbrigoId)).ReturnsAsync(enderecoAbrigo);

                var result = await _controller.ConsultarEnderecoAbrigoId(enderecoAbrigoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(enderecoAbrigo, okResult.Value);

                _output.WriteLine($"🎯 EnderecoAbrigo retornado: CEP={enderecoAbrigo.CEP}\n");
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
         * Excluir EnderecoAbrigo - Controller
         * Teste que garante que o endpoint ExcluirEnderecoAbrigo remove corretamente um enderecoAbrigo existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_EnderecoAbrigo()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_EnderecoAbrigo\n");

                // Arrange
                var enderecoAbrigoId = "683a34fe6333f0107e656ff5";

                _mockEnderecoAbrigoService.Setup(service => service.ConsultarId(enderecoAbrigoId)).ReturnsAsync(new EnderecoAbrigo { Id = enderecoAbrigoId });
                _mockEnderecoAbrigoService.Setup(service => service.Excluir(enderecoAbrigoId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirEnderecoAbrigo(enderecoAbrigoId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 EnderecoAbrigo excluído: Id={enderecoAbrigoId}\n");
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
