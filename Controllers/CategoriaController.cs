using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/Categoria")]
public class CategoriaController : Controller
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    /// <summary>
    ///     Cria uma nova categoria.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar nova categoria no banco de dados
    /// 
    /// Use este endpoint para cadastrar uma categoria no sistema.
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Categoria/CadastrarCategoria
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos necessários para criar uma nova categoria:
    /// - **descricao** (string): Descrição da categoria (Ex: "Alimentos", "Higiene", etc.)
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body da requisição:
    /// ```json
    /// {
    ///     "descricao": "Alimentos"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Alimentos"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="201">Categoria criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarCategoria")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarCategoria([FromBody] Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            await _categoriaService.Criar(categoria);
            return CreatedAtAction(nameof(ConsultarTodasCategorias), new { id = categoria.Id }, categoria); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todas as categorias.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todas as categorias do banco de dados
    /// 
    /// Use este endpoint para recuperar todas as categorias armazenadas.
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Categoria/ConsultarTodasCategorias
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos na resposta:
    /// - **id** (string): Identificador único da categoria
    /// - **descricao** (string): Descrição da categoria
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6d93",
    ///         "descricao": "Alimentos"
    ///     },
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6e01",
    ///         "descricao": "Higiene"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de categorias retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodasCategorias")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodasCategorias()
    {
        var categorias = await _categoriaService.ConsultarTodos();
        return Ok(categorias);
    }

    /// <summary>
    ///     Consultar uma única categoria.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar uma categoria pelo ID
    /// 
    /// Use este endpoint para recuperar os dados de uma categoria específica, informando o ID.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID da categoria
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Categoria/ConsultarCategoriaId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Alimentos"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria consultada com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Categoria não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarCategoriaId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarCategoriaId(string id)
    {
        var categoria = await _categoriaService.ConsultarId(id);

        if (categoria == null)
        {
            return NotFound(new { message = "Categoria não encontrado." });
        }

        return Ok(categoria);
    }

    /// <summary>
    ///     Atualiza todos os dados da categoria com base no ID.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID da categoria no banco de dados.</param>
    /// <param name="categoria">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de uma categoria
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** da categoria.
    /// ⚠️ Campos não enviados serão sobrescritos com valores nulos.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PUT http://localhost:3001/api/Categoria/AtualizarCategoria/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Higiene"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Higiene"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Categoria não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarCategoria/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarCategoria(string id, [FromBody] Categoria categoria)
    {
        if (string.IsNullOrEmpty(id) || categoria == null || id != categoria.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var categoriaExistente = await _categoriaService.ConsultarId(id);

        if (categoriaExistente == null)
        {
            return NotFound();
        }


        categoriaExistente.Descricao = categoria.Descricao;

        await _categoriaService.Atualizar(categoriaExistente);

        return Ok(categoriaExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de uma categoria.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID da categoria a ser atualizada.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de Categoria
    /// 
    /// Use este endpoint quando quiser atualizar apenas **alguns campos** da categoria.
    /// 
    /// ⚠️ Não é permitido atualizar o campo **id**.
    /// 
    /// ### Campo que pode ser atualizado:
    /// - **descricao** (string)
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Categoria/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "descricao": "Higiene Pessoal"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Higiene Pessoal"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Categoria não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPatch("AtualizarParcial/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarParcial(string id, [FromBody] Dictionary<string, object> camposParaAtualizar)
    {
        if (string.IsNullOrEmpty(id) || camposParaAtualizar == null || !camposParaAtualizar.Any())
        {
            return BadRequest("Id dos campos     ///  atualização são necessários.");
        }

        var categoriaAtualizado = await _categoriaService.AtualizarParcial(id, camposParaAtualizar);

        if (categoriaAtualizado == null)
        {
            return NotFound("Categoria não encontrado.");
        }

        return Ok(categoriaAtualizado);
    }

    /// <summary>
    ///     Exclui uma categoria do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID da categoria a ser excluída.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir uma Categoria do banco de dados
    /// 
    /// Use este endpoint para remover uma categoria permanentemente.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// DELETE http://localhost:3001/api/Categoria/ExcluirCategoria/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Categoria excluída com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria excluída com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Categoria não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirCategoria/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirCategoria(string id)
    {
        var categoria = await _categoriaService.ConsultarId(id);
        
        if (categoria == null)
        {
            return NotFound();
        }

        await _categoriaService.Excluir(id);

        return Ok(new { message = "Excluído com sucesso." });  
    }
}
