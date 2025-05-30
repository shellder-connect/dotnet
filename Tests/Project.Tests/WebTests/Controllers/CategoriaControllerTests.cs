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
    public class CategoriaControllerTests
    {
        private readonly Mock<ICategoriaService> _mockCategoriaService;
        private readonly CategoriaController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public CategoriaControllerTests(ITestOutputHelper output)
        {
            _mockCategoriaService = new Mock<ICategoriaService>();
            _controller = new CategoriaController(_mockCategoriaService.Object);
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
         * Cadastrar Categoria - Controller
         * Teste que garante que o endpoint CadastrarCategoria cria um novo categoria corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Categoria()
        {
            
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Categoria\n");

                var categoria = new Categoria 
                { 
                    Descricao = "Vestuário", 
                
                };

                _mockCategoriaService.Setup(service => service.Criar(It.IsAny<Categoria>())).ReturnsAsync(categoria);

                var result = await _controller.CadastrarCategoria(categoria);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodasCategorias", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                Assert.Equal(categoria.Id, createdAtActionResult.RouteValues["id"]);
                Assert.Equal(categoria, createdAtActionResult.Value);

                
                _output.WriteLine($"🎯 Categoria criado: Descricao={categoria.Descricao}\n");
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
         * Cadastrar Categoria - Controller
         * Teste que garante que o endpoint CadastrarCategoria retorna BadRequest quando dados inválidos são fornecidos.
         **************************************************************************************************/
        
        [Fact]
        public async Task CadastrarCategoria_DeveRetornarBadRequest()
        {
            
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: CadastrarCategoria_DeveRetornarBadRequest\n");

                var categoria = new Categoria 
                { 
                    Descricao = "", 
                   
                };

                _controller.ModelState.AddModelError("Descricao", "Descricao é obrigatório");

                var result = await _controller.CadastrarCategoria(categoria);

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
         * Consultar Todos Categorias - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosCategorias retorna todos os categorias corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosCategorias_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosCategorias_DeveRetornarOk\n");

                // Arrange
                var categorias = new List<Categoria>
                {
                    new Categoria { Descricao = "Medicamento"},
                    new Categoria { Descricao = "Vestuário", }
                };

                _mockCategoriaService.Setup(service => service.ConsultarTodos()).ReturnsAsync(categorias);

                // Act
                var result = await _controller.ConsultarTodasCategorias();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(categorias, okResult.Value);

                _output.WriteLine($"🎯 Total de categorias retornados: {categorias.Count}\n");
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
         * Consultar Categoria por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarCategoriaPorId retorna o categoria correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Categoria_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Categoria_Por_Id\n");

                var categoriaId = "67d1f58e08416bb1e4414600";
                var categoria = new Categoria 
                { 
                    Id = categoriaId,
                    Descricao = "Medicamento", 
                    
                };

                _mockCategoriaService.Setup(service => service.ConsultarId(categoriaId)).ReturnsAsync(categoria);

                var result = await _controller.ConsultarCategoriaId(categoriaId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(categoria, okResult.Value);

                _output.WriteLine($"🎯 Categoria retornado: Descricao={categoria.Descricao}\n");
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
         * Atualizar Categoria - Controller
         * Teste que garante que o endpoint AtualizarCategoria modifica corretamente os dados de um categoria existente.
         **************************************************************************************************/
        /*
        [Fact]
        public async Task Atualizar_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Categoria\n");

                // Arrange
                var categoriaId = "67d1f58e08416bb1e4414600";

                // Simulando a consulta do categoria pelo ID
                _mockCategoriaService.Setup(service => service.ConsultarId(categoriaId)).ReturnsAsync(new Categoria 
                { 
                    Id = categoriaId,
                    Descricao = "Claudio Silva Bispo", 
                    Documento = "408.307.498-10", 
                    Telefone = "11958757740", 
                    Email = "claudio_cssp@hotmail.com", 
                    Senha = "654321", 
                });

                var categoriaAtualizado = new Categoria 
                { 
                    Id = categoriaId,
                    Descricao = "João Atualizado", 
                    Documento = "12345678910", 
                    Telefone = "11975776758", 
                    Email = "joao_atualizado@exemplo.com", 
                    Senha = "nova_senha", 
                };

                // Exibindo os dados do categoria encontrado
                var categoriaExistente = await _mockCategoriaService.Object.ConsultarId(categoriaId);
                _output.WriteLine($"🔌 Categoria encontrado: Id={categoriaExistente.Id}, Descricao={categoriaExistente.Descricao}, Documento={categoriaExistente.Documento}, Telefone={categoriaExistente.Telefone}, Email={categoriaExistente.Email}, Senha={categoriaExistente.Senha},\n");

                _mockCategoriaService.Setup(service => service.Atualizar(categoriaAtualizado)).ReturnsAsync(categoriaAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarCategoria(categoriaId, categoriaAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(categoriaAtualizado, okResult.Value);

                _output.WriteLine($"🎯 Categoria atualizado: Descricao={categoriaAtualizado.Descricao}, Documento={categoriaAtualizado.Documento}, Telefone={categoriaAtualizado.Telefone}, Email={categoriaAtualizado.Email}, Senha={categoriaAtualizado.Senha}, \n");
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
         * Excluir Categoria - Controller
         * Teste que garante que o endpoint ExcluirCategoria remove corretamente um categoria existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Categoria()
        {
            try
            {
                 _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Categoria\n");

                // Arrange
                var categoriaId = "68028ee8197f1d3d7bdbda61";

                _mockCategoriaService.Setup(service => service.ConsultarId(categoriaId)).ReturnsAsync(new Categoria { Id = categoriaId });
                _mockCategoriaService.Setup(service => service.Excluir(categoriaId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirCategoria(categoriaId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                
                _output.WriteLine($"🎯 Categoria excluído: Id={categoriaId}\n");
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
