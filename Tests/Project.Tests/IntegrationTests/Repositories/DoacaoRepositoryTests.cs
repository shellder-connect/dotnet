
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

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

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new DoacaoRepository(optionsConfig);

                // Criando o Doacao para inserção
                var doacao = new Doacao 
                { 
                    Descricao = "Medicamento", 
                   
                };

                _output.WriteLine($"👤 Doacao criado para inserção: {System.Text.Json.JsonSerializer.Serialize(doacao)}\n");

                // Executa o método que insere o Doacao
                var resultado = await repository.Criar(doacao);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}, Email={resultado.Descricao}\n");

                // Verifica se o Doacao inserido possui os dados corretos
                Assert.Equal(doacao.Descricao, resultado.Descricao);

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
            Consultar Todos - Repositório
            Teste que garante que o método ConsultarTodos retorna todos os Doacaos do banco de testes.
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

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new DoacaoRepository(optionsConfig);

                _output.WriteLine("✅ Doacaos criados com sucesso.\n");

                // Executa o método que consulta todos os Doacaos
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

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new DoacaoRepository(optionsConfig);

                // ID do Doacao que será consultado
                string doacaoId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o Doacao pelo ID
                var doacaoConsultado = await repository.ConsultarId(doacaoId);

                _output.WriteLine($"🎯 Doacao retornado: Descricao={doacaoConsultado.Descricao}\n");

                // Verifica se o Doacao consultado possui os dados corretos
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

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new DoacaoRepository(optionsConfig);

                // ID do Doacao que será atualizado
                string doacaoId = "6802d91a7c1790dc372551af";

                // Dados atualizados do Doacao
                var doacaoAtualizada = new Doacao 
                { 
                    Id = doacaoId,
                    Descricao = "Aliemento", 
                   
                };

                // Executa o método que atualiza o Doacao
                var resultado = await repository.Atualizar(doacaoAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Doacao atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o Doacao atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(doacaoAtualizada.Descricao, resultado!.Descricao);

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

                // ID do Doacao que será atualizado
                string doacaoId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o Doacao
                var resultado = await repository.AtualizarParcial(doacaoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Doacao parcialmente atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                // Verifica se os campos atualizados possuem os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["Descricao"], resultado!.Descricao);

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

                // ID do Doacao que será excluído
                string doacaoId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o Doacao
                await repository.Excluir(doacaoId);

                _output.WriteLine("✅ Doacao excluído com sucesso.\n");

                // Verifica se o Doacao foi realmente excluído
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
