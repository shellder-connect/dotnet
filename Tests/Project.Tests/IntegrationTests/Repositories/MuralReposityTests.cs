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
    public class MuralRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public MuralRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Criar_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Criar_Mural\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                var mural = new Mural
                {
                    IdUsuario = "60a1b2c3d4e5f6789012345a",
                    Titulo = "Post de Teste",
                    Comentario = "Este é um comentário de teste para o mural",
                    TipoPost = "Noticia",
                    Data = DateTime.Now
                };

                _output.WriteLine($"👤 Mural criado para inserção: {System.Text.Json.JsonSerializer.Serialize(mural)}\n");

                var resultado = await repository.Criar(mural);

                _output.WriteLine("✅ Método Criar chamado com sucesso.\n");
                _output.WriteLine($"🎯 Resultado retornado: Titulo={resultado.Titulo}, TipoPost={resultado.TipoPost}, Data={resultado.Data}\n");

                Assert.Equal(mural.IdUsuario, resultado.IdUsuario);
                Assert.Equal(mural.Titulo, resultado.Titulo);
                Assert.Equal(mural.Comentario, resultado.Comentario);
                Assert.Equal(mural.TipoPost, resultado.TipoPost);

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
        public async Task Consultar_Todos_Murais()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Todos_Murais\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                var murais = await repository.ConsultarTodos();

                _output.WriteLine($"🎯 Total de Murais retornados: {murais.Count}\n");
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
        public async Task Consultar_Mural_Por_Id()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Consultar_Mural_Por_Id\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                string muralId = "683c34898c427f6cd3f7d0e2";

                var muralConsultado = await repository.ConsultarId(muralId);

                _output.WriteLine($"🎯 Mural retornado: Titulo={muralConsultado.Titulo}\n");

                Assert.Equal(muralId, muralConsultado.Id);
                Assert.NotNull(muralConsultado.Titulo);

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
        public async Task Atualizar_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Mural\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                string muralId = "683c34898c427f6cd3f7d0e2";

                var muralAtualizado = new Mural
                {
                    Id = muralId,
                    IdUsuario = "60a1b2c3d4e5f6789012345b",
                    Titulo = "Post Atualizado",
                    Comentario = "Comentário atualizado para o teste",
                    TipoPost = "Evento",
                    Data = DateTime.Now
                };

                var resultado = await repository.Atualizar(muralAtualizado);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Mural atualizado: Titulo={resultado.Titulo}, TipoPost={resultado.TipoPost}, Data={resultado.Data}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(muralAtualizado.IdUsuario, resultado!.IdUsuario);
                Assert.Equal(muralAtualizado.Titulo, resultado.Titulo);
                Assert.Equal(muralAtualizado.Comentario, resultado.Comentario);
                Assert.Equal(muralAtualizado.TipoPost, resultado.TipoPost);

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
        public async Task Atualizar_Mural_Parcialmente()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Atualizar_Mural_Parcialmente\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                string muralId = "683c34b9a51c4949f7b62d26";

                var camposParaAtualizar = new Dictionary<string, object>
                {
                    { "Titulo", "Post de Teste" },
                    { "Comentario", "Este é um comentário de teste para o mural" },
                    { "TipoPost", "Noticia" }
                };

                var resultado = await repository.AtualizarParcial(muralId, camposParaAtualizar);

                if (resultado != null)
                {
                    _output.WriteLine($"🎯 Mural parcialmente atualizado: Titulo={resultado.Titulo}, TipoPost={resultado.TipoPost}\n");
                }
                else
                {
                    _output.WriteLine("❌ Erro: O resultado da atualização parcial é nulo.\n");
                }

                Assert.NotNull(resultado);
                Assert.Equal(camposParaAtualizar["Titulo"], resultado!.Titulo);
                Assert.Equal(camposParaAtualizar["Comentario"], resultado.Comentario);
                Assert.Equal(camposParaAtualizar["TipoPost"], resultado.TipoPost);

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
        public async Task Excluir_Mural()
        {
            try
            {
                _output.WriteLine("*****************************************************\n");
                _output.WriteLine("🔌 Iniciando teste: Excluir_Mural\n");

                var settings = new ConfigMongoDb
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "TestsDb",
                    MuralCollectionName = "t_mural"
                };

                var optionsConfig = Options.Create(settings);

                var repository = new MuralRepository(optionsConfig);

                string muralId = "6802d91a7c1790dc372551af";

                await repository.Excluir(muralId);

                _output.WriteLine("✅ Mural excluído com sucesso.\n");

                var muralConsultado = await repository.ConsultarId(muralId);
                Assert.Null(muralConsultado);

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