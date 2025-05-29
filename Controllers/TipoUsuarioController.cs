using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/TipoUsuario")]
public class TipoUsuarioController : Controller
{
    private readonly ITipoUsuarioService _tipoUsuarioService;

    public TipoUsuarioController(ITipoUsuarioService tipoUsuarioService)
    {
        _tipoUsuarioService = tipoUsuarioService;
    }

    /// <summary>
    ///     Cria um novo tipo de usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo tipo de usuário no banco de dados
    /// 
    /// Use este endpoint para cadastrar um tipo de usuário no sistema. O tipo de usuário define o perfil e permissões do usuário dentro da plataforma.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/TipoUsuario/CadastrarTipoUsuario
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar um novo tipo de usuário:
    /// - **descricao** (string): Descrição do tipo de usuário (Ex: "Comum", "Administrador", etc.)
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "descricao": "Comum"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Comum"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="201">Tipo de usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarTipoUsuario")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarTipoUsuario([FromBody] TipoUsuario tipoUsuario)
    {
        if (ModelState.IsValid)
        {
            await _tipoUsuarioService.Criar(tipoUsuario);
            return CreatedAtAction(nameof(ConsultarTodosTiposUsuario), new { id = tipoUsuario.Id }, tipoUsuario); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todos os tipos de usuários.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de tipos de usuários do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os tipos de usuários armazenados no banco de dados. O tipo de usuário define o perfil e as permissões dos usuários dentro da plataforma.
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
    /// - **id** (string): Identificador único do tipo de usuário (gerado automaticamente pelo banco)
    /// - **descricao** (string): Descrição do tipo de usuário (Ex: "Comum", "Administrador", etc.)
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6d93",
    ///         "descricao": "Comum"
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
    /// <response code="200">Lista de tipos de usuários retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosTiposUsuario")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosTiposUsuario()
    {
        var tipoUsuarios = await _tipoUsuarioService.ConsultarTodos();
        return Ok(tipoUsuarios);
    }

    /// <summary>
    ///     Consultar um único registro de tipo de usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único tipo de usuário pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar os dados de um tipo de usuário específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do tipo de usuário (gerado automaticamente pelo MongoDB)
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
    ///     "descricao": "Comum"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Tipo de usuário consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Tipo de usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTipoUsuarioId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarTipoUsuarioId(string id)
    {
        var tipoUsuario = await _tipoUsuarioService.ConsultarId(id);

        if (tipoUsuario == null)
        {
            return NotFound(new { message = "Tipo de usuário não encontrado." });
        }

        return Ok(tipoUsuario);
    }

    /// <summary>
    ///     Atualiza todos os dados do tipo de usuário com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um tipo de usuário no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um tipo de usuário.
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
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de usuário no banco de dados.</param>
    /// <param name="tipoUsuario">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Tipo de usuário atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Tipo de usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarTipoUsuario/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarTipoUsuario(string id, [FromBody] TipoUsuario tipoUsuario)
    {
        if (string.IsNullOrEmpty(id) || tipoUsuario == null || id != tipoUsuario.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var tipoUsuarioExistente = await _tipoUsuarioService.ConsultarId(id);

        if (tipoUsuarioExistente == null)
        {
            return NotFound();
        }


        tipoUsuarioExistente.Descricao = tipoUsuario.Descricao;
      

        await _tipoUsuarioService.Atualizar(tipoUsuarioExistente);

        return Ok(tipoUsuarioExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um tipo de usuário existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de usuário a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um Tipo de Usuário
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do tipo de usuário,
    /// sem a necessidade de enviar todos os dados já cadastrados.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **id**: O ID do tipo de usuário não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/TipoUsuario/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao** (string): Descrição do tipo de usuário (Ex: "Administrador", "Comum")
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
    /// <response code="200">Tipo de usuário atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Tipo de usuário não encontrado</response>
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

        var tipoUsuarioAtualizado = await _tipoUsuarioService.AtualizarParcial(id, camposParaAtualizar);

        if (tipoUsuarioAtualizado == null)
        {
            return NotFound("Tipo de Usuário não encontrado.");
        }

        return Ok(tipoUsuarioAtualizado);
    }

    /// <summary>
    ///     Exclui um tipo de usuário do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de usuário a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um Tipo de Usuário do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um tipo de usuário da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/TipoUsuario/ExcluirTipoUsuario/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Tipo de usuário excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Tipo de usuário excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Tipo de usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirTipoUsuario/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirTipoUsuario(string id)
    {
        var tipoUsuario = await _tipoUsuarioService.ConsultarId(id);
        
        if (tipoUsuario == null)
        {
            return NotFound();
        }

        await _tipoUsuarioService.Excluir(id);

        return Ok(new { message = "Tipo de usuário excluído com sucesso." });  
    }
}
