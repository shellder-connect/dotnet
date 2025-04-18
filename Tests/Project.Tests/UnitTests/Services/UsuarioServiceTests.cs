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

        var resultado = await service.Criar(usuario);

        // saber o que houve no teste
        _output.WriteLine($"Usuário criado: {resultado.Nome}");

        Assert.Equal(usuario.Nome, resultado.Nome);
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

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Usuários retornados:");
        foreach (var user in resultado)
        {
            _output.WriteLine($"- {user.Nome}");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.Nome == "Claudio");
        Assert.Contains(resultado, u => u.Nome == "João");
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

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}");
        Assert.Empty(resultado);
    }

}