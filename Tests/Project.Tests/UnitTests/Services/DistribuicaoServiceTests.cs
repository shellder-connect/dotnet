using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services;

public class DistribuicaoServiceTests
{
    private readonly ITestOutputHelper _output;

    public DistribuicaoServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Distribuicao

    **************************************************************************************************/

    [Fact]
    public async Task Criar_DistribuicaoValido_RetornaDistribuicao()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();
        var service = new DistribuicaoService(mockRepo.Object);
        var distribuicao = new Distribuicao 
        { 
            IdDoacao = "507f191e810c19729de860ea", 
            QuantidadeDestinada = 10, 
            DataDestinada = DateTime.Now,
            IdPessoaAtendida = "507f1f77bcf86cd799439011"
        };

        mockRepo.Setup(repo => repo.Criar(distribuicao)).ReturnsAsync(distribuicao);

        _output.WriteLine("********************( Chamando o serviço Criar para a distribuição )********************\n");

        var resultado = await service.Criar(distribuicao);

        _output.WriteLine($"Distribuição criada: QuantidadeDestinada = {resultado.QuantidadeDestinada}\n");

        Assert.Equal(distribuicao.QuantidadeDestinada, resultado.QuantidadeDestinada);
        Assert.Equal(distribuicao.IdDoacao, resultado.IdDoacao);
        Assert.Equal(distribuicao.IdPessoaAtendida, resultado.IdPessoaAtendida);

        _output.WriteLine("********************( Teste de criação de distribuição concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos as Distribuicoes

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeDistribuicoes()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();
        var distribsEsperadas = new List<Distribuicao>
        {
            new Distribuicao 
            { 
                Id = "507f1f77bcf86cd799439011",
                IdDoacao = "507f191e810c19729de860ea",
                QuantidadeDestinada = 5,
                DataDestinada = DateTime.Now.AddDays(-1),
                IdPessoaAtendida = "507f1f77bcf86cd799439012"
            },
            new Distribuicao 
            { 
                Id = "507f191e810c19729de860eb",
                IdDoacao = "507f191e810c19729de860eb",
                QuantidadeDestinada = 15,
                DataDestinada = DateTime.Now,
                IdPessoaAtendida = "507f1f77bcf86cd799439013"
            }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(distribsEsperadas);
        var service = new DistribuicaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos as distribuições )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Distribuições retornadas:");
        foreach (var d in resultado)
        {
            _output.WriteLine($"- Id: {d.Id}, QuantidadeDestinada: {d.QuantidadeDestinada}, DataDestinada: {d.DataDestinada}, IdPessoaAtendida: {d.IdPessoaAtendida}\n");
        }

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, d => d.Id == "507f1f77bcf86cd799439011");
        Assert.Contains(resultado, d => d.Id == "507f191e810c19729de860eb");

        _output.WriteLine("********************( Teste de consulta de distribuições concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos as Distribuicoes - Cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemDistribuicoes_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Distribuicao>());
        var service = new DistribuicaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de distribuições )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de distribuições retornadas: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Distribuicao por ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarDistribuicaoPorId()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();

        var distribuicaoEsperada = new Distribuicao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdDoacao = "507f191e810c19729de860ea",
            QuantidadeDestinada = 10,
            DataDestinada = DateTime.Now,
            IdPessoaAtendida = "507f1f77bcf86cd799439012"
        };

        mockRepo.Setup(repo => repo.ConsultarId("507f1f77bcf86cd799439011")).ReturnsAsync(distribuicaoEsperada);

        var service = new DistribuicaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar a distribuição pelo Id )********************\n");

        var resultado = await service.ConsultarId("507f1f77bcf86cd799439011");

        _output.WriteLine($"Distribuição consultada: IdDoacao: {resultado?.IdDoacao}, QuantidadeDestinada: {resultado?.QuantidadeDestinada}\n");

        Assert.NotNull(resultado);
        Assert.Equal("507f191e810c19729de860ea", resultado?.IdDoacao);
        Assert.Equal(10, resultado?.QuantidadeDestinada);
        Assert.Equal("507f1f77bcf86cd799439011", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Distribuicao

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarDistribuicaoAtualizado()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();

        var distribuicaoOriginal = new Distribuicao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdDoacao = "507f191e810c19729de860ea",
            QuantidadeDestinada = 10,
            DataDestinada = DateTime.Now.AddDays(-1),
            IdPessoaAtendida = "507f1f77bcf86cd799439012"
        };

        var distribuicaoAtualizado = new Distribuicao 
        { 
            Id = "507f1f77bcf86cd799439011", 
            IdDoacao = "507f191e810c19729de860ea",
            QuantidadeDestinada = 20,
            DataDestinada = DateTime.Now,
            IdPessoaAtendida = "507f1f77bcf86cd799439012"
        };

        mockRepo.Setup(repo => repo.Atualizar(distribuicaoOriginal)).ReturnsAsync(distribuicaoAtualizado);

        var service = new DistribuicaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados da distribuição )********************\n");

        var resultado = await service.Atualizar(distribuicaoOriginal);

        _output.WriteLine($"Distribuição atualizada: QuantidadeDestinada: {resultado?.QuantidadeDestinada}, DataDestinada: {resultado?.DataDestinada}\n");

        Assert.NotNull(resultado);
        Assert.Equal(20, resultado?.QuantidadeDestinada);
        Assert.Equal(distribuicaoAtualizado.DataDestinada, resultado?.DataDestinada);
        Assert.Equal("507f1f77bcf86cd799439011", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Distribuicao

    **************************************************************************************************/

    /*
    [Fact]
    public async Task AtualizarParcial_DeveRetornarDistribuicaoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();

        var distribuicaoOriginal = new Distribuicao 
        { 
            Id = "683a34fe6333f0107e656ff2", 
            QuantidadeDestinada = 10,
            DataDestinada = DateTime.Now.AddDays(-1),
            IdPessoaAtendida = "665af9a03fae4c001fcf6d15"
        };

        var distribuicaoAtualizado = new Distribuicao 
        { 
            Id = "683a34fe6333f0107e656ff2", 
            QuantidadeDestinada = 15,
            DataDestinada = DateTime.Now,
            IdPessoaAtendida = "683a34fe6333f0107e656ff2"
        };

        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "QuantidadeDestinada", 15 }
        };

        mockRepo.Setup(repo => repo.AtualizarParcial("683a34fe6333f0107e656ff2", camposParaAtualizar)).ReturnsAsync(distribuicaoAtualizado);

        var service = new DistribuicaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial da distribuição )********************\n");

        var resultado = await service.AtualizarParcial("683a34fe6333f0107e656ff2", camposParaAtualizar);

        _output.WriteLine($"Distribuição parcialmente atualizada: QuantidadeDestinada: {resultado?.QuantidadeDestinada}\n");

        Assert.NotNull(resultado);
        Assert.Equal(15, resultado?.QuantidadeDestinada);
        Assert.Equal("683a34fe6333f0107e656ff2", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }
    */
    
    /*************************************************************************************************
    
     Excluir Distribuicao

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_DistribuicaoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IDistribuicaoRepository>();
        var distribuicaoId = "507f1f77bcf86cd799439011"; 
        var service = new DistribuicaoService(mockRepo.Object);

        mockRepo.Setup(repo => repo.Excluir(distribuicaoId)).Returns(Task.CompletedTask);

        await service.Excluir(distribuicaoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão da distribuição )********************\n");

        _output.WriteLine($"Distribuição com ID {distribuicaoId} excluída\n");

        mockRepo.Verify(repo => repo.Excluir(distribuicaoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }
}
