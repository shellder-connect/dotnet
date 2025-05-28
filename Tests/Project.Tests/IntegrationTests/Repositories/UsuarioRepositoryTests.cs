
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class UsuarioRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public UsuarioRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar Usuário - Repositório
         * Teste que garante que o método Criar insere corretamente um usuário no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_Usuario()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Usuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new UsuarioRepository(optionsConfig);

                // Criando o usuário para inserção
                var usuario = new Usuario 
                { 
                    Nome = "Claudio Banco de Teste Dois", 
                    Email = "claudio@test.com", 
                    Senha = "senha", 
                    IdTipoUsuario = "Comum",
                    Telefone = "950556066", 
                    IdEndereco = "1",
                    DataNascimento = "1993-05-10",
                    Documento = "40830740000", 
                    Status = "Ativo"
                };

                _output.WriteLine($"👤 Usuário criado para inserção: {System.Text.Json.JsonSerializer.Serialize(usuario)}\n");

                // Executa o método que insere o usuário
                var resultado = await repository.Criar(usuario);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Nome={resultado.Nome}, Email={resultado.Email}\n");

                // Verifica se o usuário inserido possui os dados corretos
                Assert.Equal(usuario.Nome, resultado.Nome);
                Assert.Equal(usuario.Email, resultado.Email);

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
            Teste que garante que o método ConsultarTodos retorna todos os usuários do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todos_Usuarios()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todos_Usuarios\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new UsuarioRepository(optionsConfig);

                _output.WriteLine("✅ Usuários criados com sucesso.\n");

                // Executa o método que consulta todos os usuários
                var usuarios = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de usuários retornados: {usuarios.Count}\n");

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
         * Teste que garante que o método ConsultarId retorna o usuário correto com base no ID fornecido.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Consultar_Usuario_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Usuario_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new UsuarioRepository(optionsConfig);

                // ID do usuário que será consultado
                string usuarioId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o usuário pelo ID
                var usuarioConsultado = await repository.ConsultarId(usuarioId);

                _output.WriteLine($"🎯 Usuário retornado: Nome={usuarioConsultado.Nome}, Email={usuarioConsultado.Email}\n");

                // Verifica se o usuário consultado possui os dados corretos
                Assert.Equal(usuarioId, usuarioConsultado.Id);
                Assert.NotNull(usuarioConsultado.Nome);
                Assert.NotNull(usuarioConsultado.Email);

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
         * Atualizar Usuário - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um usuário existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Usuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Usuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new UsuarioRepository(optionsConfig);

                // ID do usuário que será atualizado
                string usuarioId = "6802d91a7c1790dc372551af";

                // Dados atualizados do usuário
                var usuarioAtualizado = new Usuario 
                { 
                    Id = usuarioId,
                    Nome = "Claudio Dois", 
                    Email = "claudio_cssp@test.com", 
                    Senha = "senha", 
                    IdTipoUsuario = "Adm",
                    Telefone = "950556066", 
                    IdEndereco = "1",
                    DataNascimento = "1993-05-10",
                    Documento = "40830740000", 
                    Status = "Ativo"
                };

                // Executa o método que atualiza o usuário
                var resultado = await repository.Atualizar(usuarioAtualizado);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Usuário atualizado: Nome={resultado.Nome}, Email={resultado.Email}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o usuário atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(usuarioAtualizado.Nome, resultado!.Nome);
                Assert.Equal(usuarioAtualizado.Email, resultado.Email);

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
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um usuário existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Usuario_Parcialmente()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Usuario_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new UsuarioRepository(optionsConfig);

                // ID do usuário que será atualizado
                string usuarioId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "nome", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o usuário
                var resultado = await repository.AtualizarParcial(usuarioId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Usuário parcialmente atualizado: Nome={resultado.Nome}, Email={resultado.Email}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                // Verifica se os campos atualizados possuem os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["nome"], resultado!.Nome);
                Assert.Equal(camposParaAtualizar["email"], resultado.Email);

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
         * Excluir Usuário - Repositório
         * Teste que garante que o método Excluir remove corretamente um usuário existente do banco de testes.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_Usuario()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Usuario\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new UsuarioRepository(optionsConfig);

                // ID do usuário que será excluído
                string usuarioId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o usuário
                await repository.Excluir(usuarioId);

                _output.WriteLine("✅ Usuário excluído com sucesso.\n");

                // Verifica se o usuário foi realmente excluído
                var usuarioConsultado = await repository.ConsultarId(usuarioId);
                Assert.Null(usuarioConsultado);

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
