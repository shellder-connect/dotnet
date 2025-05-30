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
    public class DoacaoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public DoacaoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar Doacao - Repositório
         * Teste que garante que o método Criar insere corretamente um Doacao no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_Doacao()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Doacao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                var doacao = new Doacao 
                { 
                    IdAbrigo = "607f1f77bcf86cd799439011",
                    Descricao = "Medicamento", 
                    IdCategoria = "607f1f77bcf86cd799439012",
                    Quantidade = 10
                };

                _output.WriteLine($"👤 Doacao criado para inserção: {System.Text.Json.JsonSerializer.Serialize(doacao)}\n");

                var resultado = await repository.Criar(doacao);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Id={resultado.Id}, Descricao={resultado.Descricao}, Quantidade={resultado.Quantidade}\n");

                Assert.Equal(doacao.Descricao, resultado.Descricao);
                Assert.Equal(doacao.IdAbrigo, resultado.IdAbrigo);
                Assert.Equal(doacao.IdCategoria, resultado.IdCategoria);
                Assert.Equal(doacao.Quantidade, resultado.Quantidade);

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
         * Consultar Todos - Repositório
         * Teste que garante que o método ConsultarTodos retorna todos os Doacaos do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_Doacaos()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Doacaos\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                var doacaos = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Doacaos retornados: {doacaos.Count}\n");

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
         * Consultar por ID - Repositório
         * Teste que garante que o método ConsultarId retorna o Doacao correto com base no ID fornecido.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Doacao_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Doacao_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                string doacaoId = "6802d91a7c1790dc372551af";

                var doacaoConsultado = await repository.ConsultarId(doacaoId);

                _output.WriteLine($"🎯 Doacao retornado: Id={doacaoConsultado.Id}, Descricao={doacaoConsultado.Descricao}, Quantidade={doacaoConsultado.Quantidade}\n");

                Assert.Equal(doacaoId, doacaoConsultado.Id);
                Assert.NotNull(doacaoConsultado.Descricao);

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
         * Atualizar Doacao - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um Doacao existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Atualizar_Doacao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Doacao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                string doacaoId = "6802d91a7c1790dc372551af";

                var doacaoAtualizada = new Doacao 
                { 
                    Id = doacaoId,
                    IdAbrigo = "607f1f77bcf86cd799439011",
                    Descricao = "Alimento Atualizado", 
                    IdCategoria = "607f1f77bcf86cd799439012",
                    Quantidade = 20
                };

                var resultado = await repository.Atualizar(doacaoAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Doacao atualizado: Id={resultado.Id}, Descricao={resultado.Descricao}, Quantidade={resultado.Quantidade}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(doacaoAtualizada.Descricao, resultado!.Descricao);
                Assert.Equal(doacaoAtualizada.IdAbrigo, resultado.IdAbrigo);
                Assert.Equal(doacaoAtualizada.IdCategoria, resultado.IdCategoria);
                Assert.Equal(doacaoAtualizada.Quantidade, resultado.Quantidade);

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
         * Atualizar Parcialmente - Repositório
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um Doacao existente.
         **************************************************************************************************/
        
        [Fact]
        public async Task Atualizar_Doacao_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Doacao_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                string doacaoId = "6802d91a7c1790dc372551af";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "Quantidade", 15 }
                };

                var resultado = await repository.AtualizarParcial(doacaoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Doacao parcialmente atualizado: Descricao={resultado.Descricao}, Quantidade={resultado.Quantidade}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["Descricao"], resultado!.Descricao);
                Assert.Equal(camposParaAtualizar["Quantidade"], resultado.Quantidade);

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
         * Excluir Doacao - Repositório
         * Teste que garante que o método Excluir remove corretamente um Doacao existente do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Excluir_Doacao()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Doacao\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    DoacaoCollectionName = "t_doacao"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new DoacaoRepository(optionsConfig);

                string doacaoId = "6802d91a7c1790dc372551af";

                await repository.Excluir(doacaoId);

                _output.WriteLine("✅ Doacao excluído com sucesso.\n");

                var doacaoConsultado = await repository.ConsultarId(doacaoId);
                Assert.Null(doacaoConsultado);

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
