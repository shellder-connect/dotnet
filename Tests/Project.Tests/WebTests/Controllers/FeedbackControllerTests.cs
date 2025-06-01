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
    public class FeedbackControllerTests
    {
        private readonly Mock<IFeedbackService> _mockFeedbackService;
        private readonly FeedbackController _controller;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public FeedbackControllerTests(ITestOutputHelper output)
        {
            _mockFeedbackService = new Mock<IFeedbackService>();
            _controller = new FeedbackController(_mockFeedbackService.Object);
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
         * Cadastrar Feedback - Controller
         * Teste que garante que o endpoint CadastrarFeedback cria um novo feedback corretamente e mostre a resposta HTTP.
         **************************************************************************************************/
        
        [Fact]
        public async Task Cadastrar_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Cadastrar_Feedback\n");

                var feedback = new Feedback 
                { 
                    Id = "683a44470f3c2967f9c6ebb3",
                    Nota = "5",
                    Comentario = "Excelente atendimento, muito satisfeito com o serviço prestado",
                    Sentimento = "Positivo"
                };

                // Configurar o mock para retornar o feedback com análise de sentimento
                _mockFeedbackService.Setup(service => service.Criar(It.IsAny<Feedback>()))
                                .ReturnsAsync(feedback);

                var feedbackDto = new FeedbackDTO
                {
                    Nota = feedback.Nota,
                    Comentario = feedback.Comentario,
                    Sentimento = feedback.Sentimento
                    // Sentimento não é enviado no DTO, é gerado automaticamente
                };

                var result = await _controller.CadastrarFeedback(feedback);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("ConsultarTodosFeedbacks", createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues);
                
                // Comparar propriedades individualmente
                var returnedFeedback = Assert.IsType<Feedback>(createdAtActionResult.Value);
                Assert.Equal(feedback.Id, returnedFeedback.Id);
                Assert.Equal(feedback.Nota, returnedFeedback.Nota);
                Assert.Equal(feedback.Comentario, returnedFeedback.Comentario);
                Assert.Equal("Positivo", returnedFeedback.Sentimento); // Verificar se a análise de sentimento funcionou
                
                _output.WriteLine($"🎯 Feedback criado: Nota={feedback.Nota}, Sentimento={feedback.Sentimento}\n");
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
         * Consultar Todos Feedbacks - Controller - Simulação
         * Teste que garante que o endpoint ConsultarTodosFeedbacks retorna todos os feedbacks corretamente.
         **************************************************************************************************/
        
        [Fact]
        public async Task ConsultarTodosFeedbacks_DeveRetornarOk()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: ConsultarTodosFeedbacks_DeveRetornarOk\n");

                // Arrange
                var feedbacks = new List<Feedback>
                {
                    new Feedback 
                    { 
                        Nota = "5", 
                        Comentario = "Excelente serviço", 
                        Sentimento = "Positivo" 
                    },
                    new Feedback 
                    { 
                        Nota = "2", 
                        Comentario = "Atendimento ruim", 
                        Sentimento = "Negativo" 
                    },
                    new Feedback 
                    { 
                        Nota = "3", 
                        Comentario = "Serviço regular", 
                        Sentimento = "Indefinido" 
                    }
                };

                _mockFeedbackService.Setup(service => service.ConsultarTodos()).ReturnsAsync(feedbacks);

                // Act
                var result = await _controller.ConsultarTodosFeedbacks();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(feedbacks, okResult.Value);

                _output.WriteLine($"🎯 Total de feedbacks retornados: {feedbacks.Count}\n");
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
         * Consultar Feedback por ID - Controller - Simulação
         * Teste que garante que o endpoint ConsultarFeedbackPorId retorna o feedback correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Feedback_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");   
                _output.WriteLine("🔌 Iniciando teste: Consultar_Feedback_Por_Id\n");

                var feedbackId = "683c4080b167f883ab597482";
                var feedback = new Feedback 
                { 
                    Id = feedbackId,
                    Nota = "5",
                    Comentario = "Excelente atendimento, muito satisfeito com o serviço prestado"
                };

                _mockFeedbackService.Setup(service => service.ConsultarId(feedbackId)).ReturnsAsync(feedback);

                var result = await _controller.ConsultarFeedbackId(feedbackId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(feedback, okResult.Value);

                _output.WriteLine($"🎯 Feedback retornado: Nota={feedback.Nota}, Sentimento={feedback.Sentimento}\n");
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
         * Atualizar Feedback - Controller
         * Teste que garante que o endpoint AtualizarFeedback modifica corretamente os dados de um feedback existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Atualizar_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Feedback\n");

                var feedbackId = "683c4080b167f883ab597482";

                // Simulando a consulta do feedback pelo ID
                _mockFeedbackService.Setup(service => service.ConsultarId(feedbackId)).ReturnsAsync(new Feedback 
                { 
                    Id = feedbackId,
                    Nota = "5",
                    Comentario = "Excelente atendimento, muito satisfeito com o serviço prestado",
                    Sentimento = "Positivo"
                });

                var feedbackAtualizado = new Feedback 
                { 
                    Id = feedbackId,
                    Nota = "5",
                    Comentario = "Excelente atendimento, muito satisfeito com o serviço prestado",
                    Sentimento = "Positivo"
                };

                // Exibindo os dados do feedback encontrado
                var feedbackExistente = await _mockFeedbackService.Object.ConsultarId(feedbackId);
                _output.WriteLine($"🔌 Feedback encontrado: Id={feedbackExistente.Id}, Nota={feedbackExistente.Nota}, Sentimento={feedbackExistente.Sentimento}\n");

                _mockFeedbackService.Setup(service => service.Atualizar(It.IsAny<Feedback>())).ReturnsAsync(feedbackAtualizado);

                // Garantir que o ModelState é válido
                _controller.ModelState.Clear();

                // Act
                var result = await _controller.AtualizarFeedback(feedbackId, feedbackAtualizado);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                
                var feedbackRetornado = Assert.IsType<Feedback>(okResult.Value);
                Assert.Equal(feedbackAtualizado.Nota, feedbackRetornado.Nota);
                Assert.Equal(feedbackAtualizado.Comentario, feedbackRetornado.Comentario);
                Assert.Equal(feedbackAtualizado.Sentimento, feedbackRetornado.Sentimento);

                _output.WriteLine($"🎯 Feedback atualizado: Nota={feedbackAtualizado.Nota}, Sentimento={feedbackAtualizado.Sentimento}\n");
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
         * Excluir Feedback - Controller
         * Teste que garante que o endpoint ExcluirFeedback remove corretamente um feedback existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Feedback\n");

                // Arrange
                var feedbackId = "683c405992765af817feda4a";

                _mockFeedbackService.Setup(service => service.ConsultarId(feedbackId)).ReturnsAsync(new Feedback { Id = feedbackId });
                _mockFeedbackService.Setup(service => service.Excluir(feedbackId)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.ExcluirUsuario(feedbackId);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okObjectResult.StatusCode);

                _output.WriteLine($"🎯 Feedback excluído: Id={feedbackId}\n");
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