using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class DistribuicaoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public DistribuicaoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Criar_Distribuicao()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Distribuicao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                var distribuicao = new Distribuicao
                {
                    IdDoacao = "665af8b13fae4c001fcf6d12",
                    QuantidadeDestinada = 10,
                    DataDestinada = DateTime.Parse("2025-06-01T10:00:00"),
                    IdPessoaAtendida = "665af9a03fae4c001fcf6d15"
                };

                _output.WriteLine($"👤 Distribuicao criado para inserção: {System.Text.Json.JsonSerializer.Serialize(distribuicao)}\n");

                var resultado = await repository.Criar(distribuicao);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: QuantidadeDestinada={resultado.QuantidadeDestinada}, DataDestinada={resultado.DataDestinada}\n");

                Assert.Equal(distribuicao.IdDoacao, resultado.IdDoacao);

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
        public async Task Consultar_Todas_Distribuicaos()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Distribuicaos\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                var distribuicaos = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Distribuicaos retornados: {distribuicaos.Count}\n");

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
        public async Task Consultar_Distribuicao_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Distribuicao_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                string distribuicaoId = "6802d91a7c1790dc372551af";

                var distribuicaoConsultado = await repository.ConsultarId(distribuicaoId);

                _output.WriteLine($"🎯 Distribuicao retornado: IdDoacao={distribuicaoConsultado.IdDoacao}\n");

                Assert.Equal(distribuicaoId, distribuicaoConsultado.Id);
                Assert.NotNull(distribuicaoConsultado.IdDoacao);

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
        public async Task Atualizar_Distribuicao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Distribuicao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                string distribuicaoId = "6802d91a7c1790dc372551af";

                var distribuicaoAtualizada = new Distribuicao 
                { 
                    Id = distribuicaoId,
                    IdDoacao = "665af8b13fae4c001fcf6d12",
                    QuantidadeDestinada = 20,
                    DataDestinada = DateTime.Parse("2025-07-01T10:00:00"),
                    IdPessoaAtendida = "665af9a03fae4c001fcf6d16"
                };

                var resultado = await repository.Atualizar(distribuicaoAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Distribuicao atualizado: QuantidadeDestinada={resultado.QuantidadeDestinada}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(distribuicaoAtualizada.QuantidadeDestinada, resultado!.QuantidadeDestinada);

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
        public async Task Atualizar_Distribuicao_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Distribuicao_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                string distribuicaoId = "6802d91a7c1790dc372551af";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "QuantidadeDestinada", 30 },
                    { "IdPessoaAtendida", "665af9a03fae4c001fcf6d99" }
                };

                var resultado = await repository.AtualizarParcial(distribuicaoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Distribuicao parcialmente atualizado: QuantidadeDestinada={resultado.QuantidadeDestinada}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["QuantidadeDestinada"], resultado!.QuantidadeDestinada);

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
        public async Task Excluir_Distribuicao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Distribuicao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DistribuicaoCollectionName = "t_distribuicao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DistribuicaoRepository(optionsConfig);

                string distribuicaoId = "6802d91a7c1790dc372551af";

                await repository.Excluir(distribuicaoId);

                _output.WriteLine("✅ Distribuicao excluído com sucesso.\n");

                var distribuicaoConsultado = await repository.ConsultarId(distribuicaoId);
                Assert.Null(distribuicaoConsultado);

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
