using Xunit;
using Moq;
using MongoDB.Driver;
using Project.Models;
using Project.Repositories;
using Project.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Tests
{
    public class EnderecoRepositoryTests
    {
        private readonly Mock<IMongoCollection<Endereco>> _mockCollection;
        private readonly Mock<IOptions<ConfigMongoDb>> _mockOptions;
        private readonly EnderecoRepository _repository;

        public EnderecoRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Endereco>>();
            _mockCollection.Setup(c => c.DocumentSerializer).Returns(new MongoDB.Bson.Serialization.BsonClassMapSerializer<Endereco>(MongoDB.Bson.Serialization.BsonClassMap.LookupClassMap(typeof(Endereco))));

            _mockOptions = new Mock<IOptions<ConfigMongoDb>>();
            _mockOptions.Setup(o => o.Value).Returns(new ConfigMongoDb
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDb",
                EnderecoCollectionName = "t_endereco"
            });

            var client = new MongoClient(_mockOptions.Object.Value.ConnectionString);
            var database = client.GetDatabase(_mockOptions.Object.Value.DatabaseName);

            // Usar um database real com uma collection mockada
            _repository = new EnderecoRepository(_mockOptions.Object);
        }

        [Fact]
        public async Task Criar_DeveInserirEndereco()
        {
            // Arrange
            var endereco = new Endereco { Id = "683a34fe6333f0107e656ff5", CEP = "12345678" };

            _mockCollection.Setup(c => c.InsertOneAsync(
                endereco, null, default))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _repository.Criar(endereco);

            // Assert
            Assert.Equal(endereco, resultado);
        }

        /*
        [Fact]
        public async Task ConsultarId_DeveRetornarEndereco()
        {
            // Arrange
            var endereco = new Endereco { Id = "683a34fe6333f0107e656ff5", CEP = "12345678" };

            var mockCursor = new Mock<IAsyncCursor<Endereco>>();
            mockCursor.Setup(_ => _.Current).Returns(new List<Endereco> { endereco });
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Endereco>>(),
                It.IsAny<FindOptions<Endereco, Endereco>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(mockCursor.Object);

            // Act
            var resultado = await _repository.ConsultarId("683a34fe6333f0107e656ff5");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("683a34fe6333f0107e656ff5", resultado.Id);
        }
        */

        /*
        [Fact]
        public async Task Atualizar_DeveRetornarEnderecoAtualizado()
        {
            // Arrange
            var endereco = new Endereco
            {
                Id = "683a34fe6333f0107e656ff5",
                CEP = "12345678",
                Estado = "SP",
                Cidade = "São Paulo",
                Bairro = "Centro",
                Rua = "Rua A"
            };

            var updateResult = Mock.Of<UpdateResult>(u => u.ModifiedCount == 1);

            _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Endereco>>(),
                It.IsAny<UpdateDefinition<Endereco>>(),
                null,
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(updateResult);

            // Act
            var resultado = await _repository.Atualizar(endereco);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("12345678", resultado.CEP);
        }
        */

        [Fact]
        public async Task Excluir_DeveExecutarSemErros()
        {
            // Arrange
            var deleteResult = Mock.Of<DeleteResult>(d => d.DeletedCount == 1);

            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Endereco>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(deleteResult);

            // Act
            var exception = await Record.ExceptionAsync(() => _repository.Excluir("683a34fe6333f0107e656ff5"));

            // Assert
            Assert.Null(exception);
        }
    }
}
