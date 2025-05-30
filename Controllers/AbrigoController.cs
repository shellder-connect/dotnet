using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/Abrigo")]
public class AbrigoController : Controller
{
    private readonly IAbrigoService _abrigoService;

    public AbrigoController(IAbrigoService abrigoService)
    {
        _abrigoService = abrigoService;
    }

    /// <summary>
    ///     Cria um novo tipo de tipo de usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo tipo de tipo de usuário no banco de dados
    /// 
    /// Use este endpoint para cadastrar um tipo de tipo de usuário no sistema. O tipo de tipo de usuário define o perfil e permissões do tipo de usuário dentro da plataforma.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Abrigo/CadastrarAbrigo
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar um novo tipo de tipo de usuário:
    /// - **descricao** (string): Descrição do tipo de tipo de usuário (Ex: "Comum", "Administrador", etc.)
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
    /// <response code="201">Abrigo criado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarAbrigo")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarAbrigo([FromBody] Abrigo abrigo)
    {
        if (ModelState.IsValid)
        {
            await _abrigoService.Criar(abrigo);
            return CreatedAtAction(nameof(ConsultarTodosAbrigo), new { id = abrigo.Id }, abrigo); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todos os tipos de tipo de usuários.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de tipos de tipo de usuários do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os tipos de tipo de usuários armazenados no banco de dados. O tipo de tipo de usuário define o perfil e as permissões dos tipo de usuários dentro da plataforma.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Abrigo/ConsultarTodosAbrigo
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do tipo de tipo de usuário (gerado automaticamente pelo banco)
    /// - **descricao** (string): Descrição do tipo de tipo de usuário (Ex: "Comum", "Administrador", etc.)
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
    /// <response code="200">Lista de tipos de tipo de usuários retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosAbrigo")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosAbrigo()
    {
        var abrigos = await _abrigoService.ConsultarTodos();
        return Ok(abrigos);
    }

    /// <summary>
    ///     Consultar um único registro de tipo de tipo de usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único tipo de tipo de usuário pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar os dados de um tipo de tipo de usuário específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do tipo de tipo de usuário (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Abrigo/ConsultarAbrigoId/{id}
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
    /// <response code="200">Abrigo consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Abrigo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarAbrigoId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarAbrigoId(string id)
    {
        var abrigo = await _abrigoService.ConsultarId(id);

        if (abrigo == null)
        {
            return NotFound(new { message = "Abrigo não encontrado." });
        }

        return Ok(abrigo);
    }

    /// <summary>
    ///     Atualiza todos os dados do tipo de tipo de usuário com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um tipo de tipo de usuário no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um tipo de tipo de usuário.
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/Abrigo/AtualizarAbrigo/{id}
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
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de tipo de usuário no banco de dados.</param>
    /// <param name="abrigo">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Abrigo atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Abrigo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarAbrigo/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarAbrigo(string id, [FromBody] Abrigo abrigo)
    {
        if (string.IsNullOrEmpty(id) || abrigo == null || id != abrigo.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var abrigoExistente = await _abrigoService.ConsultarId(id);

        if (abrigoExistente == null)
        {
            return NotFound();
        }


        abrigoExistente.Descricao = abrigo.Descricao;
      

        await _abrigoService.Atualizar(abrigoExistente);

        return Ok(abrigoExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um tipo de tipo de usuário existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de tipo de usuário a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um  Abrigo
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do tipo de tipo de usuário,
    /// sem a necessidade de enviar todos os dados já cadastrados.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **id**: O ID do tipo de tipo de usuário não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Abrigo/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao** (string): Descrição do tipo de tipo de usuário (Ex: "Administrador", "Comum")
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
    /// <response code="200">Abrigo atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Abrigo não encontrado</response>
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

        var abrigoAtualizado = await _abrigoService.AtualizarParcial(id, camposParaAtualizar);

        if (abrigoAtualizado == null)
        {
            return NotFound(" Abrigo não encontrado.");
        }

        return Ok(abrigoAtualizado);
    }

    /// <summary>
    ///     Exclui um tipo de tipo de usuário do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de tipo de usuário a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um  Abrigo do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um tipo de tipo de usuário da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Abrigo/ExcluirAbrigo/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Abrigo excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Abrigo excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Abrigo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirAbrigo/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirAbrigo(string id)
    {
        var abrigo = await _abrigoService.ConsultarId(id);
        
        if (abrigo == null)
        {
            return NotFound();
        }

        await _abrigoService.Excluir(id);

        return Ok(new { message = "Abrigo excluído com sucesso." });  
    }
}
