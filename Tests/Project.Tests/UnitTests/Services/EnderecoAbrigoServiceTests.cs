using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services
{
    public class EnderecoAbrigoServiceTests
    {
        private readonly ITestOutputHelper _output;

        public EnderecoAbrigoServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************

         Criar EnderecoAbrigo

         Teste será realizado apenas para criação de um enderecoAbrigo

        **************************************************************************************************/

        [Fact]
        public async Task Criar_EnderecoAbrigoValido_RetornaEnderecoAbrigo()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();
            var service = new EnderecoAbrigoService(mockRepo.Object);
            var enderecoAbrigo = new EnderecoAbrigo { CEP = "05656330" };

            mockRepo.Setup(repo => repo.Criar(enderecoAbrigo)).ReturnsAsync(enderecoAbrigo);

            _output.WriteLine("********************( Chamando o serviço Criar para o enderecoAbrigo )********************\n");

            var resultado = await service.Criar(enderecoAbrigo);

            _output.WriteLine($"Endereço criado: {resultado.CEP}\n");

            Assert.Equal(enderecoAbrigo.CEP, resultado.CEP);

            _output.WriteLine("********************( Teste de criação de enderecoAbrigo concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os EnderecoAbrigos

         Teste será realizado para consultar a lista completa de enderecoAbrigos com dados.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_DeveRetornarListaDeEnderecoAbrigos()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();
            var enderecoAbrigosEsperados = new List<EnderecoAbrigo>
            {
                new EnderecoAbrigo { CEP = "0576290" },
                new EnderecoAbrigo { Estado = "Estado" },
                new EnderecoAbrigo { Cidade = "Cidade" },
                new EnderecoAbrigo { Bairro = "Bairro" },
                new EnderecoAbrigo { Rua = "Rua" }
            };
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(enderecoAbrigosEsperados);
            var service = new EnderecoAbrigoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar todos os enderecoAbrigos )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine("Endereços retornados:");
            foreach (var enderecoAbrigo in resultado)
            {
                _output.WriteLine($"- CEP: {enderecoAbrigo.CEP}, Estado: {enderecoAbrigo.Estado}, Cidade: {enderecoAbrigo.Cidade}, Bairro: {enderecoAbrigo.Bairro}, Rua: {enderecoAbrigo.Rua}");
            }

            Assert.Equal(5, resultado.Count);
            Assert.Contains(resultado, e => e.CEP == "0576290");
            Assert.Contains(resultado, e => e.Estado == "Estado");
            Assert.Contains(resultado, e => e.Cidade == "Cidade");

            _output.WriteLine("********************( Teste de consulta de enderecoAbrigo concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os EnderecoAbrigos - Cenário vazio

         Teste para retornar lista vazia caso não haja enderecoAbrigos.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_SemEnderecoAbrigos_DeveRetornarListaVazia()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<EnderecoAbrigo>());
            var service = new EnderecoAbrigoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de enderecoAbrigo )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine($"Total de enderecoAbrigos retornados: {resultado.Count}\n");
            Assert.Empty(resultado);

            _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
        }


        
        /*************************************************************************************************

         Atualizar EnderecoAbrigo

         Teste para atualizar um enderecoAbrigo existente

        **************************************************************************************************/

        [Fact]
        public async Task Atualizar_DeveRetornarEnderecoAbrigoAtualizado()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();

            var enderecoAbrigoOriginal = new EnderecoAbrigo { CEP = "05656330", Id = "123" };
            var enderecoAbrigoAtualizado = new EnderecoAbrigo { CEP = "99999999", Id = "123" };

            mockRepo.Setup(repo => repo.Atualizar(enderecoAbrigoOriginal)).ReturnsAsync(enderecoAbrigoAtualizado);

            var service = new EnderecoAbrigoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do enderecoAbrigo )********************\n");

            var resultado = await service.Atualizar(enderecoAbrigoOriginal);

            _output.WriteLine($"Endereço atualizado: CEP: {resultado?.CEP}\n");
            Assert.NotNull(resultado);
            Assert.Equal("99999999", resultado?.CEP);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização concluído )********************\n");
        }

        /*************************************************************************************************

         Atualizar Parcial EnderecoAbrigo

         Teste para atualizar parcialmente um enderecoAbrigo existente

        **************************************************************************************************/

        [Fact]
        public async Task AtualizarParcial_DeveRetornarEnderecoAbrigoAtualizadoParcialmente()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();

            var enderecoAbrigoOriginal = new EnderecoAbrigo { CEP = "05656330", Id = "123", Cidade = "São Paulo" };

            var enderecoAbrigoAtualizado = new EnderecoAbrigo { CEP = "05656330", Id = "123", Cidade = "Campinas" };

            var camposParaAtualizar = new Dictionary<string, object>
            {
                { "Cidade", "Campinas" }
            };

            mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(enderecoAbrigoAtualizado);

            var service = new EnderecoAbrigoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Atualização parcial do enderecoAbrigo )********************\n");

            var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

            _output.WriteLine($"Endereço parcialmente atualizado: CEP: {resultado?.CEP}, Cidade: {resultado?.Cidade}\n");
            Assert.NotNull(resultado);
            Assert.Equal("Campinas", resultado?.Cidade);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
        }

        /*************************************************************************************************

         Excluir EnderecoAbrigo

         Teste para garantir que o método Excluir seja chamado corretamente

        **************************************************************************************************/

        [Fact]
        public async Task Excluir_EnderecoAbrigoValido_ChamaMetodoExcluirNoRepositorio()
        {
            var mockRepo = new Mock<IEnderecoAbrigoRepository>();
            var enderecoAbrigoId = "123";
            var service = new EnderecoAbrigoService(mockRepo.Object);

            mockRepo.Setup(repo => repo.Excluir(enderecoAbrigoId)).Returns(Task.CompletedTask);

            await service.Excluir(enderecoAbrigoId);

            _output.WriteLine("********************( Chamando o serviço Exclusão do enderecoAbrigo )********************\n");

            _output.WriteLine($"Endereço com ID {enderecoAbrigoId} excluído\n");

            mockRepo.Verify(repo => repo.Excluir(enderecoAbrigoId), Times.Once);

            _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
        }
    }
}
