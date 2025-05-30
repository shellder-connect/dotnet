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
    ///     Cria um novo registro de evento.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo registro de evento no banco de dados
    /// 
    /// Use este endpoint para cadastrar um registro de evento no sistema.
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
    /// ### Campos que devem ser utilizados para criar um novo registro:
    /// - **descricao** (string): Descrição do evento
    /// - **dataHora** (DateTime): Data e hora do evento (local)
    /// - **idUsuario** (string): ID do usuário associado
    /// - **localizacao** (string): Local onde ocorreu o evento
    /// - **idAbrigo** (string): ID do abrigo associado
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "descricao": "Reunião de voluntários",
    ///     "dataHora": "2025-05-30T14:00:00",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "localizacao": "Sala 5",
    ///     "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d94",
    ///     "descricao": "Reunião de voluntários",
    ///     "dataHora": "2025-05-30T14:00:00",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "localizacao": "Sala 5",
    ///     "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="201">Registro de evento criado com sucesso</response>
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
    ///     Consultar todos os registros de eventos.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de eventos do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os registros de eventos armazenados no banco.
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/RegistroEvento/ConsultarTodosRegistroEvento
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do registro (gerado automaticamente)
    /// - **descricao** (string): Descrição do evento
    /// - **dataHora** (DateTime): Data e hora do evento
    /// - **idUsuario** (string): ID do usuário associado
    /// - **localizacao** (string): Local do evento
    /// - **idAbrigo** (string): ID do abrigo associado
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6d94",
    ///         "descricao": "Reunião de voluntários",
    ///         "dataHora": "2025-05-30T14:00:00",
    ///         "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///         "localizacao": "Sala 5",
    ///         "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de registros retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosRegistroEvento")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosRegistroEvento()
    {
        var registroEventos = await _registroEventoService.ConsultarTodos();
        return Ok(registroEventos);
    }

    /// <summary>
    ///     Consultar um único registro de evento pelo ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar registro pelo ID
    /// 
    /// Use este endpoint para recuperar os dados de um registro específico, informando seu ID.
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/RegistroEvento/ConsultarRegistroEventoId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d94",
    ///     "descricao": "Reunião de voluntários",
    ///     "dataHora": "2025-05-30T14:00:00",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "localizacao": "Sala 5",
    ///     "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Registro consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Registro não encontrado</response>
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
    ///     Atualiza todos os dados de um registro de evento com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um registro no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um registro de evento.
    /// ⚠️ Para atualização parcial, utilize o endpoint PATCH.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/RegistroEvento/AtualizarRegistroEvento/{id}
    /// ```
    /// 
    /// ### Exemplo de requisição:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d94",
    ///     "descricao": "Nova descrição",
    ///     "dataHora": "2025-06-01T10:00:00",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "localizacao": "Nova sala",
    ///     "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Registro atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Registro não encontrado</response>
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
    ///     Atualiza parcialmente os dados de um registro de evento existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d94">ID do registro a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um Registro de Evento
    /// 
    /// Use este endpoint para atualizar apenas alguns campos, sem enviar todos os dados.
    /// Somente os campos incluídos na requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados**:
    /// - **id**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PATCH http://localhost:3001/api/RegistroEvento/AtualizarParcial/{id}
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao**
    /// - **dataHora**
    /// - **idUsuario**
    /// - **localizacao**
    /// - **idAbrigo**
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "descricao": "Descrição atualizada",
    ///     "localizacao": "Auditório"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d94",
    ///     "descricao": "Descrição atualizada",
    ///     "dataHora": "2025-05-30T14:00:00",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "localizacao": "Auditório",
    ///     "idAbrigo": "6659fbbd3fae4c001fcf6e01"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Registro atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Registro não encontrado</response>
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
    ///     Exclui um registro de evento do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d94">ID do registro a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um registro do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um registro.
    /// ⚠️ Exclusão é irreversível.
    /// 
    /// Requisição via rota:
    /// ```http
    /// DELETE http://localhost:3001/api/RegistroEvento/ExcluirRegistroEvento/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Registro excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Registro excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Registro não encontrado</response>
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
