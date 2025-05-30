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
    ///     Cria um novo distribuicao.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo distribuicao no banco de dados
    /// 
    /// Use este endpoint para cadastrar um distribuicao no sistema.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Distribuicao/CadastrarDistribuicao
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar um novo distribuicao:
    /// - **descricao** (string): Descrição do distribuicao (Ex: "Comum", "Administrador", etc.)
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
    /// <response code="201">Distribuicao criado com sucesso</response>
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
    ///     Consultar a lista com todos os distribuições.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de distribuições do banco de dados
    /// 
    /// Use este endpoint para recuperar todos os distribuições armazenados no banco de dados. O distribuicao define o perfil e as permissões dos tipo de usuários dentro da plataforma.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Distribuicao/ConsultarTodosDistribuicao
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do distribuicao (gerado automaticamente pelo banco)
    /// - **descricao** (string): Descrição do distribuicao (Ex: "Comum", "Administrador", etc.)
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
    ///     Consultar um único registro de distribuicao.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único distribuicao pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar os dados de um distribuicao específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do distribuicao (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Distribuicao/ConsultarDistribuicaoId/{id}
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
    /// <response code="200">Distribuicao consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Distribuicao não encontrado</response>
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
    ///     Atualiza todos os dados do distribuicao com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um distribuicao no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um distribuicao.
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/Distribuicao/AtualizarDistribuicao/{id}
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
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do distribuicao no banco de dados.</param>
    /// <param name="distribuicao">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Distribuicao atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Distribuicao não encontrado</response>
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
    ///     Atualiza parcialmente os dados de um distribuicao existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do distribuicao a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um  Distribuicao
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do distribuicao,
    /// sem a necessidade de enviar todos os dados já cadastrados.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **id**: O ID do distribuicao não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Distribuicao/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **descricao** (string): Descrição do distribuicao (Ex: "Administrador", "Comum")
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
    /// <response code="200">Distribuicao atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Distribuicao não encontrado</response>
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
    ///     Exclui um distribuicao do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do distribuicao a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um  Distribuicao do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um distribuicao da base de dados.
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
    ///     "message": "Distribuicao excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Distribuicao excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Distribuicao não encontrado</response>
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
