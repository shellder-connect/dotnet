using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests.Repositories
{
    public class FeedbackRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public FeedbackRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Criar_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Feedback\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                var feedback = new Feedback
                {
                    Nota = "5",
                    Comentario = "Excelente atendimento, muito satisfeito com o serviço prestado",
                    Sentimento = "bom"
                };

                _output.WriteLine($"👤 Feedback criado para inserção: {System.Text.Json.JsonSerializer.Serialize(feedback)}\n");

                var resultado = await repository.Criar(feedback);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Nota={resultado.Nota}, Comentario={resultado.Comentario}, Sentimento={resultado.Sentimento}\n");

                Assert.Equal(feedback.Nota, resultado.Nota);
                Assert.Equal(feedback.Comentario, resultado.Comentario);
                Assert.Equal(feedback.Sentimento, resultado.Sentimento);

                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task Consultar_Todos_Feedbacks()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todos_Feedbacks\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                var feedbacks = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Feedbacks retornados: {feedbacks.Count}\n");
                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task Consultar_Feedback_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Feedback_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                string feedbackId = "683c4080b167f883ab597482";

                var feedbackConsultado = await repository.ConsultarId(feedbackId);

                _output.WriteLine($"🎯 Feedback retornado: Nota={feedbackConsultado.Nota}, Comentario={feedbackConsultado.Comentario}\n");

                Assert.Equal(feedbackId, feedbackConsultado.Id);
                Assert.NotNull(feedbackConsultado.Nota);

                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task Atualizar_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Feedback\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                string feedbackId = "683c4080b167f883ab597482";

                var feedbackAtualizado = new Feedback
                {
                    Id = feedbackId,
                    Nota = "3",
                    Comentario = "Atendimento regular, pode melhorar em alguns aspectos",
                    Sentimento = "bom"
                };

                var resultado = await repository.Atualizar(feedbackAtualizado);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Feedback atualizado: Nota={resultado.Nota}, Comentario={resultado.Comentario}, Sentimento={resultado.Sentimento}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(feedbackAtualizado.Nota, resultado!.Nota);
                Assert.Equal(feedbackAtualizado.Comentario, resultado.Comentario);
                Assert.Equal(feedbackAtualizado.Sentimento, resultado.Sentimento);

                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task Atualizar_Feedback_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Feedback_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                string feedbackId = "683c4080b167f883ab597482";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Nota", "5" },
                    { "Comentario", "Excelente atendimento, muito satisfeito com o serviço prestado" },
                    { "Sentimento", "bom" }
                };

                var resultado = await repository.AtualizarParcial(feedbackId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Feedback parcialmente atualizado: Nota={resultado.Nota}, Sentimento={resultado.Sentimento}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["Nota"], resultado!.Nota);
                Assert.Equal(camposParaAtualizar["Comentario"], resultado.Comentario);
                Assert.Equal(camposParaAtualizar["Sentimento"], resultado.Sentimento);

                _output.WriteLine("🔚 Teste finalizado com sucesso.\n");
                _output.WriteLine("*****************************************************\n");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task Excluir_Feedback()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Feedback\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    FeedbackCollectionName = "t_feedback"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new FeedbackRepository(optionsConfig);

                string feedbackId = "6802d91a7c1790dc372551af";

                await repository.Excluir(feedbackId);

                _output.WriteLine("✅ Feedback excluído com sucesso.\n");

                var feedbackConsultado = await repository.ConsultarId(feedbackId);
                Assert.Null(feedbackConsultado);

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