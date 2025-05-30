using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class TipoUsuarioServiceTests
{
    private readonly ITestOutputHelper _output;

    public TipoUsuarioServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Tipo de Usuário

     Teste será realizado apenas para criaçao de um usuário

    **************************************************************************************************/

    [Fact]
    public async Task Criar_TipoUsuarioValido_RetornaTipoUsuario()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        var service = new TipoUsuarioService(mockRepo.Object);
        var tipoUsuario = new TipoUsuario { Descricao = "Medicamento" };

        mockRepo.Setup(repo => repo.Criar(tipoUsuario)).ReturnsAsync(tipoUsuario);

        _output.WriteLine("********************( Chamando o serviço Criar para o usuário )********************\n");

        var resultado = await service.Criar(tipoUsuario);

        // saber o que houve no teste
        _output.WriteLine($"Tipo de Usuário criado: {resultado.Descricao}\n");

        Assert.Equal(tipoUsuario.Descricao, resultado.Descricao);

        _output.WriteLine("********************( Teste de criação de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Tipo de Usuários

     Teste será realizado apenas para consultar a lista completa de usuários com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeTipoUsuarios()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        var tipoUsuariosEsperados = new List<TipoUsuario>
        {
            new TipoUsuario { Descricao = "Medicamento" },
            new TipoUsuario { Descricao = "João" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(tipoUsuariosEsperados);
        var service = new TipoUsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os usuários )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Tipo de Usuários retornados:");
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
    
     Consultar todos os Tipo de Usuários

     Teste será realizado apenas para consultar a lista completa de usuários com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemTipoUsuarios_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<TipoUsuario>());
        var service = new TipoUsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de usuário )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Tipo de Usuário por ID

     Teste será realizado apenas para consultar um usuário pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarTipoUsuarioPorId()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        
        // Configura o mock para retornar um usuário específico quando consultado pelo ID
        var tipoUsuarioEsperado = new TipoUsuario { Descricao = "Medicamento", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(tipoUsuarioEsperado);
        
        var service = new TipoUsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o usuário pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Tipo de Usuário e Id consultado: {resultado?.Descricao}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Tipo de Usuário

     Teste será realizado apenas para atualizar um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarTipoUsuarioAtualizado()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        
        // Tipo de Usuário original
        var tipoUsuarioOriginal = new TipoUsuario { Descricao = "Medicamento", Id = "123" };
        var tipoUsuarioAtualizado = new TipoUsuario { Descricao = "Medicamento Atualizado", Id = "123" };
        
        // Configura o mock para retornar o usuário atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(tipoUsuarioOriginal)).ReturnsAsync(tipoUsuarioAtualizado);
        
        var service = new TipoUsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do usuário )********************\n");

        // Chama o serviço para atualizar o usuário
        var resultado = await service.Atualizar(tipoUsuarioOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Tipo de Usuário atualizado: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Tipo de Usuário

     Teste será realizado apenas para atualizar parcialmente um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarTipoUsuarioAtualizadoParcialmente()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        
        // Tipo de Usuário original
        var tipoUsuarioOriginal = new TipoUsuario { Descricao = "Medicamento", Id = "123" };
         
        // Descricao e Email serão atualizados
        var tipoUsuarioAtualizado = new TipoUsuario { Descricao = "Medicamento Silva", Id = "123" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Descricao", "Medicamento Silva" },
            { "Email", "claudio.silva@exemplo.com" }
        };
        
        // Configura o mock para retornar o usuário parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(tipoUsuarioAtualizado);
        
        var service = new TipoUsuarioService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do usuário )********************\n");

        // Chama o serviço para atualizar parcialmente o usuário
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Tipo de Usuário parcialmente atualizado: Descricao: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Tipo de Usuário

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_TipoUsuarioValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<ITipoUsuarioRepository>();
        var tipoUsuarioId = "123";  // ID do usuário a ser excluído
        var service = new TipoUsuarioService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(tipoUsuarioId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(tipoUsuarioId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do usuário )********************\n");

        _output.WriteLine($"Tipo de Usuário com ID {tipoUsuarioId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(tipoUsuarioId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}