using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/Distribuicao")]
public class DistribuicaoController : Controller
{
    private readonly IDistribuicaoService _distribuicaoService;

    public DistribuicaoController(IDistribuicaoService distribuicaoService)
    {
        _distribuicaoService = distribuicaoService;
    }

    /// <summary>
    ///     Cria uma nova distribuição.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar nova distribuição no banco de dados
    /// 
    /// Use este endpoint para cadastrar uma nova distribuição no sistema.
    /// 
    /// ### Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Distribuicao/CadastrarDistribuicao
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar uma nova distribuição:
    /// - **idDoacao** (string): ID da doação relacionada
    /// - **quantidadeDestinada** (int): Quantidade de itens destinados
    /// - **dataDestinada** (string - formato ISO 8601): Data em que a distribuição foi realizada
    /// - **idPessoaAtendida** (string): ID da pessoa atendida pela distribuição
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "idDoacao": "665af8b13fae4c001fcf6d12",
    ///     "quantidadeDestinada": 10,
    ///     "dataDestinada": "2025-05-30T14:30:00",
    ///     "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "665afa483fae4c001fcf6d19",
    ///     "idDoacao": "665af8b13fae4c001fcf6d12",
    ///     "quantidadeDestinada": 10,
    ///     "dataDestinada": "2025-05-30T14:30:00",
    ///     "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="201">Distribuição criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarDistribuicao")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarDistribuicao([FromBody] Distribuicao distribuicao)
    {
        if (ModelState.IsValid)
        {
            await _distribuicaoService.Criar(distribuicao);
            return CreatedAtAction(nameof(ConsultarTodosDistribuicao), new { id = distribuicao.Id }, distribuicao); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todas as distribuições.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de distribuições no banco de dados
    /// 
    /// Use este endpoint para recuperar todas as distribuições armazenadas no banco de dados.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/Distribuicao/ConsultarTodasDistribuicao
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único da distribuição
    /// - **idDoacao** (string): ID da doação associada
    /// - **quantidadeDestinada** (int): Quantidade de itens destinados
    /// - **dataDestinada** (string - formato ISO 8601): Data da distribuição
    /// - **idPessoaAtendida** (string): ID da pessoa atendida
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "665afa483fae4c001fcf6d19",
    ///         "idDoacao": "665af8b13fae4c001fcf6d12",
    ///         "quantidadeDestinada": 10,
    ///         "dataDestinada": "2025-05-30T14:30:00",
    ///         "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de distribuições retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosDistribuicao")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosDistribuicao()
    {
        var distribuicaos = await _distribuicaoService.ConsultarTodos();
        return Ok(distribuicaos);
    }

    /// <summary>
    ///     Consultar um único registro de distribuição.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar uma distribuição pelo ID
    /// 
    /// Use este endpoint para recuperar os dados de uma distribuição específica, informando o ID.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID da distribuição
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/Distribuicao/ConsultarDistribuicaoId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "665afa483fae4c001fcf6d19",
    ///     "idDoacao": "665af8b13fae4c001fcf6d12",
    ///     "quantidadeDestinada": 10,
    ///     "dataDestinada": "2025-05-30T14:30:00",
    ///     "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Distribuição consultada com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Distribuição não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarDistribuicaoId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarDistribuicaoId(string id)
    {
        var distribuicao = await _distribuicaoService.ConsultarId(id);

        if (distribuicao == null)
        {
            return NotFound(new { message = "Distribuicao não encontrado." });
        }

        return Ok(distribuicao);
    }

    /// <summary>
    ///     Atualiza todos os dados da distribuição com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de uma distribuição
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** de uma distribuição.
    /// ⚠️ Se desejar atualizar apenas alguns campos, utilize o endpoint PATCH.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PUT http://localhost:3001/api/Distribuicao/AtualizarDistribuicao/{id}
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "id": "665afa483fae4c001fcf6d19",
    ///     "idDoacao": "665af8b13fae4c001fcf6d12",
    ///     "quantidadeDestinada": 20,
    ///     "dataDestinada": "2025-06-01T10:00:00",
    ///     "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <param name="id" type="string">ID da distribuição no banco de dados.</param>
    /// <param name="distribuicao">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Distribuição atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Distribuição não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarDistribuicao/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarDistribuicao(string id, [FromBody] Distribuicao distribuicao)
    {
        if (string.IsNullOrEmpty(id) || distribuicao == null || id != distribuicao.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var distribuicaoExistente = await _distribuicaoService.ConsultarId(id);

        if (distribuicaoExistente == null)
        {
            return NotFound();
        }


        distribuicaoExistente.IdDoacao = distribuicao.IdDoacao;
        distribuicaoExistente.QuantidadeDestinada = distribuicao.QuantidadeDestinada;
        distribuicaoExistente.DataDestinada = distribuicao.DataDestinada;
        distribuicaoExistente.IdPessoaAtendida = distribuicao.IdPessoaAtendida;

        await _distribuicaoService.Atualizar(distribuicaoExistente);

        return Ok(distribuicaoExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de uma distribuição existente.
    /// </summary>
    /// 
    /// <param name="id" type="string">ID da distribuição a ser atualizada.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização parcial de uma distribuição
    /// 
    /// Use este endpoint para atualizar **apenas alguns campos** da distribuição.
    /// Somente os campos enviados no body serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados**:
    /// - **id**: O ID da distribuição não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PATCH http://localhost:3001/api/Distribuicao/AtualizarParcial/{id}
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **idDoacao** (string)
    /// - **quantidadeDestinada** (int)
    /// - **dataDestinada** (string - formato ISO 8601)
    /// - **idPessoaAtendida** (string)
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "quantidadeDestinada": 25,
    ///     "dataDestinada": "2025-06-02T09:00:00"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "665afa483fae4c001fcf6d19",
    ///     "idDoacao": "665af8b13fae4c001fcf6d12",
    ///     "quantidadeDestinada": 25,
    ///     "dataDestinada": "2025-06-02T09:00:00",
    ///     "idPessoaAtendida": "665af9a03fae4c001fcf6d15"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Distribuição atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Distribuição não encontrada</response>
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

        var distribuicaoAtualizado = await _distribuicaoService.AtualizarParcial(id, camposParaAtualizar);

        if (distribuicaoAtualizado == null)
        {
            return NotFound(" Distribuicao não encontrado.");
        }

        return Ok(distribuicaoAtualizado);
    }

    /// <summary>
    ///     Exclui uma distribuição do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string">ID da distribuição a ser excluída.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir uma distribuição do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente uma distribuição da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Distribuicao/ExcluirDistribuicao/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Distribuição excluída com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Distribuição excluída com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Distribuição não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirDistribuicao/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirDistribuicao(string id)
    {
        var distribuicao = await _distribuicaoService.ConsultarId(id);
        
        if (distribuicao == null)
        {
            return NotFound();
        }

        await _distribuicaoService.Excluir(id);

        return Ok(new { message = "Distribuicao excluído com sucesso." });  
    }
}
