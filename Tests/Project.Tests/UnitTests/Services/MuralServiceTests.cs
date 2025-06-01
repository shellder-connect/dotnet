

using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class MuralServiceTests
{
    private readonly ITestOutputHelper _output;

    public MuralServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Mural

     Teste será realizado apenas para criação de um post no mural

    **************************************************************************************************/

    [Fact]
    public async Task Criar_MuralValido_RetornaMural()
    {
        var mockRepo = new Mock<IMuralRepository>();
        var service = new MuralService(mockRepo.Object);
        var mural = new Mural 
        { 
            IdUsuario = "60a1b2c3d4e5f6789012345a",
            Titulo = "Evento Beneficente", 
            Comentario = "Evento para arrecadação de fundos",
            TipoPost = "Evento",
            Data = DateTime.Now
        };

        mockRepo.Setup(repo => repo.Criar(mural)).ReturnsAsync(mural);

        _output.WriteLine("********************( Chamando o serviço Criar para o post )********************\n");

        var resultado = await service.Criar(mural);

        // saber o que houve no teste
        _output.WriteLine($"Post criado: {resultado.Titulo}\n");

        Assert.Equal(mural.Titulo, resultado.Titulo);
        Assert.Equal(mural.IdUsuario, resultado.IdUsuario);
        Assert.Equal(mural.TipoPost, resultado.TipoPost);

        _output.WriteLine("********************( Teste de criação de post concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Posts do Mural

     Teste será realizado apenas para consultar a lista completa de posts com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeMurais()
    {
        var mockRepo = new Mock<IMuralRepository>();
        var muraisEsperados = new List<Mural>
        {
            new Mural { Titulo = "Doação de Alimentos", TipoPost = "Noticia" },
            new Mural { Titulo = "Evento Solidário", TipoPost = "Evento" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(muraisEsperados);
        var service = new MuralService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os posts )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Posts retornados:");
        foreach (var post in resultado)
        {
            _output.WriteLine($"- {post.Titulo} ({post.TipoPost})\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.Titulo == "Doação de Alimentos");
        Assert.Contains(resultado, u => u.Titulo == "Evento Solidário");

        _output.WriteLine("********************( Teste de consulta de posts concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Posts do Mural

     Teste será realizado apenas para consultar a lista completa de posts com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemMurais_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IMuralRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Mural>());
        var service = new MuralService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de posts )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de posts retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Post do Mural por ID

     Teste será realizado apenas para consultar um post pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarMuralPorId()
    {
        var mockRepo = new Mock<IMuralRepository>();
        
        // Configura o mock para retornar um post específico quando consultado pelo ID
        var muralEsperado = new Mural 
        { 
            Titulo = "Campanha de Inverno", 
            Id = "123",
            TipoPost = "Anuncio",
            IdUsuario = "60a1b2c3d4e5f6789012345a"
        };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(muralEsperado);
        
        var service = new MuralService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o post pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Post e Id consultado: {resultado?.Titulo}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Campanha de Inverno", resultado?.Titulo);
        Assert.Equal("123", resultado?.Id);
        Assert.Equal("Anuncio", resultado?.TipoPost);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Post do Mural

     Teste será realizado apenas para atualizar um post existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarMuralAtualizado()
    {
        var mockRepo = new Mock<IMuralRepository>();
        
        // Post original
        var muralOriginal = new Mural 
        { 
            Titulo = "Campanha de Inverno", 
            Id = "123",
            TipoPost = "Anuncio",
            Comentario = "Campanha para arrecadação de agasalhos"
        };
        var muralAtualizado = new Mural 
        { 
            Titulo = "Campanha de Inverno Atualizada", 
            Id = "123",
            TipoPost = "Evento",
            Comentario = "Campanha atualizada para arrecadação de agasalhos e cobertores"
        };
        
        // Configura o mock para retornar o post atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(muralOriginal)).ReturnsAsync(muralAtualizado);
        
        var service = new MuralService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do post )********************\n");

        // Chama o serviço para atualizar o post
        var resultado = await service.Atualizar(muralOriginal);

        // Verifica se o título foi atualizado
        _output.WriteLine($"Post atualizado: {resultado?.Titulo}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Campanha de Inverno Atualizada", resultado?.Titulo);
        Assert.Equal("123", resultado?.Id);
        Assert.Equal("Evento", resultado?.TipoPost);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Post do Mural

     Teste será realizado apenas para atualizar parcialmente um post existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarMuralAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IMuralRepository>();
        
        // Post original
        var muralOriginal = new Mural 
        { 
            Titulo = "Campanha de Inverno", 
            Id = "123",
            TipoPost = "Anuncio"
        };
         
        var muralAtualizado = new Mural 
        { 
            Titulo = "Campanha de Inverno e Verão", 
            Id = "123",
            TipoPost = "Evento"
        }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Titulo", "Campanha de Inverno e Verão" },
            { "TipoPost", "Evento" }
        };
        
        // Configura o mock para retornar o post parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(muralAtualizado);
        
        var service = new MuralService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do post )********************\n");

        // Chama o serviço para atualizar parcialmente o post
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Post parcialmente atualizado: Titulo: {resultado?.Titulo}, TipoPost: {resultado?.TipoPost}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Campanha de Inverno e Verão", resultado?.Titulo);
        Assert.Equal("Evento", resultado?.TipoPost);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Post do Mural

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_MuralValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IMuralRepository>();
        var muralId = "123";  // ID do post a ser excluído
        var service = new MuralService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(muralId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(muralId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do post )********************\n");

        _output.WriteLine($"Post com ID {muralId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(muralId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }
}