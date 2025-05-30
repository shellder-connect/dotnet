using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class AbrigoServiceTests
{
    private readonly ITestOutputHelper _output;

    public AbrigoServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Abrigo

     Teste será realizado apenas para criaçao de um usuário

    **************************************************************************************************/

    [Fact]
    public async Task Criar_AbrigoValido_RetornaAbrigo()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        var service = new AbrigoService(mockRepo.Object);
        var abrigo = new Abrigo { CapacidadeTotal = 10, OcupacaoAtual = 20, Descricao = "Polo" };

        mockRepo.Setup(repo => repo.Criar(abrigo)).ReturnsAsync(abrigo);

        _output.WriteLine("********************( Chamando o serviço Criar para o usuário )********************\n");

        var resultado = await service.Criar(abrigo);

        // saber o que houve no teste
        _output.WriteLine($"Abrigo criado: {resultado.Descricao}\n");

        Assert.Equal(abrigo.Descricao, resultado.Descricao);

        _output.WriteLine("********************( Teste de criação de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Abrigos

     Teste será realizado apenas para consultar a lista completa de usuários com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeAbrigos()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        var abrigosEsperados = new List<Abrigo>
        {
            new Abrigo { Descricao = "Medicamento" },
            new Abrigo { Descricao = "João" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(abrigosEsperados);
        var service = new AbrigoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os usuários )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Abrigos retornados:");
        foreach (var user in resultado)
        {
            _output.WriteLine($"- {user.Descricao}\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.Descricao == "Medicamento");
        Assert.Contains(resultado, u => u.Descricao == "João");

        _output.WriteLine("********************( Teste de consulta de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Abrigos

     Teste será realizado apenas para consultar a lista completa de usuários com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemAbrigos_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Abrigo>());
        var service = new AbrigoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de usuário )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Abrigo por ID

     Teste será realizado apenas para consultar um usuário pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarAbrigoPorId()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        
        // Configura o mock para retornar um usuário específico quando consultado pelo ID
        var abrigoEsperado = new Abrigo { Descricao = "Medicamento", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(abrigoEsperado);
        
        var service = new AbrigoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o usuário pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Abrigo e Id consultado: {resultado?.Descricao}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Abrigo

     Teste será realizado apenas para atualizar um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarAbrigoAtualizado()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        
        // Abrigo original
        var abrigoOriginal = new Abrigo { Descricao = "Medicamento", Id = "123" };
        var abrigoAtualizado = new Abrigo { Descricao = "Medicamento Atualizado", Id = "123" };
        
        // Configura o mock para retornar o usuário atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(abrigoOriginal)).ReturnsAsync(abrigoAtualizado);
        
        var service = new AbrigoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do usuário )********************\n");

        // Chama o serviço para atualizar o usuário
        var resultado = await service.Atualizar(abrigoOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Abrigo atualizado: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Abrigo

     Teste será realizado apenas para atualizar parcialmente um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarAbrigoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        
        // Abrigo original
        var abrigoOriginal = new Abrigo { Descricao = "Medicamento", Id = "123" };
         
        // Descricao e Email serão atualizados
        var abrigoAtualizado = new Abrigo { Descricao = "Medicamento Silva", Id = "123" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Descricao", "Medicamento Silva" },
            { "Email", "claudio.silva@exemplo.com" }
        };
        
        // Configura o mock para retornar o usuário parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(abrigoAtualizado);
        
        var service = new AbrigoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do usuário )********************\n");

        // Chama o serviço para atualizar parcialmente o usuário
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Abrigo parcialmente atualizado: Descricao: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Abrigo

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_AbrigoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IAbrigoRepository>();
        var abrigoId = "123";  // ID do usuário a ser excluído
        var service = new AbrigoService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(abrigoId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(abrigoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do usuário )********************\n");

        _output.WriteLine($"Abrigo com ID {abrigoId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(abrigoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}