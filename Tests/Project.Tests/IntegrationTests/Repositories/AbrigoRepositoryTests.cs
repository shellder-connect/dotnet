
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class AbrigoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public AbrigoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar Abrigo - Repositório
         * Teste que garante que o método Criar insere corretamente um Abrigo no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_Abrigo()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new AbrigoRepository(optionsConfig);

                // Criando o Abrigo para inserção
                var abrigo = new Abrigo 
                { 
                    Descricao = "Medicamento", 
                   
                };

                _output.WriteLine($"👤 Abrigo criado para inserção: {System.Text.Json.JsonSerializer.Serialize(abrigo)}\n");

                // Executa o método que insere o Abrigo
                var resultado = await repository.Criar(abrigo);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}, Email={resultado.Descricao}\n");

                // Verifica se o Abrigo inserido possui os dados corretos
                Assert.Equal(abrigo.Descricao, resultado.Descricao);

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
            Teste que garante que o método ConsultarTodos retorna todos os Abrigos do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_Abrigos()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Abrigos\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new AbrigoRepository(optionsConfig);

                _output.WriteLine("✅ Abrigos criados com sucesso.\n");

                // Executa o método que consulta todos os Abrigos
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
        
        
        /*************************************************************************************************
         * Consultar por ID - Repositório
         * Teste que garante que o método ConsultarId retorna o Abrigo correto com base no ID fornecido.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Consultar_Abrigo_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Abrigo_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new AbrigoRepository(optionsConfig);

                // ID do Abrigo que será consultado
                string abrigoId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o Abrigo pelo ID
                var abrigoConsultado = await repository.ConsultarId(abrigoId);

                _output.WriteLine($"🎯 Abrigo retornado: Descricao={abrigoConsultado.Descricao}\n");

                // Verifica se o Abrigo consultado possui os dados corretos
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
        
        
        /*************************************************************************************************
         * Atualizar Abrigo - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um Abrigo existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new AbrigoRepository(optionsConfig);

                // ID do Abrigo que será atualizado
                string abrigoId = "6802d91a7c1790dc372551af";

                // Dados atualizados do Abrigo
                var abrigoAtualizada = new Abrigo 
                { 
                    Id = abrigoId,
                    Descricao = "Aliemento", 
                   
                };

                // Executa o método que atualiza o Abrigo
                var resultado = await repository.Atualizar(abrigoAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Abrigo atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o Abrigo atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(abrigoAtualizada.Descricao, resultado!.Descricao);

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
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um Abrigo existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Abrigo_Parcialmente()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Abrigo_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                // ID do Abrigo que será atualizado
                string abrigoId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o Abrigo
                var resultado = await repository.AtualizarParcial(abrigoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Abrigo parcialmente atualizado: Descricao={resultado.Descricao}\n");
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
         * Excluir Abrigo - Repositório
         * Teste que garante que o método Excluir remove corretamente um Abrigo existente do banco de testes.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_Abrigo()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Abrigo\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    AbrigoCollectionName = "t_abrigo"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new AbrigoRepository(optionsConfig);

                // ID do Abrigo que será excluído
                string abrigoId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o Abrigo
                await repository.Excluir(abrigoId);

                _output.WriteLine("✅ Abrigo excluído com sucesso.\n");

                // Verifica se o Abrigo foi realmente excluído
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
