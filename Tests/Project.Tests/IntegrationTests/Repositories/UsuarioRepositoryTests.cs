using Moq;
using Project.Repositories;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Xunit;
using Xunit.Abstractions;
using MongoDB.Driver;
using Project.Application.Services;
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
    
        Criar Usuário - Repositório

        Teste será realizado para garantir que o método Criar no repositório insira corretamente o usuário

        **************************************************************************************************/

        [Fact]
        public async Task Criar_UsuarioValido_RetornaUsuario()
        {   
            try
            {
                _output.WriteLine("🔌 Iniciando teste: Criar_UsuarioValido_RetornaUsuario");

                // Criando o mock do IMongoCollection<Usuario>
                var mockCollection = new Mock<IMongoCollection<Usuario>>();

                _output.WriteLine("✅ Mock da coleção IMongoCollection<Usuario> criado.");

                // Criando o mock do MongoClient
                var mockMongoClient = new Mock<IMongoClient>();

                // Criando o mock do MongoDatabase
                var mockMongoDatabase = new Mock<IMongoDatabase>();

                // Configurando o mock para o método GetCollection
                mockMongoDatabase.Setup(db => db.GetCollection<Usuario>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                                .Returns(mockCollection.Object);

                _output.WriteLine("🔧 Configurações de mock para MongoDatabase aplicadas.");

                // Criando o mock de IOptions<ConfigMongoDb> para injetar as configurações
                var mockConfig = new Mock<IOptions<ConfigMongoDb>>();
                mockConfig.Setup(c => c.Value).Returns(new ConfigMongoDb
                {
                    ConnectionString = "teste",
                    DatabaseName = "TestsDb",
                    UsuarioCollectionName = "t_usuario"
                });

                _output.WriteLine("🧪 Mock de configurações do MongoDB inicializado.");

                // Criando a instância do repositório
                //var repository = new UsuarioRepository(mockConfig.Object);
                var repository = new UsuarioRepository(mockCollection.Object);

                // Criando o usuário a ser inserido
                var usuario = new Usuario { Nome = "Claudio", CPF = "40830740000", Telefone = "950556066", Email = "claudio@test.com", Senha = "senha", Perfil = "Comum" };

                _output.WriteLine($"👤 Usuário criado para inserção: {System.Text.Json.JsonSerializer.Serialize(usuario)}");

                // Configurando o mock para simular a inserção
                mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<Usuario>(), null, default))
                            .Returns(Task.CompletedTask)
                            .Callback<Usuario, InsertOneOptions, CancellationToken>((u, _, _) =>
                                {
                                    _output.WriteLine("📥 Inserção simulada do usuário no banco.");
                                });

                // Chamada do método Criar
                var resultado = await repository.Criar(usuario);

                _output.WriteLine("✅ Método Criar chamado com sucesso.");
                _output.WriteLine($"🎯 Resultado retornado: Nome={resultado.Nome}, Email={resultado.Email}");

                // Verificando se o retorno foi o mesmo objeto que passamos
                Assert.Equal(usuario.Id, resultado.Id);
                Assert.Equal(usuario.Nome, resultado.Nome);

                // Verificando se o método InsertOneAsync foi chamado no mock
                mockCollection.Verify(m => m.InsertOneAsync(It.IsAny<Usuario>(), null, default), Times.Once);

                _output.WriteLine("🔚 Teste finalizado com sucesso.");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Erro no teste: {ex.Message}");
                throw;
            }
            
        }
        

    }
}
