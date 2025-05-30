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

        [Fact]
        public async Task Criar_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                var categoria = new Categoria
                {
                    Descricao = "Medicamento"
                };

                _output.WriteLine($"👤 Categoria criado para inserção: {System.Text.Json.JsonSerializer.Serialize(categoria)}\n");

                var resultado = await repository.Criar(categoria);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Descricao={resultado.Descricao}\n");

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

        [Fact]
        public async Task Consultar_Todas_Categorias()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todas_Categorias\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

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

        [Fact]
        public async Task Consultar_Categoria_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Categoria_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                string categoriaId = "683a34fe6333f0107e656ff1";

                var categoriaConsultado = await repository.ConsultarId(categoriaId);

                _output.WriteLine($"🎯 Categoria retornado: Descricao={categoriaConsultado?.Descricao}\n");

                Assert.Equal(categoriaId, categoriaConsultado?.Id);
                Assert.NotNull(categoriaConsultado?.Descricao);

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
        public async Task Atualizar_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                string categoriaId = "683a34fe6333f0107e656ff1";

                var categoriaAtualizada = new Categoria
                {
                    Id = categoriaId,
                    Descricao = "Alimento"
                };

                var resultado = await repository.Atualizar(categoriaAtualizada);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Categoria atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

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

        [Fact]
        public async Task Atualizar_Categoria_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Categoria_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                string categoriaId = "683a34fe6333f0107e656ff1";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Descricao", "Claudio Parcialmente Atualizado" }
                };

                var resultado = await repository.AtualizarParcial(categoriaId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Categoria parcialmente atualizado: Descricao={resultado.Descricao}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

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

        [Fact]
        public async Task Excluir_Categoria()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Categoria\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    CategoriaCollectionName = "t_categoria"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new CategoriaRepository(optionsConfig);

                string categoriaId = "6802d91a7c1790dc372551af";

                await repository.Excluir(categoriaId);

                _output.WriteLine("✅ Categoria excluído com sucesso.\n");

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
