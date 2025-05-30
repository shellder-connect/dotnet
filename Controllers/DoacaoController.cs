using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/Doacao")]
public class DoacaoController : Controller
{
    private readonly IDoacaoService _doacaoService;

    public DoacaoController(IDoacaoService doacaoService)
    {
        _doacaoService = doacaoService;
    }

   /// <summary>
    ///     Cria uma nova doação.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar nova doação no banco de dados
    /// 
    /// Use este endpoint para cadastrar uma nova doação no sistema.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Doacao/CadastrarDoacao
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que devem ser utilizados para criar uma nova doação:
    /// - **idAbrigo** (string): ID do abrigo associado (ex: "665c1e9c3fae4c001fcf6d87")
    /// - **descricao** (string): Descrição da doação (ex: "Cestas básicas")
    /// - **idCategoria** (string): ID da categoria (ex: "665c1ea83fae4c001fcf6d89")
    /// - **quantidade** (int): Quantidade de itens doados
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Cestas básicas",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 50
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "665c1f983fae4c001fcf6d88",
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Cestas básicas",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 50
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="201">Doação criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarDoacao")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarDoacao([FromBody] Doacao doacao)
    {
        if (ModelState.IsValid)
        {
            await _doacaoService.Criar(doacao);
            return CreatedAtAction(nameof(ConsultarTodosDoacao), new { id = doacao.Id }, doacao); 
        }
        return BadRequest(ModelState); 
    }

    /// <summary>
    ///     Consultar a lista com todas as doações.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de doações no banco de dados
    /// 
    /// Use este endpoint para recuperar todas as doações cadastradas.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/Doacao/ConsultarTodosDoacao
    /// ```
    /// 
    /// ### Campos retornados:
    /// - **id** (string): ID da doação
    /// - **idAbrigo** (string): ID do abrigo associado
    /// - **descricao** (string): Descrição da doação
    /// - **idCategoria** (string): ID da categoria
    /// - **quantidade** (int): Quantidade de itens doados
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///   {
    ///     "id": "665c1f983fae4c001fcf6d88",
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Cestas básicas",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 50
    ///   }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de doações retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosDoacao")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosDoacao()
    {
        var doacaos = await _doacaoService.ConsultarTodos();
        return Ok(doacaos);
    }

    /// <summary>
    ///     Consultar um único registro de doação.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar uma doação pelo ID
    /// 
    /// Use este endpoint para recuperar os dados de uma doação específica, informando o ID.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID da doação
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/Doacao/ConsultarDoacaoId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "665c1f983fae4c001fcf6d88",
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Cestas básicas",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 50
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Doação consultada com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Doação não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarDoacaoId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarDoacaoId(string id)
    {
        var doacao = await _doacaoService.ConsultarId(id);

        if (doacao == null)
        {
            return NotFound(new { message = "Doacao não encontrado." });
        }

        return Ok(doacao);
    }

    /// <summary>
    ///     Atualiza todos os dados da doação com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de uma doação
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** de uma doação.
    /// ⚠️ Se desejar atualizar apenas alguns campos, utilize o endpoint PATCH.
    /// 
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PUT http://localhost:3001/api/Doacao/AtualizarDoacao/{id}
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "id": "665c1f983fae4c001fcf6d88",
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Doação de roupas",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 100
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <param name="id" type="string">ID da doação no banco de dados.</param>
    /// <param name="doacao">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Doação atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Doação não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarDoacao/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarDoacao(string id, [FromBody] Doacao doacao)
    {
        if (string.IsNullOrEmpty(id) || doacao == null || id != doacao.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var doacaoExistente = await _doacaoService.ConsultarId(id);

        if (doacaoExistente == null)
        {
            return NotFound();
        }

        doacaoExistente.Descricao = doacao.Descricao;
        doacaoExistente.Quantidade = doacao.Quantidade;
      
        await _doacaoService.Atualizar(doacaoExistente);

        return Ok(doacaoExistente); 
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de uma doação existente.
    /// </summary>
    /// 
    /// <param name="id" type="string">ID da doação a ser atualizada.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização parcial de uma doação
    /// 
    /// Use este endpoint para atualizar **apenas alguns campos** da doação.
    /// Somente os campos enviados no body serão modificados.
    /// 
    /// ⚠️ Campos que **não podem ser atualizados**:
    /// - **id**: O ID da doação não pode ser alterado.
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// PATCH http://localhost:3001/api/Doacao/AtualizarParcial/{id}
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **idAbrigo** (string)
    /// - **descricao** (string)
    /// - **idCategoria** (string)
    /// - **quantidade** (int)
    /// 
    /// ### Exemplo de body:
    /// ```json
    /// {
    ///     "descricao": "Doação de alimentos",
    ///     "quantidade": 80
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "665c1f983fae4c001fcf6d88",
    ///     "idAbrigo": "665c1e9c3fae4c001fcf6d87",
    ///     "descricao": "Doação de alimentos",
    ///     "idCategoria": "665c1ea83fae4c001fcf6d89",
    ///     "quantidade": 80
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Doação atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Doação não encontrada</response>
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

        var doacaoAtualizado = await _doacaoService.AtualizarParcial(id, camposParaAtualizar);

        if (doacaoAtualizado == null)
        {
            return NotFound(" Doacao não encontrado.");
        }

        return Ok(doacaoAtualizado);
    }

    /// <summary>
    ///     Exclui uma doação do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string">ID da doação a ser excluída.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir uma doação do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente uma doação da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Doacao/ExcluirDoacao/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Doação excluída com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Doação excluída com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Doação não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirDoacao/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirDoacao(string id)
    {
        var doacao = await _doacaoService.ConsultarId(id);
        
        if (doacao == null)
        {
            return NotFound();
        }

        await _doacaoService.Excluir(id);

        return Ok(new { message = "Doacao excluído com sucesso." });  
    }
}
