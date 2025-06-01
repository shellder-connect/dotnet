using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services
{
    public class FeedbackServiceTests
    {
        private readonly ITestOutputHelper _output;

        public FeedbackServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************

         Criar Feedback com Análise de Sentimento Positivo

         Teste será realizado para criação de um feedback com comentário positivo

        **************************************************************************************************/

        [Fact]
        public async Task Criar_FeedbackComComentarioPositivo_RetornaFeedbackComSentimentoPositivo()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);
            
            var feedback = new Feedback 
            { 
                Nota = "5", 
                Comentario = "Excelente atendimento, muito satisfeito com o serviço"
            };

            var feedbackEsperado = new Feedback 
            { 
                Nota = "5", 
                Comentario = "Excelente atendimento, muito satisfeito com o serviço",
                Sentimento = "Positivo"
            };

            // Mock da análise de sentimento retornando positivo
            var predictionMock = new FeedbackPrediction { IsPositive = true };
            mockSentimentService.Setup(s => s.Predict(feedback.Comentario)).Returns(predictionMock);
            mockRepo.Setup(repo => repo.Criar(It.IsAny<Feedback>())).ReturnsAsync(feedbackEsperado);

            _output.WriteLine("********************( Chamando o serviço Criar para feedback positivo )********************\n");

            var resultado = await service.Criar(feedback);

            _output.WriteLine($"Feedback criado: Nota={resultado.Nota}, Sentimento={resultado.Sentimento}\n");

            Assert.Equal(feedback.Nota, resultado.Nota);
            Assert.Equal(feedback.Comentario, resultado.Comentario);
            Assert.Equal("Positivo", resultado.Sentimento);
            
            // Verifica se a análise de sentimento foi chamada
            mockSentimentService.Verify(s => s.Predict(feedback.Comentario), Times.Once);

            _output.WriteLine("********************( Teste de criação de feedback positivo concluído )********************\n");
        }

        /*************************************************************************************************

         Criar Feedback com Análise de Sentimento Negativo

         Teste será realizado para criação de um feedback com comentário negativo

        **************************************************************************************************/

        [Fact]
        public async Task Criar_FeedbackComComentarioNegativo_RetornaFeedbackComSentimentoNegativo()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);
            
            var feedback = new Feedback 
            { 
                Nota = "2", 
                Comentario = "Atendimento terrível, muito insatisfeito"
            };

            var feedbackEsperado = new Feedback 
            { 
                Nota = "2", 
                Comentario = "Atendimento terrível, muito insatisfeito",
                Sentimento = "Negativo"
            };

            // Mock da análise de sentimento retornando negativo
            var predictionMock = new FeedbackPrediction { IsPositive = false };
            mockSentimentService.Setup(s => s.Predict(feedback.Comentario)).Returns(predictionMock);
            mockRepo.Setup(repo => repo.Criar(It.IsAny<Feedback>())).ReturnsAsync(feedbackEsperado);

            _output.WriteLine("********************( Chamando o serviço Criar para feedback negativo )********************\n");

            var resultado = await service.Criar(feedback);

            _output.WriteLine($"Feedback criado: Nota={resultado.Nota}, Sentimento={resultado.Sentimento}\n");

            Assert.Equal(feedback.Nota, resultado.Nota);
            Assert.Equal(feedback.Comentario, resultado.Comentario);
            Assert.Equal("Negativo", resultado.Sentimento);
            
            // Verifica se a análise de sentimento foi chamada
            mockSentimentService.Verify(s => s.Predict(feedback.Comentario), Times.Once);

            _output.WriteLine("********************( Teste de criação de feedback negativo concluído )********************\n");
        }

        /*************************************************************************************************

         Criar Feedback sem Comentário

         Teste será realizado para criação de um feedback sem comentário

        **************************************************************************************************/

        [Fact]
        public async Task Criar_FeedbackSemComentario_RetornaFeedbackComSentimentoIndefinido()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);
            
            var feedback = new Feedback 
            { 
                Nota = "3", 
                Comentario = "" // Comentário vazio
            };

            var feedbackEsperado = new Feedback 
            { 
                Nota = "3", 
                Comentario = "",
                Sentimento = "Indefinido"
            };

            mockRepo.Setup(repo => repo.Criar(It.IsAny<Feedback>())).ReturnsAsync(feedbackEsperado);

            _output.WriteLine("********************( Chamando o serviço Criar para feedback sem comentário )********************\n");

            var resultado = await service.Criar(feedback);

            _output.WriteLine($"Feedback criado: Nota={resultado.Nota}, Sentimento={resultado.Sentimento}\n");

            Assert.Equal(feedback.Nota, resultado.Nota);
            Assert.Equal("Indefinido", resultado.Sentimento);
            
            // Verifica se a análise de sentimento NÃO foi chamada
            mockSentimentService.Verify(s => s.Predict(It.IsAny<string>()), Times.Never);

            _output.WriteLine("********************( Teste de criação de feedback sem comentário concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os Feedbacks

         Teste será realizado para consultar a lista completa de feedbacks com dados.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_DeveRetornarListaDeFeedbacks()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            var feedbacksEsperados = new List<Feedback>
            {
                new Feedback { Nota = "5", Comentario = "Excelente serviço", Sentimento = "Positivo" },
                new Feedback { Nota = "3", Comentario = "Atendimento regular", Sentimento = "Indefinido" },
                new Feedback { Nota = "2", Comentario = "Serviço ruim, precisa melhorar", Sentimento = "Negativo" },
                new Feedback { Nota = "4", Comentario = "Muito bom, recomendo", Sentimento = "Positivo" },
                new Feedback { Nota = "1", Comentario = "Péssimo atendimento", Sentimento = "Negativo" }
            };
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(feedbacksEsperados);
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar todos os feedbacks )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine("Feedbacks retornados:");
            foreach (var feedback in resultado)
            {
                _output.WriteLine($"- Nota: {feedback.Nota}, Comentario: {feedback.Comentario}, Sentimento: {feedback.Sentimento}");
            }

            Assert.Equal(5, resultado.Count);
            Assert.Contains(resultado, f => f.Nota == "5" && f.Sentimento == "Positivo");
            Assert.Contains(resultado, f => f.Nota == "3" && f.Sentimento == "Indefinido");
            Assert.Contains(resultado, f => f.Nota == "1" && f.Sentimento == "Negativo");

            _output.WriteLine("********************( Teste de consulta de feedback concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os Feedbacks - Cenário vazio

         Teste para retornar lista vazia caso não haja feedbacks.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_SemFeedbacks_DeveRetornarListaVazia()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Feedback>());
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de feedback )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine($"Total de feedbacks retornados: {resultado.Count}\n");
            Assert.Empty(resultado);

            _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar Feedback por ID

         Teste será realizado apenas para consultar um feedback pelo ID

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarId_DeveRetornarFeedbackPorId()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            
            // Configura o mock para retornar um feedback específico quando consultado pelo ID
            var feedbackEsperado = new Feedback 
            { 
                Nota = "4", 
                Comentario = "Bom atendimento, gostei do serviço",
                Sentimento = "Positivo",
                Id = "123" 
            };
            mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(feedbackEsperado);
            
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar o feedback pelo Id )********************\n");

            var resultado = await service.ConsultarId("123");

            _output.WriteLine($"Feedback e Id consultado: Nota={resultado?.Nota}, Id={resultado?.Id}, Sentimento={resultado?.Sentimento}\n");
            Assert.NotNull(resultado);
            Assert.Equal("4", resultado?.Nota);
            Assert.Equal("123", resultado?.Id);
            Assert.Equal("Positivo", resultado?.Sentimento);

            _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
        }

        /*************************************************************************************************

         Atualizar Feedback

         Teste para atualizar um feedback existente

        **************************************************************************************************/

        [Fact]
        public async Task Atualizar_DeveRetornarFeedbackAtualizado()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();

            var feedbackOriginal = new Feedback 
            { 
                Nota = "3", 
                Comentario = "Atendimento regular",
                Sentimento = "Indefinido",
                Id = "123" 
            };
            var feedbackAtualizado = new Feedback 
            { 
                Nota = "5", 
                Comentario = "Atendimento excelente, muito melhor agora",
                Sentimento = "Positivo",
                Id = "123" 
            };

            mockRepo.Setup(repo => repo.Atualizar(feedbackOriginal)).ReturnsAsync(feedbackAtualizado);

            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do feedback )********************\n");

            var resultado = await service.Atualizar(feedbackOriginal);

            _output.WriteLine($"Feedback atualizado: Nota={resultado?.Nota}, Sentimento={resultado?.Sentimento}\n");
            Assert.NotNull(resultado);
            Assert.Equal("5", resultado?.Nota);
            Assert.Equal("Positivo", resultado?.Sentimento);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização concluído )********************\n");
        }

        /*************************************************************************************************

         Atualizar Parcial Feedback

         Teste para atualizar parcialmente um feedback existente

        **************************************************************************************************/

        [Fact]
        public async Task AtualizarParcial_DeveRetornarFeedbackAtualizadoParcialmente()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();

            var feedbackAtualizado = new Feedback 
            { 
                Nota = "4", 
                Comentario = "Atendimento bom, melhorou bastante",
                Sentimento = "Positivo",
                Id = "123" 
            };

            var camposParaAtualizar = new Dictionary<string, object>
            {
                { "Nota", "4" },
                { "Comentario", "Atendimento bom, melhorou bastante" }
            };

            mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(feedbackAtualizado);

            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            _output.WriteLine("********************( Chamando o serviço Atualização parcial do feedback )********************\n");

            var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

            _output.WriteLine($"Feedback parcialmente atualizado: Nota={resultado?.Nota}, Sentimento={resultado?.Sentimento}\n");
            Assert.NotNull(resultado);
            Assert.Equal("4", resultado?.Nota);
            Assert.Equal("Positivo", resultado?.Sentimento);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
        }

        /*************************************************************************************************

         Excluir Feedback

         Teste para garantir que o método Excluir seja chamado corretamente

        **************************************************************************************************/

        [Fact]
        public async Task Excluir_FeedbackValido_ChamaMetodoExcluirNoRepositorio()
        {
            var mockRepo = new Mock<IFeedbackRepository>();
            var mockSentimentService = new Mock<ISentimentAnalysisService>();
            var feedbackId = "123";
            var service = new FeedbackService(mockRepo.Object, mockSentimentService.Object);

            mockRepo.Setup(repo => repo.Excluir(feedbackId)).Returns(Task.CompletedTask);

            await service.Excluir(feedbackId);

            _output.WriteLine("********************( Chamando o serviço Exclusão do feedback )********************\n");

            _output.WriteLine($"Feedback com ID {feedbackId} excluído\n");

            mockRepo.Verify(repo => repo.Excluir(feedbackId), Times.Once);

            _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
        }
    }
}