
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class TipoUsuarioRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public TipoUsuarioRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar TipoUsuario - Repositório
         * Teste que garante que o método Criar insere corretamente um TipoUsuario no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_TipoUsuario()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_TipoUsuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new TipoUsuarioRepository(optionsConfig);

                // Criando o TipoUsuario para inserção
                var tipoUsuario = new TipoUsuario 
                { 
                    Descricao = "Medicamento", 
                   
                };

                _output.WriteLine($"👤 TipoUsuario criado para inserção: {System.Text.Json.JsonSerializer.Serialize(tipoUsuario)}\n");

                // Executa o método que insere o TipoUsuario
                var resultado = await repository.Criar(tipoUsuario);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}, Email={resultado.Descricao}\n");

                // Verifica se o TipoUsuario inserido possui os dados corretos
                Assert.Equal(tipoUsuario.Descricao, resultado.Descricao);

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
            Teste que garante que o método ConsultarTodos retorna todos os TipoUsuarios do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_TipoUsuarios()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_TipoUsuarios\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new TipoUsuarioRepository(optionsConfig);

                _output.WriteLine("✅ TipoUsuarios criados com sucesso.\n");

                // Executa o método que consulta todos os TipoUsuarios
                var tipoUsuarios = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de TipoUsuarios retornados: {tipoUsuarios.Count}\n");

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
         * Teste que garante que o método ConsultarId retorna o TipoUsuario correto com base no ID fornecido.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Consultar_TipoUsuario_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_TipoUsuario_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new TipoUsuarioRepository(optionsConfig);

                // ID do TipoUsuario que será consultado
                string tipoUsuarioId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o TipoUsuario pelo ID
                var tipoUsuarioConsultado = await repository.ConsultarId(tipoUsuarioId);

                _output.WriteLine($"🎯 TipoUsuario retornado: Descricao={tipoUsuarioConsultado.Descricao}\n");

                // Verifica se o TipoUsuario consultado possui os dados corretos
                Assert.Equal(tipoUsuarioId, tipoUsuarioConsultado.Id);
                Assert.NotNull(tipoUsuarioConsultado.Descricao);

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
         * Atualizar TipoUsuario - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um TipoUsuario existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_TipoUsuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_TipoUsuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new TipoUsuarioRepository(optionsConfig);

                // ID do TipoUsuario que será atualizado
                string tipoUsuarioId = "6802d91a7c1790dc372551af";

                // Dados atualizados do TipoUsuario
                var tipoUsuarioAtualizada = new TipoUsuario 
                { 
                    Id = tipoUsuarioId,
                    Descricao = "Aliemento", 
                   
                };

                // Executa o método que atualiza o TipoUsuario
                var resultado = await repository.Atualizar(tipoUsuarioAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 TipoUsuario atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o TipoUsuario atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(tipoUsuarioAtualizada.Descricao, resultado!.Descricao);

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
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um TipoUsuario existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_TipoUsuario_Parcialmente()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_TipoUsuario_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new TipoUsuarioRepository(optionsConfig);

                // ID do TipoUsuario que será atualizado
                string tipoUsuarioId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o TipoUsuario
                var resultado = await repository.AtualizarParcial(tipoUsuarioId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 TipoUsuario parcialmente atualizado: Descricao={resultado.Descricao}\n");
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
         * Excluir TipoUsuario - Repositório
         * Teste que garante que o método Excluir remove corretamente um TipoUsuario existente do banco de testes.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_TipoUsuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_TipoUsuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    TipoUsuarioCollectionName = "t_tipoUsuario"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new TipoUsuarioRepository(optionsConfig);

                // ID do TipoUsuario que será excluído
                string tipoUsuarioId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o TipoUsuario
                await repository.Excluir(tipoUsuarioId);

                _output.WriteLine("✅ TipoUsuario excluído com sucesso.\n");

                // Verifica se o TipoUsuario foi realmente excluído
                var tipoUsuarioConsultado = await repository.ConsultarId(tipoUsuarioId);
                Assert.Null(tipoUsuarioConsultado);

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
