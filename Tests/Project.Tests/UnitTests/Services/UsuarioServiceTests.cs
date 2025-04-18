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
}