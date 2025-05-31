using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests.Services
{
    public class EnderecoServiceTests
    {
        private readonly ITestOutputHelper _output;

        public EnderecoServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /*************************************************************************************************

         Criar Endereco

         Teste será realizado apenas para criação de um endereco

        **************************************************************************************************/

        [Fact]
        public async Task Criar_EnderecoValido_RetornaEndereco()
        {
            var mockRepo = new Mock<IEnderecoRepository>();
            var service = new EnderecoService(mockRepo.Object);
            var endereco = new Endereco { CEP = "05656330" };

            mockRepo.Setup(repo => repo.Criar(endereco)).ReturnsAsync(endereco);

            _output.WriteLine("********************( Chamando o serviço Criar para o endereco )********************\n");

            var resultado = await service.Criar(endereco);

            _output.WriteLine($"Endereço criado: {resultado.CEP}\n");

            Assert.Equal(endereco.CEP, resultado.CEP);

            _output.WriteLine("********************( Teste de criação de endereco concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os Enderecos

         Teste será realizado para consultar a lista completa de enderecos com dados.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_DeveRetornarListaDeEnderecos()
        {
            var mockRepo = new Mock<IEnderecoRepository>();
            var enderecosEsperados = new List<Endereco>
            {
                new Endereco { CEP = "0576290" },
                new Endereco { Estado = "Estado" },
                new Endereco { Cidade = "Cidade" },
                new Endereco { Bairro = "Bairro" },
                new Endereco { Rua = "Rua" }
            };
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(enderecosEsperados);
            var service = new EnderecoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar todos os enderecos )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine("Endereços retornados:");
            foreach (var endereco in resultado)
            {
                _output.WriteLine($"- CEP: {endereco.CEP}, Estado: {endereco.Estado}, Cidade: {endereco.Cidade}, Bairro: {endereco.Bairro}, Rua: {endereco.Rua}");
            }

            Assert.Equal(5, resultado.Count);
            Assert.Contains(resultado, e => e.CEP == "0576290");
            Assert.Contains(resultado, e => e.Estado == "Estado");
            Assert.Contains(resultado, e => e.Cidade == "Cidade");

            _output.WriteLine("********************( Teste de consulta de endereco concluído )********************\n");
        }

        /*************************************************************************************************

         Consultar todos os Enderecos - Cenário vazio

         Teste para retornar lista vazia caso não haja enderecos.

        **************************************************************************************************/

        [Fact]
        public async Task ConsultarTodos_SemEnderecos_DeveRetornarListaVazia()
        {
            var mockRepo = new Mock<IEnderecoRepository>();
            mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Endereco>());
            var service = new EnderecoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de endereco )********************\n");

            var resultado = await service.ConsultarTodos();

            _output.WriteLine($"Total de enderecos retornados: {resultado.Count}\n");
            Assert.Empty(resultado);

            _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
        }


        
        /*************************************************************************************************

         Atualizar Endereco

         Teste para atualizar um endereco existente

        **************************************************************************************************/

        [Fact]
        public async Task Atualizar_DeveRetornarEnderecoAtualizado()
        {
            var mockRepo = new Mock<IEnderecoRepository>();

            var enderecoOriginal = new Endereco { CEP = "05656330", Id = "123" };
            var enderecoAtualizado = new Endereco { CEP = "99999999", Id = "123" };

            mockRepo.Setup(repo => repo.Atualizar(enderecoOriginal)).ReturnsAsync(enderecoAtualizado);

            var service = new EnderecoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do endereco )********************\n");

            var resultado = await service.Atualizar(enderecoOriginal);

            _output.WriteLine($"Endereço atualizado: CEP: {resultado?.CEP}\n");
            Assert.NotNull(resultado);
            Assert.Equal("99999999", resultado?.CEP);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização concluído )********************\n");
        }

        /*************************************************************************************************

         Atualizar Parcial Endereco

         Teste para atualizar parcialmente um endereco existente

        **************************************************************************************************/

        [Fact]
        public async Task AtualizarParcial_DeveRetornarEnderecoAtualizadoParcialmente()
        {
            var mockRepo = new Mock<IEnderecoRepository>();

            var enderecoOriginal = new Endereco { CEP = "05656330", Id = "123", Cidade = "São Paulo" };

            var enderecoAtualizado = new Endereco { CEP = "05656330", Id = "123", Cidade = "Campinas" };

            var camposParaAtualizar = new Dictionary<string, object>
            {
                { "Cidade", "Campinas" }
            };

            mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(enderecoAtualizado);

            var service = new EnderecoService(mockRepo.Object);

            _output.WriteLine("********************( Chamando o serviço Atualização parcial do endereco )********************\n");

            var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

            _output.WriteLine($"Endereço parcialmente atualizado: CEP: {resultado?.CEP}, Cidade: {resultado?.Cidade}\n");
            Assert.NotNull(resultado);
            Assert.Equal("Campinas", resultado?.Cidade);
            Assert.Equal("123", resultado?.Id);

            _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
        }

        /*************************************************************************************************

         Excluir Endereco

         Teste para garantir que o método Excluir seja chamado corretamente

        **************************************************************************************************/

        [Fact]
        public async Task Excluir_EnderecoValido_ChamaMetodoExcluirNoRepositorio()
        {
            var mockRepo = new Mock<IEnderecoRepository>();
            var enderecoId = "123";
            var service = new EnderecoService(mockRepo.Object);

            mockRepo.Setup(repo => repo.Excluir(enderecoId)).Returns(Task.CompletedTask);

            await service.Excluir(enderecoId);

            _output.WriteLine("********************( Chamando o serviço Exclusão do endereco )********************\n");

            _output.WriteLine($"Endereço com ID {enderecoId} excluído\n");

            mockRepo.Verify(repo => repo.Excluir(enderecoId), Times.Once);

            _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
        }
    }
}
