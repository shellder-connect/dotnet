using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class EnderecoServiceTests
{
    private readonly ITestOutputHelper _output;

    public EnderecoServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Endereco

     Teste será realizado apenas para criação de um endereço

    **************************************************************************************************/

    [Fact]
    public async Task Criar_EnderecoValido_RetornaEndereco()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var service = new EnderecoService(mockRepo.Object);
        var endereco = new Endereco 
        { 
            CEP = "05763290", 
            Estado = "São Paulo", 
            Cidade = "Polo", 
            Bairro = "Centro", 
            Rua = "Teste" 
        };

        mockRepo.Setup(repo => repo.Criar(endereco)).ReturnsAsync(endereco);

        _output.WriteLine("********************( Chamando o serviço Criar para o endereço )********************\n");

        var resultado = await service.Criar(endereco);

        _output.WriteLine($"Endereco criado: {resultado.CEP}\n");

        Assert.Equal(endereco.CEP, resultado.CEP);

        _output.WriteLine("********************( Teste de criação de endereço concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Enderecos

     Teste será realizado para consultar a lista completa de endereços com dados

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeEnderecos()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var enderecosEsperados = new List<Endereco>
        {
            new Endereco { CEP = "05763290", Estado = "SP", Cidade = "Polo", Bairro = "Centro", Rua = "Teste" },
            new Endereco { CEP = "12345678", Estado = "RJ", Cidade = "Rio", Bairro = "Copacabana", Rua = "Av. Atlântica" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(enderecosEsperados);
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os endereços )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Endereços retornados:");
        foreach (var endereco in resultado)
        {
            _output.WriteLine($"- CEP: {endereco.CEP}, Estado: {endereco.Estado}, Cidade: {endereco.Cidade}, Bairro: {endereco.Bairro}, Rua: {endereco.Rua}\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, e => e.CEP == "05763290" && e.Estado == "SP" && e.Cidade == "Polo");
        Assert.Contains(resultado, e => e.CEP == "12345678" && e.Estado == "RJ" && e.Cidade == "Rio");

        _output.WriteLine("********************( Teste de consulta de endereços concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Enderecos - lista vazia

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemEnderecos_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Endereco>());
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de endereços )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de endereços retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Endereco por ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarEnderecoPorId()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        // Configura o mock para retornar um endereço específico quando consultado pelo ID
        var enderecoEsperado = new Endereco 
        { 
            CEP = "05763290", 
            Estado = "SP", 
            Cidade = "Polo", 
            Bairro = "Centro", 
            Rua = "Teste",
            Id = "123" 
        };
        mockRepo.Setup(repo => repo.ConsultarPorUsuarioId("123")).ReturnsAsync(enderecoEsperado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o endereço pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Endereço consultado: CEP={resultado?.CEP}, Estado={resultado?.Estado}, Id={resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("05763290", resultado?.CEP);
        Assert.Equal("SP", resultado?.Estado);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Endereco

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarEnderecoAtualizado()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        var enderecoOriginal = new Endereco 
        { 
            CEP = "05763290", 
            Estado = "SP", 
            Cidade = "Polo", 
            Bairro = "Centro", 
            Rua = "Teste",
            Id = "123" 
        };
        var enderecoAtualizado = new Endereco 
        { 
            CEP = "99999999", 
            Estado = "RJ", 
            Cidade = "Rio", 
            Bairro = "Copacabana", 
            Rua = "Av. Atlântica",
            Id = "123" 
        };
        
        mockRepo.Setup(repo => repo.Atualizar(enderecoOriginal)).ReturnsAsync(enderecoAtualizado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar endereço )********************\n");

        var resultado = await service.Atualizar(enderecoOriginal);

        _output.WriteLine($"Endereço atualizado: CEP={resultado?.CEP}, Estado={resultado?.Estado}\n");
        Assert.NotNull(resultado);
        Assert.Equal("99999999", resultado?.CEP);
        Assert.Equal("RJ", resultado?.Estado);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Endereco

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarEnderecoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        var enderecoAtualizado = new Endereco 
        { 
            CEP = "05763290", 
            Estado = "SP", 
            Id = "123" 
        };
        
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "CEP", "05763290" },
            { "Estado", "SP" }
        };
        
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(enderecoAtualizado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do endereço )********************\n");

        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        _output.WriteLine($"Endereço parcialmente atualizado: CEP={resultado?.CEP}, Estado={resultado?.Estado}\n");
        Assert.NotNull(resultado);
        Assert.Equal("05763290", resultado?.CEP);
        Assert.Equal("SP", resultado?.Estado);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Endereco

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_EnderecoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var enderecoId = "123";
        var service = new EnderecoService(mockRepo.Object);

        mockRepo.Setup(repo => repo.Excluir(enderecoId)).Returns(Task.CompletedTask);

        await service.Excluir(enderecoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do endereço )********************\n");

        _output.WriteLine($"Endereço com ID {enderecoId} excluído\n");

        mockRepo.Verify(repo => repo.Excluir(enderecoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }
}
