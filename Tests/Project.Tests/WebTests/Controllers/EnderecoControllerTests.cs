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
    public class EnderecoControllerTests
    {
        private readonly Mock<IEnderecoService> _mockEnderecoService;
        private readonly Mock<ICepService> _mockCepService;
        private readonly EnderecoController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public EnderecoControllerTests(ITestOutputHelper output)
        {
            _mockEnderecoService = new Mock<IEnderecoService>();
            _mockCepService = new Mock<ICepService>();
            _controller = new EnderecoController(_mockEnderecoService.Object, _mockCepService.Object);
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
         * Cadastrar Endereco - Controller
         * Teste que garante que o endpoint CadastrarEndereco cria um novo endereco corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Endereco()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Endereco\n");

                // Configurar um ID fixo para o teste
         
                var endereco = new Endereco 
                { 
  
                    IdUsuario = "683a44470f3c2967f9c6ebb3",
                    CEP = "05763290", 
                    Estado = "São Paulo", 
                    Cidade = "Adm",
                    Bairro = "Centro", 
                    Rua = "Teste",
                };

                // Configurar o mock para retornar o endereço com ID
                _mockEnderecoService.Setup(service => service.Criar(It.IsAny<Endereco>()))
                                .ReturnsAsync(endereco);

                var enderecoDto = new EnderecoDTO
                {
                    IdUsuario = endereco.IdUsuario,
                    CEP = endereco.CEP,
                    Estado = endereco.Estado,
                    Cidade = endereco.Cidade,
                    Bairro = endereco.Bairro,
                    Rua = endereco.Rua,
                };

                var result = await _controller.CadastrarEndereco(enderecoDto);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("Consultar", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                
                // Comparar propriedades individualmente
                var returnedEndereco = Assert.IsType<Endereco>(createdAtActionResult.Value);
                Assert.Equal(endereco.Id, returnedEndereco.Id);
                Assert.Equal(endereco.CEP, returnedEndereco.CEP);
                Assert.Equal(endereco.Estado, returnedEndereco.Estado);
                // ... comparar outras propriedades relevantes
                
                _output.WriteLine($"🎯 Endereco criado: CEP={endereco.CEP}\n");
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
         * Cadastrar Endereco - Controller
         * Teste que garante que o endpoint CadastrarEndereco retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarEndereco_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarEndereco_DeveRetornarBadRequest\n");

                var enderecoDto = new EnderecoDTO
                {
                    CEP = "",
                    // Map other properties as needed
                };

                _controller.ModelState.AddModelError("CEP", "CEP é obrigatório");

                var result = await _controller.CadastrarEndereco(enderecoDto);

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
         * Consultar Todos Enderecos - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosEnderecos retorna todos os enderecos corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosEnderecos_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosEnderecos_DeveRetornarOk\n");

                // Arrange
                var enderecos = new List<Endereco>
                {
                    new Endereco { CEP = "0576290"},
                    new Endereco { Estado = "Santa Catarina", }
                };

                _mockEnderecoService.Setup(service => service.ConsultarTodos()).ReturnsAsync(enderecos);

                // Act
                var result = await _controller.ConsultarTodosEndereco();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(enderecos, okResult.Value);

                _output.WriteLine($"🎯 Total de enderecos retornados: {enderecos.Count}\n");
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
         * Consultar Endereco por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarEnderecoPorId retorna o endereco correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Endereco_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Endereco_Por_Id\n");

                var enderecoId = "683a34fe6333f0107e656ff5";
                var endereco = new Endereco 
                { 
                    Id = enderecoId,
                    CEP = "05763290", 
                    
                };

                _mockEnderecoService.Setup(service => service.ConsultarId(enderecoId)).ReturnsAsync(endereco);

                var result = await _controller.ConsultarEnderecoId(enderecoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(endereco, okResult.Value);

                _output.WriteLine($"🎯 Endereco retornado: CEP={endereco.CEP}\n");
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
         * Excluir Endereco - Controller
         * Teste que garante que o endpoint ExcluirEndereco remove corretamente um endereco existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Endereco()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Endereco\n");

                // Arrange
                var enderecoId = "683a34fe6333f0107e656ff5";

                _mockEnderecoService.Setup(service => service.ConsultarId(enderecoId)).ReturnsAsync(new Endereco { Id = enderecoId });
                _mockEnderecoService.Setup(service => service.Excluir(enderecoId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirEndereco(enderecoId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 Endereco excluído: Id={enderecoId}\n");
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
