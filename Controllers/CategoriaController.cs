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
    ///     Cria um novo Categoria.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo Categoria no banco de dados
    /// 
    /// Use este endpoint para cadastrar um Categoria no sistema.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Categoria/CadastrarCategoria
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar um novo Categoria:
    /// - **descricao** (string): Descrição do Categoria (Ex: "Vestuário", "Administrador", etc.)
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "descricao": "Vestuário"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Vestuário"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="201">Categoria criado com sucesso</response>
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
    ///     Consultar a lista com todos os tipos de categorias.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de tipos de categorias do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os tipos de categorias armazenados no banco de dados. O Categoria define o perfil e as permissões dos categorias dentro da plataforma.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/TipoUsuario/ConsultarTodosTiposUsuario
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do Categoria (gerado automaticamente pelo banco)
    /// - **descricao** (string): Descrição do Categoria (Ex: "Vestuário", "Administrador", etc.)
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6d93",
    ///         "descricao": "Vestuário"
    ///     },
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6e01",
    ///         "descricao": "Administrador"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de tipos de categorias retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodasCategorias")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodasCategorias()
    {
        var categorias = await _categoriaService.ConsultarTodos();
        return Ok(categorias);
    }

    /// <summary>
    ///     Consultar um único registro de Categoria.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único Categoria pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar os dados de um Categoria específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do Categoria (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/TipoUsuario/ConsultarTipoUsuarioId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Vestuário"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Categoria não encontrado</response>
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
    ///     Atualiza todos os dados do Categoria com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um Categoria no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um Categoria.
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/TipoUsuario/AtualizarTipoUsuario/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de requisição para atualizar os dados:
    /// 
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Administrador"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do Categoria no banco de dados.</param>
    /// <param name="categoria">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Categoria atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Categoria não encontrado</response>
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
    ///     Atualiza parcialmente os dados de um Categoria existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do Categoria a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um Categoria
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do Categoria,
    /// sem a necessidade de enviar todos os dados já cadastrados.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **id**: O ID do Categoria não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Categoria/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao** (string): Descrição do Categoria (Ex: "Administrador", "Vestuário")
    /// 
    /// ### Exemplo de requisição:
    /// ```json
    /// {
    ///     "descricao": "Administrador"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Administrador"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Categoria não encontrado</response>
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
    ///     Exclui um Categoria do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do Categoria a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um Categoria do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um Categoria da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Categoria/Excluir/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Categoria excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Categoria excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Categoria não encontrado</response>
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
