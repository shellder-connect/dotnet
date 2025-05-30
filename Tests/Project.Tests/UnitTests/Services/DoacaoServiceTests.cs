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

     Teste será realizado apenas para criaçao de um usuário

    **************************************************************************************************/

    [Fact]
    public async Task Criar_DoacaoValido_RetornaDoacao()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var service = new DoacaoService(mockRepo.Object);
        var doacao = new Doacao { IdAbrigo = "1", IdCategoria = "2", Descricao = "Polo", Quantidade = 10 };

        mockRepo.Setup(repo => repo.Criar(doacao)).ReturnsAsync(doacao);

        _output.WriteLine("********************( Chamando o serviço Criar para o usuário )********************\n");

        var resultado = await service.Criar(doacao);

        // saber o que houve no teste
        _output.WriteLine($"Doação criado: {resultado.Descricao}\n");

        Assert.Equal(doacao.Descricao, resultado.Descricao);

        _output.WriteLine("********************( Teste de criação de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Doação

     Teste será realizado apenas para consultar a lista completa de usuários com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeDoacaos()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var doacaosEsperados = new List<Doacao>
        {
            new Doacao { Descricao = "Medicamento" },
            new Doacao { Descricao = "João" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(doacaosEsperados);
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os usuários )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Doação retornados:");
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
    
     Consultar todos os Doação

     Teste será realizado apenas para consultar a lista completa de usuários com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemDoacaos_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Doacao>());
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de usuário )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Doação por ID

     Teste será realizado apenas para consultar um usuário pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarDoacaoPorId()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        
        // Configura o mock para retornar um usuário específico quando consultado pelo ID
        var doacaoEsperado = new Doacao { Descricao = "Medicamento", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(doacaoEsperado);
        
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o usuário pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Doação e Id consultado: {resultado?.Descricao}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Doação

     Teste será realizado apenas para atualizar um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarDoacaoAtualizado()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        
        // Doação original
        var doacaoOriginal = new Doacao { Descricao = "Medicamento", Id = "123" };
        var doacaoAtualizado = new Doacao { Descricao = "Medicamento Atualizado", Id = "123" };
        
        // Configura o mock para retornar o usuário atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(doacaoOriginal)).ReturnsAsync(doacaoAtualizado);
        
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do usuário )********************\n");

        // Chama o serviço para atualizar o usuário
        var resultado = await service.Atualizar(doacaoOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Doação atualizado: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Doação

     Teste será realizado apenas para atualizar parcialmente um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarDoacaoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        
        // Doação original
        var doacaoOriginal = new Doacao { Descricao = "Medicamento", Id = "123" };
         
        // Descricao e Email serão atualizados
        var doacaoAtualizado = new Doacao { Descricao = "Medicamento Silva", Id = "123" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Descricao", "Medicamento Silva" },
            { "Email", "claudio.silva@exemplo.com" }
        };
        
        // Configura o mock para retornar o usuário parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(doacaoAtualizado);
        
        var service = new DoacaoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do usuário )********************\n");

        // Chama o serviço para atualizar parcialmente o usuário
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Doação parcialmente atualizado: Descricao: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Doação

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_DoacaoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IDoacaoRepository>();
        var doacaoId = "123";  // ID do usuário a ser excluído
        var service = new DoacaoService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(doacaoId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(doacaoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do usuário )********************\n");

        _output.WriteLine($"Doação com ID {doacaoId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(doacaoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}