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

     Teste será realizado apenas para criaçao de um usuário

    **************************************************************************************************/

    [Fact]
    public async Task Criar_EnderecoValido_RetornaEndereco()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var service = new EnderecoService(mockRepo.Object);
        var endereco = new Endereco { CEP = "05763290", Estado = "São Paulo", Cidade = "Polo", Bairro = "Centro", Rua = "Teste" };

        mockRepo.Setup(repo => repo.Criar(endereco)).ReturnsAsync(endereco);

        _output.WriteLine("********************( Chamando o serviço Criar para o usuário )********************\n");

        var resultado = await service.Criar(endereco);

        // saber o que houve no teste
        _output.WriteLine($"Endereco criado: {resultado.CEP}\n");

        Assert.Equal(endereco.CEP, resultado.CEP);

        _output.WriteLine("********************( Teste de criação de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Enderecos

     Teste será realizado apenas para consultar a lista completa de usuários com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeEnderecos()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var enderecosEsperados = new List<Endereco>
        {
            new Endereco { CEP = "05763290" },
            new Endereco { Estado = "João" },
            new Endereco { Cidade = "São Carlos" },
            new Endereco { Bairro = "Teste" },
            new Endereco { Rua = "João da Cunha" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(enderecosEsperados);
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os usuários )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Enderecos retornados:");
        foreach (var user in resultado)
        {
            _output.WriteLine($"- {user.CEP}\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.CEP == "05763290");
        Assert.Contains(resultado, u => u.Estado == "Estado");
        Assert.Contains(resultado, u => u.Cidade == "Cidade");
        Assert.Contains(resultado, u => u.Bairro == "Bairro");
        Assert.Contains(resultado, u => u.Rua == "Rua");

        _output.WriteLine("********************( Teste de consulta de usuário concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Enderecos

     Teste será realizado apenas para consultar a lista completa de usuários com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemEnderecos_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Endereco>());
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de usuário )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de usuários retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Endereco por ID

     Teste será realizado apenas para consultar um usuário pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarEnderecoPorId()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        // Configura o mock para retornar um usuário específico quando consultado pelo ID
        var enderecoEsperado = new Endereco { CEP = "CEP", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarPorUsuarioId("1")).ReturnsAsync(enderecoEsperado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o usuário pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Endereco e Id consultado: {resultado?.CEP}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.CEP);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Endereco

     Teste será realizado apenas para atualizar um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarEnderecoAtualizado()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        // Endereco original
        var enderecoOriginal = new Endereco { Estado = "Estado", Id = "1" };
        var enderecoAtualizado = new Endereco { Estado = "Estado Atualizado", Id = "1" };
        
        // Configura o mock para retornar o usuário atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(enderecoOriginal)).ReturnsAsync(enderecoAtualizado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do usuário )********************\n");

        // Chama o serviço para atualizar o usuário
        var resultado = await service.Atualizar(enderecoOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Endereco atualizado: {resultado?.CEP}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.CEP);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Endereco

     Teste será realizado apenas para atualizar parcialmente um usuário existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarEnderecoAtualizadoParcialmente()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        
        // Endereco original
        var enderecoOriginal = new Endereco { CEP = "CEP", Id = "1" };
         
        // Descricao e Email serão atualizados
        var enderecoAtualizado = new Endereco { CEP = "05763290", Id = "1" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "CEP", "05763290" },
            { "Estado", "Centro" }
        };
        
        // Configura o mock para retornar o usuário parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(enderecoAtualizado);
        
        var service = new EnderecoService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do usuário )********************\n");

        // Chama o serviço para atualizar parcialmente o usuário
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Endereco parcialmente atualizado: Descricao: {resultado?.CEP}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.CEP);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Endereco

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_EnderecoValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<IEnderecoRepository>();
        var enderecoId = "123";  // ID do usuário a ser excluído
        var service = new EnderecoService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(enderecoId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(enderecoId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do usuário )********************\n");

        _output.WriteLine($"Endereco com ID {enderecoId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(enderecoId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}