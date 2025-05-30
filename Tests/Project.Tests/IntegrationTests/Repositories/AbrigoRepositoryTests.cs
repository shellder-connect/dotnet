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
    public class AbrigoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public AbrigoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Criar_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                var abrigo = new Abrigo
                {
                    Descricao = "Abrigo Teste",
                    CapacidadeTotal = 100,
                    OcupacaoAtual = 25
                };

                _output.WriteLine($"👤 Abrigo criado para inserção: {System.Text.Json.JsonSerializer.Serialize(abrigo)}\n");

                var resultado = await repository.Criar(abrigo);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}, CapacidadeTotal={resultado.CapacidadeTotal}, OcupacaoAtual={resultado.OcupacaoAtual}\n");

                Assert.Equal(abrigo.Descricao, resultado.Descricao);
                Assert.Equal(abrigo.CapacidadeTotal, resultado.CapacidadeTotal);
                Assert.Equal(abrigo.OcupacaoAtual, resultado.OcupacaoAtual);

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
        public async Task Consultar_Todas_Abrigos()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Abrigos\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                var abrigos = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Abrigos retornados: {abrigos.Count}\n");
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
        public async Task Consultar_Abrigo_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Abrigo_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                string abrigoId = "683a44470f3c2967f9c6ebb3";

                var abrigoConsultado = await repository.ConsultarId(abrigoId);

                _output.WriteLine($"🎯 Abrigo retornado: Descricao={abrigoConsultado.Descricao}\n");

                Assert.Equal(abrigoId, abrigoConsultado.Id);
                Assert.NotNull(abrigoConsultado.Descricao);

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
        public async Task Atualizar_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                string abrigoId = "683a44470f3c2967f9c6ebb3";

                var abrigoAtualizado = new Abrigo
                {
                    Id = abrigoId,
                    Descricao = "Abrigo Atualizado",
                    CapacidadeTotal = 150,
                    OcupacaoAtual = 50
                };

                var resultado = await repository.Atualizar(abrigoAtualizado);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Abrigo atualizado: Descricao={resultado.Descricao}, CapacidadeTotal={resultado.CapacidadeTotal}, OcupacaoAtual={resultado.OcupacaoAtual}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(abrigoAtualizado.Descricao, resultado!.Descricao);
                Assert.Equal(abrigoAtualizado.CapacidadeTotal, resultado.CapacidadeTotal);
                Assert.Equal(abrigoAtualizado.OcupacaoAtual, resultado.OcupacaoAtual);

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
        public async Task Atualizar_Abrigo_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Abrigo_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                string abrigoId = "683a44470f3c2967f9c6ebb3";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Abrigo Parcialmente Atualizado" },
                    { "CapacidadeTotal", 100 },
                    { "OcupacaoAtual", 25 }
                };

                var resultado = await repository.AtualizarParcial(abrigoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Abrigo parcialmente atualizado: Descricao={resultado.Descricao}, CapacidadeTotal={resultado.CapacidadeTotal}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["Descricao"], resultado!.Descricao);
                Assert.Equal(camposParaAtualizar["CapacidadeTotal"], resultado.CapacidadeTotal);
                Assert.Equal(camposParaAtualizar["OcupacaoAtual"], resultado.OcupacaoAtual);

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
        public async Task Excluir_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                string abrigoId = "6802d91a7c1790dc372551af";

                await repository.Excluir(abrigoId);

                _output.WriteLine("✅ Abrigo excluído com sucesso.\n");

                var abrigoConsultado = await repository.ConsultarId(abrigoId);
                Assert.Null(abrigoConsultado);

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
