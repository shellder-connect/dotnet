using Moq;
using Project.Application.Services;
using Project.Infrastructure.Interfaces;
using Project.Models;
using Xunit;
using Xunit.Abstractions;

namespace Project.Tests.UnitTests.Services; 

public class CategoriaServiceTests
{
    private readonly ITestOutputHelper _output;

    public CategoriaServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /*************************************************************************************************
    
     Criar Categoria

     Teste será realizado apenas para criaçao de um categoria

    **************************************************************************************************/

    [Fact]
    public async Task Criar_CategoriaValido_RetornaCategoria()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        var service = new CategoriaService(mockRepo.Object);
        var categoria = new Categoria { Descricao = "Medicamento" };

        mockRepo.Setup(repo => repo.Criar(categoria)).ReturnsAsync(categoria);

        _output.WriteLine("********************( Chamando o serviço Criar para o categoria )********************\n");

        var resultado = await service.Criar(categoria);

        // saber o que houve no teste
        _output.WriteLine($"Categoria criado: {resultado.Descricao}\n");

        Assert.Equal(categoria.Descricao, resultado.Descricao);

        _output.WriteLine("********************( Teste de criação de categoria concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Categorias

     Teste será realizado apenas para consultar a lista completa de categorias com dados, não avistando o cenário vazio.

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_DeveRetornarListaDeCategorias()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        var categoriasEsperados = new List<Categoria>
        {
            new Categoria { Descricao = "Medicamento" },
            new Categoria { Descricao = "João" }
        };
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(categoriasEsperados);
        var service = new CategoriaService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar todos os categorias )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine("Categorias retornados:");
        foreach (var user in resultado)
        {
            _output.WriteLine($"- {user.Descricao}\n");
        }
    
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.Descricao == "Medicamento");
        Assert.Contains(resultado, u => u.Descricao == "João");

        _output.WriteLine("********************( Teste de consulta de categoria concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar todos os Categorias

     Teste será realizado apenas para consultar a lista completa de categorias com cenário vazio

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarTodos_SemCategorias_DeveRetornarListaVazia()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        mockRepo.Setup(repo => repo.ConsultarTodos()).ReturnsAsync(new List<Categoria>());
        var service = new CategoriaService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar lista vazia de categoria )********************\n");

        var resultado = await service.ConsultarTodos();

        _output.WriteLine($"Total de categorias retornados: {resultado.Count}\n");
        Assert.Empty(resultado);

        _output.WriteLine("********************( Teste de consulta vazia concluído )********************\n");
    }

    /*************************************************************************************************
    
     Consultar Categoria por ID

     Teste será realizado apenas para consultar um categoria pelo ID

    **************************************************************************************************/

    [Fact]
    public async Task ConsultarId_DeveRetornarCategoriaPorId()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        
        // Configura o mock para retornar um categoria específico quando consultado pelo ID
        var categoriaEsperado = new Categoria { Descricao = "Medicamento", Id = "123" };
        mockRepo.Setup(repo => repo.ConsultarId("123")).ReturnsAsync(categoriaEsperado);
        
        var service = new CategoriaService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Consultar o categoria pelo Id )********************\n");

        var resultado = await service.ConsultarId("123");

        _output.WriteLine($"Categoria e Id consultado: {resultado?.Descricao}, {resultado?.Id}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de consulta pelo Id concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Categoria

     Teste será realizado apenas para atualizar um categoria existente

    **************************************************************************************************/

    [Fact]
    public async Task Atualizar_DeveRetornarCategoriaAtualizado()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        
        // Categoria original
        var categoriaOriginal = new Categoria { Descricao = "Medicamento", Id = "123" };
        var categoriaAtualizado = new Categoria { Descricao = "Medicamento Atualizado", Id = "123" };
        
        // Configura o mock para retornar o categoria atualizado quando chamado o método de atualização
        mockRepo.Setup(repo => repo.Atualizar(categoriaOriginal)).ReturnsAsync(categoriaAtualizado);
        
        var service = new CategoriaService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualizar todos os dados do categoria )********************\n");

        // Chama o serviço para atualizar o categoria
        var resultado = await service.Atualizar(categoriaOriginal);

        // Verifica se o nome foi atualizado
        _output.WriteLine($"Categoria atualizado: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Atualizado", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);

        _output.WriteLine("********************( Teste de atualização concluído )********************\n");
    }

    /*************************************************************************************************
    
     Atualizar Parcial Categoria

     Teste será realizado apenas para atualizar parcialmente um categoria existente

    **************************************************************************************************/

    [Fact]
    public async Task AtualizarParcial_DeveRetornarCategoriaAtualizadoParcialmente()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        
        // Categoria original
        var categoriaOriginal = new Categoria { Descricao = "Medicamento", Id = "123" };
         
        // Descricao e Email serão atualizados
        var categoriaAtualizado = new Categoria { Descricao = "Medicamento Silva", Id = "123" }; 
        
        // Dados de campos a serem atualizados
        var camposParaAtualizar = new Dictionary<string, object>
        {
            { "Descricao", "Medicamento Silva" },
            { "Email", "claudio.silva@exemplo.com" }
        };
        
        // Configura o mock para retornar o categoria parcialmente atualizado
        mockRepo.Setup(repo => repo.AtualizarParcial("123", camposParaAtualizar)).ReturnsAsync(categoriaAtualizado);
        
        var service = new CategoriaService(mockRepo.Object);

        _output.WriteLine("********************( Chamando o serviço Atualização parcial do categoria )********************\n");

        // Chama o serviço para atualizar parcialmente o categoria
        var resultado = await service.AtualizarParcial("123", camposParaAtualizar);

        // Verifica se os campos foram atualizados
        _output.WriteLine($"Categoria parcialmente atualizado: Descricao: {resultado?.Descricao}\n");
        Assert.NotNull(resultado);
        Assert.Equal("Medicamento Silva", resultado?.Descricao);
        Assert.Equal("123", resultado?.Id);  // ID não foi alterado

        _output.WriteLine("********************( Teste de atualização parcial concluído )********************\n");
    }

    /*************************************************************************************************

    Excluir Categoria

    Teste será realizado para garantir que o método Excluir seja chamado corretamente

    **************************************************************************************************/

    [Fact]
    public async Task Excluir_CategoriaValido_ChamaMetodoExcluirNoRepositorio()
    {
        var mockRepo = new Mock<ICategoriaRepository>();
        var categoriaId = "123";  // ID do categoria a ser excluído
        var service = new CategoriaService(mockRepo.Object);

        // Configurando o mock para simular a exclusão sem retornar nada
        mockRepo.Setup(repo => repo.Excluir(categoriaId)).Returns(Task.CompletedTask);

        // Chama o método Excluir no serviço
        await service.Excluir(categoriaId);

        _output.WriteLine("********************( Chamando o serviço Exclusão do categoria )********************\n");

        _output.WriteLine($"Categoria com ID {categoriaId} excluído\n");

        // Verifica se o método Excluir foi chamado uma vez no repositório com o ID correto
        mockRepo.Verify(repo => repo.Excluir(categoriaId), Times.Once);

        _output.WriteLine("********************( Teste de exclusão concluído )********************\n");
    }





}