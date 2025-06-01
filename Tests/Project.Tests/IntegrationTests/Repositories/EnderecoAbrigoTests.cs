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
    public class EnderecoAbrigoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public EnderecoAbrigoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Criar_EnderecoAbrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_EnderecoAbrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                var enderecoAbrigo = new EnderecoAbrigo
                {
                    IdAbrigo = "60a1b2c3d4e5f6789012345a",
                    CEP = "01234567"
                };

                _output.WriteLine($"👤 EnderecoAbrigo criado para inserção: {System.Text.Json.JsonSerializer.Serialize(enderecoAbrigo)}\n");

                var resultado = await repository.Criar(enderecoAbrigo);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: IdAbrigo={resultado.IdAbrigo}, CEP={resultado.CEP}\n");

                Assert.Equal(enderecoAbrigo.IdAbrigo, resultado.IdAbrigo);
                Assert.Equal(enderecoAbrigo.CEP, resultado.CEP);

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
        public async Task Consultar_Todos_EnderecosAbrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todos_EnderecosAbrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                var enderecosAbrigo = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de EnderecosAbrigo retornados: {enderecosAbrigo.Count}\n");
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
        public async Task Consultar_EnderecoAbrigo_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_EnderecoAbrigo_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                string enderecoAbrigoId = "683c3ccb5a59cb66d3037992";

                var enderecoAbrigoConsultado = await repository.ConsultarId(enderecoAbrigoId);

                _output.WriteLine($"🎯 EnderecoAbrigo retornado: CEP={enderecoAbrigoConsultado.CEP}\n");

                Assert.Equal(enderecoAbrigoId, enderecoAbrigoConsultado.Id);
                Assert.NotNull(enderecoAbrigoConsultado.CEP);

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
        public async Task Atualizar_EnderecoAbrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_EnderecoAbrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                string enderecoAbrigoId = "683c3ccb5a59cb66d3037992";

                var enderecoAbrigoAtualizado = new EnderecoAbrigo
                {
                    Id = enderecoAbrigoId,
                    IdAbrigo = "60a1b2c3d4e5f6789012345a",
                    CEP = "09876543"
                };

                var resultado = await repository.Atualizar(enderecoAbrigoAtualizado);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 EnderecoAbrigo atualizado: IdAbrigo={resultado.IdAbrigo}, CEP={resultado.CEP}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(enderecoAbrigoAtualizado.IdAbrigo, resultado!.IdAbrigo);
                Assert.Equal(enderecoAbrigoAtualizado.CEP, resultado.CEP);

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
        public async Task Atualizar_EnderecoAbrigo_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_EnderecoAbrigo_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                string enderecoAbrigoId = "683c3ccb5a59cb66d3037992";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "IdAbrigo", "60a1b2c3d4e5f6789012345a" },
                    { "CEP", "12345678" }
                };

                var resultado = await repository.AtualizarParcial(enderecoAbrigoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 EnderecoAbrigo parcialmente atualizado: IdAbrigo={resultado.IdAbrigo}, CEP={resultado.CEP}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["IdAbrigo"], resultado!.IdAbrigo);
                Assert.Equal(camposParaAtualizar["CEP"], resultado.CEP);

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
        public async Task Excluir_EnderecoAbrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_EnderecoAbrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    EnderecoAbrigoCollectionName = "t_endereco_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoAbrigoRepository(optionsConfig);

                string enderecoAbrigoId = "6802d91a7c1790dc372551af";

                await repository.Excluir(enderecoAbrigoId);

                _output.WriteLine("✅ EnderecoAbrigo excluído com sucesso.\n");

                var enderecoAbrigoConsultado = await repository.ConsultarId(enderecoAbrigoId);
                Assert.Null(enderecoAbrigoConsultado);

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