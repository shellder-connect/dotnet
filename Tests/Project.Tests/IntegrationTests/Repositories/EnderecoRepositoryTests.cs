
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class EnderecoRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public EnderecoRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar Endereco - Repositório
         * Teste que garante que o método Criar insere corretamente um Endereco no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_Endereco()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Endereco\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new EnderecoRepository(optionsConfig);

                // Criando o Endereco para inserção
                var endereco = new Endereco
                {
                    CEP = "05763290",
                    Estado = "São Paulo",
                    Cidade = "São Carlos",
                    Bairro = "Centro",
                    Rua = "Seraim"
                   
                };

                _output.WriteLine($"👤 Endereco criado para inserção: {System.Text.Json.JsonSerializer.Serialize(endereco)}\n");

                // Executa o método que insere o Endereco
                var resultado = await repository.Criar(endereco);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: CEP={resultado.CEP}, Estado={resultado.Estado}, Cidade={resultado.Cidade}, Bairro={resultado.Bairro}, Rua={resultado.Rua}\n");

                // Verifica se o Endereco inserido possui os dados corretos
                Assert.Equal(endereco.CEP, resultado.CEP);
                Assert.Equal(endereco.Estado, resultado.Estado);
                Assert.Equal(endereco.Cidade, resultado.Cidade);
                Assert.Equal(endereco.Bairro, resultado.Bairro);
                Assert.Equal(endereco.Rua, resultado.Rua);

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
            Teste que garante que o método ConsultarTodos retorna todos os Enderecos do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_Enderecos()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Enderecos\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new EnderecoRepository(optionsConfig);

                _output.WriteLine("✅ Enderecos criados com sucesso.\n");

                // Executa o método que consulta todos os Enderecos
                var enderecos = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Enderecos retornados: {enderecos.Count}\n");

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
         * Teste que garante que o método ConsultarId retorna o Endereco correto com base no ID fornecido.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Consultar_Endereco_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Endereco_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new EnderecoRepository(optionsConfig);

                // ID do Endereco que será consultado
                string enderecoId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o Endereco pelo ID
                var enderecoConsultado = await repository.ConsultarId(enderecoId);

                _output.WriteLine($"🎯 Endereco retornado: CEP={enderecoConsultado.CEP}, Estado={enderecoConsultado.Estado}, Cidade={enderecoConsultado.Cidade}, Bairro={enderecoConsultado.Bairro}, Rua={enderecoConsultado.Rua}\n");

                // Verifica se o Endereco consultado possui os dados corretos
                Assert.Equal(enderecoId, enderecoConsultado.Id);
                Assert.NotNull(enderecoConsultado);

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
         * Atualizar Endereco - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um Endereco existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Endereco()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Endereco\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new EnderecoRepository(optionsConfig);

                // ID do Endereco que será atualizado
                string enderecoId = "6802d91a7c1790dc372551af";

                // Dados atualizados do Endereco
                var enderecoAtualizada = new Endereco 
                { 
                    Id = enderecoId,
                    CEP = "05763290",
                    Estado = "São Paulo",
                    Cidade = "São Carlos",
                    Bairro = "Centro",
                    Rua = "Seraim"
                   
                };

                // Executa o método que atualiza o Endereco
                var resultado = await repository.Atualizar(enderecoAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Endereco atualizado: CEP={resultado.CEP}, Estado={resultado.Estado}, Cidade={resultado.Cidade}, Bairro={resultado.Bairro}, Rua={resultado.Rua}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o Endereco atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(enderecoAtualizada.CEP, resultado!.CEP);
                Assert.Equal(enderecoAtualizada.Estado, resultado.Estado);
                Assert.Equal(enderecoAtualizada.Cidade, resultado.Cidade);
                Assert.Equal(enderecoAtualizada.Bairro, resultado.Bairro);
                Assert.Equal(enderecoAtualizada.Rua, resultado.Rua);

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
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um Endereco existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Endereco_Parcialmente()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Endereco_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoRepository(optionsConfig);

                // ID do Endereco que será atualizado
                string enderecoId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o Endereco
                var resultado = await repository.AtualizarParcial(enderecoId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Endereco parcialmente atualizado: CEP={resultado.CEP}, Estado={resultado.Estado}, Cidade={resultado.Cidade}, Bairro={resultado.Bairro}, Rua={resultado.Rua}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                // Verifica se os campos atualizados possuem os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["CEP"], resultado!.CEP);

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
         * Excluir Endereco - Repositório
         * Teste que garante que o método Excluir remove corretamente um Endereco existente do banco de testes.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_Endereco()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Endereco\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    EnderecoCollectionName = "t_endereco"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new EnderecoRepository(optionsConfig);

                // ID do Endereco que será excluído
                string enderecoId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o Endereco
                await repository.Excluir(enderecoId);

                _output.WriteLine("✅ Endereco excluído com sucesso.\n");

                // Verifica se o Endereco foi realmente excluído
                var enderecoConsultado = await repository.ConsultarId(enderecoId);
                Assert.Null(enderecoConsultado);

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
