using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class UsuarioServiceTests
{
    private readonly ITestOutputHelper _output;

    public UsuarioServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Usuário

     Teste será realizado apenas para criaçao de um usuário

    **************************************************************************************************/

    [Fact]
    public async Task Criar_UsuarioValido_RetornaUsuario()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        var service = new UsuarioService(mockRepo.Object);
        var usuario = new Usuario { Nome = "Claudio" };

        mockRepo.Setup(repo => repo.Criar(usuario)).ReturnsAsync(usuario);

        _output.WriteLine("********************( Chamando o serviço Criar para o usuário )********************\n");

        var resultado = await service.Criar(usuario);

        // saber o que houve no teste
        _output.WriteLine($"Usuário criado: {resultado.Nome}\n");

        Assert.Equal(usuario.Nome, resultado.Nome);

        _output.WriteLine("********************( Teste de criação de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Usuários

     Teste será realizado apenas para consultar a lista completa de usuários com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeUsuarios()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        var usuariosEsperados = new List<Usuario>
        {
            new Usuario { Nome = "Claudio" },
            new Usuario { Nome = "João" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(usuariosEsperados);
        var service = new UsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os usuários )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Usuários retornados:");
        foreach (var user in resultado)
        {
            _output.WriteLine($"- {user.Nome}\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.Nome == "Claudio");
        Assert.Contains(resultado, u => u.Nome == "João");

        _output.WriteLine("********************( Teste de consulta de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Usuários

     Teste será realizado apenas para consultar a lista completa de usuários com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemUsuarios_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Usuario>());
        var service = new UsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de usuário )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Usuário por ID

     Teste será realizado apenas para consultar um usuário pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarUsuarioPorId()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Configura o mock para retornar um usuário específico quando consultado pelo ID
        var usuarioEsperado = new Usuario { Nome = "Claudio", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(usuarioEsperado);
        
        var service = new UsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o usuário pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Usuário e Id consultado: {resultado?.Nome}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Claudio", resultado?.Nome);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Usuário

     Teste será realizado apenas para atualizar um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarUsuarioAtualizado()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Usuário original
        var usuarioOriginal = new Usuario { Nome = "Claudio", Id = "123" };
        var usuarioAtualizado = new Usuario { Nome = "Claudio Atualizado", Id = "123" };
        
        // Configura o mock para retornar o usuário atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(usuarioOriginal)).ReturnsAsync(usuarioAtualizado);
        
        var service = new UsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do usuário )********************\n");

        // Chama o serviço para atualizar o usuário
        var resultado = await service.Atualizar(usuarioOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Usuário atualizado: {resultado?.Nome}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Claudio Atualizado", resultado?.Nome);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Usuário

     Teste será realizado apenas para atualizar parcialmente um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarUsuarioAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Usuário original
        var usuarioOriginal = new Usuario { Nome = "Claudio", Id = "123", Email = "claudio@exemplo.com" };
         
        // Nome e Email serão atualizados
        var usuarioAtualizado = new Usuario { Nome = "Claudio Silva", Id = "123", Email = "claudio.silva@exemplo.com" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Nome", "Claudio Silva" },
            { "Email", "claudio.silva@exemplo.com" }
        };
        
        // Configura o mock para retornar o usuário parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(usuarioAtualizado);
        
        var service = new UsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do usuário )********************\n");

        // Chama o serviço para atualizar parcialmente o usuário
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Usuário parcialmente atualizado: Nome: {resultado?.Nome}, Email: {resultado?.Email}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Claudio Silva", resultado?.Nome);
        Assert.Equal("claudio.silva@exemplo.com", resultado?.Email);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Usuário

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_UsuarioValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IUsuarioRepository>();
        var usuarioId = "123";  // ID do usuário a ser excluído
        var service = new UsuarioService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(usuarioId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(usuarioId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do usuário )********************\n");

        _output.WriteLine($"Usuário com ID {usuarioId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(usuarioId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}