
using Project.Repositories;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Tests.UnitTests.Repositories
{
    public class CategoriaRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public CategoriaRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************
         * Criar Categoria - Repositório
         * Teste que garante que o método Criar insere corretamente um Categoria no banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Criar_Categoria()
        {   
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new CategoriaRepository(optionsConfig);

                // Criando o Categoria para inserção
                var categoria = new Categoria 
                { 
                    Descricao = "Medicamento", 
                   
                };

                _output.WriteLine($"👤 Categoria criado para inserção: {System.Text.Json.JsonSerializer.Serialize(categoria)}\n");

                // Executa o método que insere o Categoria
                var resultado = await repository.Criar(categoria);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}, Email={resultado.Descricao}\n");

                // Verifica se o Categoria inserido possui os dados corretos
                Assert.Equal(categoria.Descricao, resultado.Descricao);

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
            Teste que garante que o método ConsultarTodos retorna todos os Categorias do banco de testes.
         **************************************************************************************************/
        
        [Fact]
        public async Task Consultar_Todas_Categorias()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Categorias\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new CategoriaRepository(optionsConfig);

                _output.WriteLine("✅ Categorias criados com sucesso.\n");

                // Executa o método que consulta todos os Categorias
                var categorias = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Categorias retornados: {categorias.Count}\n");

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
         * Teste que garante que o método ConsultarId retorna o Categoria correto com base no ID fornecido.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Consultar_Categoria_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Categoria_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new CategoriaRepository(optionsConfig);

                // ID do Categoria que será consultado
                string categoriaId = "6802d91a7c1790dc372551af";

                // Executa o método que consulta o Categoria pelo ID
                var categoriaConsultado = await repository.ConsultarId(categoriaId);

                _output.WriteLine($"🎯 Categoria retornado: Descricao={categoriaConsultado.Descricao}\n");

                // Verifica se o Categoria consultado possui os dados corretos
                Assert.Equal(categoriaId, categoriaConsultado.Id);
                Assert.NotNull(categoriaConsultado.Descricao);

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
         * Atualizar Categoria - Repositório
         * Teste que garante que o método Atualizar modifica corretamente todos os dados de um Categoria existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                // Criando a instância do repositório com as configurações apontando para o banco de testes
                var repository = new CategoriaRepository(optionsConfig);

                // ID do Categoria que será atualizado
                string categoriaId = "6802d91a7c1790dc372551af";

                // Dados atualizados do Categoria
                var categoriaAtualizada = new Categoria 
                { 
                    Id = categoriaId,
                    Descricao = "Aliemento", 
                   
                };

                // Executa o método que atualiza o Categoria
                var resultado = await repository.Atualizar(categoriaAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Categoria atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                // Verifica se o Categoria atualizado possui os dados corretos
                Assert.NotNull(resultado);
                Assert.Equal(categoriaAtualizada.Descricao, resultado!.Descricao);

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
         * Teste que garante que o método AtualizarParcial modifica corretamente os dados especificados de um Categoria existente.
         **************************************************************************************************/
        
        
        [Fact]
        public async Task Atualizar_Categoria_Parcialmente()
        {
            try
            {

                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Categoria_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                // ID do Categoria que será atualizado
                string categoriaId = "6802d91a7c1790dc372551af";

                // Campos para atualização parcial
                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" },
                    { "email", "claudio_parcial@test.com" }
                };

                // Executa o método que atualiza parcialmente o Categoria
                var resultado = await repository.AtualizarParcial(categoriaId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Categoria parcialmente atualizado: Descricao={resultado.Descricao}\n");
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
         * Excluir Categoria - Repositório
         * Teste que garante que o método Excluir remove corretamente um Categoria existente do banco de testes.
         **************************************************************************************************/
        [Fact]
        public async Task Excluir_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "inserir a string de conexão aqui",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                // ID do Categoria que será excluído
                string categoriaId = "6802d91a7c1790dc372551af";

                // Executa o método que exclui o Categoria
                await repository.Excluir(categoriaId);

                _output.WriteLine("✅ Categoria excluído com sucesso.\n");

                // Verifica se o Categoria foi realmente excluído
                var categoriaConsultado = await repository.ConsultarId(categoriaId);
                Assert.Null(categoriaConsultado);

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
