using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/RegistroEvento")]
public class RegistroEventoController : Controller
{
    private readonly IRegistroEventoService _registroEventoService;

    public RegistroEventoController(IRegistroEventoService registroEventoService)
    {
        _registroEventoService = registroEventoService;
    }

    /// <summary>
    ///     Cria um novo registro do evento.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo registro do evento no banco de dados
    /// 
    /// Use este endpoint para cadastrar um registro do evento no sistema.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/RegistroEvento/CadastrarRegistroEvento
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar um novo registro do evento:
    /// - **descricao** (string): Descrição do registro do evento (Ex: "Comum", "Administrador", etc.)
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
    /// <response code="201">RegistroEvento criado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarRegistroEvento")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarRegistroEvento([FromBody] RegistroEvento registroEvento)
    {
        if (ModelState.IsValid)
        {
            await _registroEventoService.Criar(registroEvento);
            return CreatedAtAction(nameof(ConsultarTodosRegistroEvento), new { id = registroEvento.Id }, registroEvento); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todos os tipos de registro do eventos.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de tipos de registro do eventos do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os tipos de registro do eventos armazenados no banco de dados. O registro do evento define o perfil e as permissões dos registro do eventos dentro da plataforma.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/RegistroEvento/ConsultarTodosRegistroEvento
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do registro do evento (gerado automaticamente pelo banco)
    /// - **descricao** (string): Descrição do registro do evento (Ex: "Comum", "Administrador", etc.)
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
    /// <response code="200">Lista de tipos de registro do eventos retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosRegistroEvento")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosRegistroEvento()
    {
        var registroEventos = await _registroEventoService.ConsultarTodos();
        return Ok(registroEventos);
    }

    /// <summary>
    ///     Consultar um único registro de registro do evento.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único registro do evento pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar os dados de um registro do evento específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do registro do evento (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/RegistroEvento/ConsultarRegistroEventoId/{id}
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
    /// <response code="200">RegistroEvento consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">RegistroEvento não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarRegistroEventoId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarRegistroEventoId(string id)
    {
        var registroEvento = await _registroEventoService.ConsultarId(id);

        if (registroEvento == null)
        {
            return NotFound(new { message = "RegistroEvento não encontrado." });
        }

        return Ok(registroEvento);
    }

    /// <summary>
    ///     Atualiza todos os dados do registro do evento com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um registro do evento no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um registro do evento.
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/RegistroEvento/AtualizarRegistroEvento/{id}
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
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do registro do evento no banco de dados.</param>
    /// <param name="registroEvento">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">RegistroEvento atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">RegistroEvento não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarRegistroEvento/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarRegistroEvento(string id, [FromBody] RegistroEvento registroEvento)
    {
        if (string.IsNullOrEmpty(id) || registroEvento == null || id != registroEvento.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var registroEventoExistente = await _registroEventoService.ConsultarId(id);

        if (registroEventoExistente == null)
        {
            return NotFound();
        }


        registroEventoExistente.Descricao = registroEvento.Descricao;
        registroEventoExistente.DataHora = registroEvento.DataHora;
        registroEventoExistente.IdUsuario = registroEvento.IdUsuario;
        registroEventoExistente.Localizacao = registroEvento.Localizacao;
        registroEventoExistente.IdAbrigo = registroEvento.IdAbrigo;

        await _registroEventoService.Atualizar(registroEventoExistente);

        return Ok(registroEventoExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um registro do evento existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do registro do evento a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um  RegistroEvento
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do registro do evento,
    /// sem a necessidade de enviar todos os dados já cadastrados.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **id**: O ID do registro do evento não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/RegistroEvento/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao** (string): Descrição do registro do evento (Ex: "Administrador", "Comum")
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
    /// <response code="200">RegistroEvento atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">RegistroEvento não encontrado</response>
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

        var registroEventoAtualizado = await _registroEventoService.AtualizarParcial(id, camposParaAtualizar);

        if (registroEventoAtualizado == null)
        {
            return NotFound(" RegistroEvento não encontrado.");
        }

        return Ok(registroEventoAtualizado);
    }

    /// <summary>
    ///     Exclui um registro do evento do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do registro do evento a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um  RegistroEvento do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um registro do evento da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/RegistroEvento/ExcluirRegistroEvento/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "RegistroEvento excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">RegistroEvento excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">RegistroEvento não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirRegistroEvento/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirRegistroEvento(string id)
    {
        var registroEvento = await _registroEventoService.ConsultarId(id);
        
        if (registroEvento == null)
        {
            return NotFound();
        }

        await _registroEventoService.Excluir(id);

        return Ok(new { message = "RegistroEvento excluído com sucesso." });  
    }
}
