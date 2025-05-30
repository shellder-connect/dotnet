using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services;

public class DoacaoServiceTests
{
    private readonly ITestOutputHelper _output;

    public DoacaoServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Doação

    **************************************************************************************************/

    [Fact]
    public async Task Criar_DoacaoValido_RetornaDoacao()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var service = new DoacaoService(mockRepo.Object);
        var doacao = new Doacao 
        { 
            IdAbrigo = "507f1f77bcf86cd799439011", 
            IdCategoria = "507f191e810c19729de860ea", 
            Descricao = "Polo", 
            Quantidade = 10 
        };

        mockRepo.Setup(repo => repo.Criar(doacao)).ReturnsAsync(doacao);

        _output.WriteLine("********************( Chamando o serviço Criar para a doação )********************\n");

        var resultado = await service.Criar(doacao);

        _output.WriteLine($"Doação criada: {resultado.Descricao}\n");

        Assert.Equal(doacao.Descricao, resultado.Descricao);

        _output.WriteLine("********************( Teste de criação de doação concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos as Doações

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeDoacoes()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var doacoesEsperadas = new List<Doacao>
        {
            new Doacao 
            { 
                Id = "507f1f77bcf86cd799439011",
                IdAbrigo = "507f1f77bcf86cd799439011",
                IdCategoria = "507f191e810c19729de860ea",
                Descricao = "Medicamento", 
                Quantidade = 5
            },
            new Doacao 
            { 
                Id = "507f191e810c19729de860eb",
                IdAbrigo = "507f1f77bcf86cd799439012",
                IdCategoria = "507f191e810c19729de860eb",
                Descricao = "João", 
                Quantidade = 15
            }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(doacoesEsperadas);
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos as doações )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Doações retornadas:");
        foreach (var doacao in resultado)
        {
            _output.WriteLine($"- {doacao.Descricao} (Qtd: {doacao.Quantidade})\n");
        }

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, d => d.Descricao == "Medicamento");
        Assert.Contains(resultado, d => d.Descricao == "João");

        _output.WriteLine("********************( Teste de consulta de doação concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos as Doações - Cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemDoacoes_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Doacao>());
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de doações )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de doações retornadas: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Doação por ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarDoacaoPorId()
    {
        var mockRepo = new Mock<IDoacaoRepository>();

        var doacaoEsperada = new Doacao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdAbrigo = "507f1f77bcf86cd799439011",
            IdCategoria = "507f191e810c19729de860ea",
            Descricao = "Medicamento", 
            Quantidade = 10 
        };

        mockRepo.Setup(repo => repo.ConsultarId("507f1f77bcf86cd799439011")).ReturnsAsync(doacaoEsperada);

        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar a doação pelo Id )********************\n");

        var resultado = await service.ConsultarId("507f1f77bcf86cd799439011");

        _output.WriteLine($"Doação e Id consultado: {resultado?.Descricao}, {resultado?.Id}\n");

        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.Descricao);
        Assert.Equal("507f1f77bcf86cd799439011", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Doação

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarDoacaoAtualizado()
    {
        var mockRepo = new Mock<IDoacaoRepository>();

        var doacaoOriginal = new Doacao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdAbrigo = "507f1f77bcf86cd799439011",
            IdCategoria = "507f191e810c19729de860ea",
            Descricao = "Medicamento", 
            Quantidade = 10 
        };

        var doacaoAtualizado = new Doacao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdAbrigo = "507f1f77bcf86cd799439011",
            IdCategoria = "507f191e810c19729de860ea",
            Descricao = "Medicamento Atualizado", 
            Quantidade = 20 
        };

        mockRepo.Setup(repo => repo.Atualizar(doacaoOriginal)).ReturnsAsync(doacaoAtualizado);

        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados da doação )********************\n");

        var resultado = await service.Atualizar(doacaoOriginal);

        _output.WriteLine($"Doação atualizada: {resultado?.Descricao}, Quantidade: {resultado?.Quantidade}\n");

        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.Descricao);
        Assert.Equal(20, resultado?.Quantidade);
        Assert.Equal("507f1f77bcf86cd799439011", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Doação

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarDoacaoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IDoacaoRepository>();

        var doacaoOriginal = new Doacao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            Descricao = "Medicamento", 
            Quantidade = 10 
        };

        var doacaoAtualizado = new Doacao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            Descricao = "Medicamento Silva", 
            Quantidade = 10 
        };

        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Descricao", "Medicamento Silva" }
        };

        mockRepo.Setup(repo => repo.AtualizarParcial("507f1f77bcf86cd799439011", camposParaAtualizar)).ReturnsAsync(doacaoAtualizado);

        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial da doação )********************\n");

        var resultado = await service.AtualizarParcial("507f1f77bcf86cd799439011", camposParaAtualizar);

        _output.WriteLine($"Doação parcialmente atualizada: Descricao: {resultado?.Descricao}\n");

        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.Descricao);
        Assert.Equal("507f1f77bcf86cd799439011", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************
    
     Excluir Doação

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_DoacaoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var doacaoId = "507f1f77bcf86cd799439011"; 
        var service = new DoacaoService(mockRepo.Object);

        mockRepo.Setup(repo => repo.Excluir(doacaoId)).Returns(Task.CompletedTask);

        await service.Excluir(doacaoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão da doação )********************\n");

        _output.WriteLine($"Doação com ID {doacaoId} excluída\n");

        mockRepo.Verify(repo => repo.Excluir(doacaoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }
}
