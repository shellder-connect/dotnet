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

                var endereco = new Endereco 
                { 
                    CEP = "05763290", 
                
                };

                _mockEnderecoService.Setup(service => service.Criar(It.IsAny<Endereco>())).ReturnsAsync(endereco);

                var enderecoDto = new EnderecoDTO
                {
                    CEP = endereco.CEP,
                    // Map other properties as needed
                };
                var result = await _controller.CadastrarEndereco(enderecoDto);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosEnderecos", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(endereco.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(endereco, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 Endereco criado: Descricao={endereco.CEP}\n");
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

                _controller.ModelState.AddModelError("Descricao", "Descricao é obrigatório");

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

                var enderecoId = "67d1f58e08416bb1e4414600";
                var endereco = new Endereco 
                { 
                    Id = enderecoId,
                    CEP = "05711222", 
                    
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
         * Atualizar Endereco - Controller
         * Teste que garante que o endpoint AtualizarEndereco modifica corretamente os dados de um endereco existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Endereco()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Endereco\n");

                // Arrange
                var enderecoId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do endereco pelo ID
                _mockEnderecoService.Setup(service => service.ConsultarId(enderecoId)).ReturnsAsync(new Endereco 
                { 
                    Id = enderecoId,
                    Descricao = "Claudio Silva Bispo", 
                    Documento = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                });

                var enderecoAtualizado = new Endereco 
                { 
                    Id = enderecoId,
                    Descricao = "João Atualizado", 
                    Documento = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                };

                // Exibindo os dados do endereco encontrado
                var enderecoExistente = await _mockEnderecoService.Object.ConsultarId(enderecoId);
                _output.WriteLine($"🔌 Endereco encontrado: Id={enderecoExistente.Id}, Descricao={enderecoExistente.Descricao}, Documento={enderecoExistente.Documento}, Telefone={enderecoExistente.Telefone}, Email={enderecoExistente.Email}, Senha={enderecoExistente.Senha},\n");

                _mockEnderecoService.Setup(service => service.Atualizar(enderecoAtualizado)).ReturnsAsync(enderecoAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarEndereco(enderecoId, enderecoAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(enderecoAtualizado, okResult.Value);

                _output.WriteLine($"🎯 Endereco atualizado: Descricao={enderecoAtualizado.Descricao}, Documento={enderecoAtualizado.Documento}, Telefone={enderecoAtualizado.Telefone}, Email={enderecoAtualizado.Email}, Senha={enderecoAtualizado.Senha}, \n");
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
                var enderecoId = "68028ee8197f1d3d7bdbda61";

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
