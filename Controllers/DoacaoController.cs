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
    /// POST http://localhost:3001/api/Doacao/CadastrarDoacao
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
    /// <response code="201">Doacao criado com sucesso</response>
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
    /// GET http://localhost:3001/api/Doacao/ConsultarTodosDoacao
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
    [HttpGet("ConsultarTodosDoacao")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosDoacao()
    {
        var doacaos = await _doacaoService.ConsultarTodos();
        return Ok(doacaos);
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
    /// GET http://localhost:3001/api/Doacao/ConsultarDoacaoId/{id}
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
    /// <response code="200">Doacao consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Doacao não encontrado</response>
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
    /// PUT http://localhost:3001/api/Doacao/AtualizarDoacao/{id}
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
    /// <param name="doacao">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Doacao atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Doacao não encontrado</response>
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
      

        await _doacaoService.Atualizar(doacaoExistente);

        return Ok(doacaoExistente); 
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
    /// ## Atualização Parcial de um  Doacao
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
    /// PATCH http://localhost:3001/api/Doacao/AtualizarParcial/{id}
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
    /// <response code="200">Doacao atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Doacao não encontrado</response>
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
    ///     Exclui um tipo de tipo de usuário do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do tipo de tipo de usuário a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um  Doacao do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um tipo de tipo de usuário da base de dados.
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
    ///     "message": "Doacao excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Doacao excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Doacao não encontrado</response>
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
