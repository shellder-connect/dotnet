using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("Mural")]
public class MuralController : Controller
{
    private readonly IMuralService _muralService;

    public MuralController(IMuralService muralService)
    {
        _muralService = muralService;
    }

    [HttpGet("Criar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Criar()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost("Criar")]
    [ValidateAntiForgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Criar([Bind("Id,Nome,Email,Senha,IdTipoMural,Telefone,IdEndereco,DataNascimento,Documento,Status")] Mural mural)
    {
        if (ModelState.IsValid)
        {
            await _muralService.Criar(mural);
            TempData["SuccessMessage"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Mensagem");
        }
        return View(mural);
    }

    /// <summary>
    ///     Cria um novo post no mural.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo post no mural
    /// 
    /// Use este endpoint para cadastrar um novo post no mural da plataforma.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Mural/CadastrarMural
    /// ```
    ///
    /// ```http
    /// Content-Type: application/json
    /// ```
    ///
    /// ### Campos que devem ser utilizados para criar um novo post:
    /// - **idUsuario** (string): ID do usuário que está criando o post
    /// - **titulo** (string): Título do post no mural
    /// - **comentario** (string): Conteúdo/comentário do post
    /// - **tipoPost** (string): Tipo do post (Ex: "Texto", "Imagem", "Anúncio", etc.)
    /// - **data** (datetime): Data e hora de criação do post (formato ISO)
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "titulo": "Bem-vindos ao nosso mural!",
    ///     "comentario": "Este é o primeiro post do nosso mural comunitário. Sintam-se à vontade para compartilhar suas ideias!",
    ///     "tipoPost": "Texto",
    ///     "data": "2024-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6e10",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "titulo": "Bem-vindos ao nosso mural!",
    ///     "comentario": "Este é o primeiro post do nosso mural comunitário. Sintam-se à vontade para compartilhar suas ideias!",
    ///     "tipoPost": "Texto",
    ///     "data": "2024-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    [HttpPost("CadastrarMural")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarMural([FromBody] Mural mural)
    {
        if (ModelState.IsValid)
        {
            await _muralService.Criar(mural);
            return CreatedAtAction(nameof(ConsultarTodosMurals), new { id = mural.Id }, mural); 
        }
        return BadRequest(ModelState); 
    }


    // Rota de View
    [HttpGet("Consultar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Consultar()
    {
        var murals = await _muralService.ConsultarTodos(); 
        return View(murals); 
    }

    /// <summary>
    ///     Consultar a lista com todos os posts do mural.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os posts do mural
    /// 
    /// Use este endpoint se o seu objetivo é recuperar todos os posts armazenados no mural.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Mural/ConsultarTodosMurals
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    ///
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do post (gerado automaticamente pelo banco)
    /// - **idUsuario** (string): ID do usuário que criou o post
    /// - **titulo** (string): Título do post
    /// - **comentario** (string): Conteúdo/comentário do post
    /// - **tipoPost** (string): Tipo do post (Ex: "Texto", "Imagem", "Anúncio", etc.)
    /// - **data** (datetime): Data e hora de criação do post
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6e10",
    ///         "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///         "titulo": "Bem-vindos ao nosso mural!",
    ///         "comentario": "Este é o primeiro post do nosso mural comunitário.",
    ///         "tipoPost": "Texto",
    ///         "data": "2024-05-31T10:30:00Z"
    ///     },
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6e11",
    ///         "idUsuario": "6659fbbd3fae4c001fcf6d94",
    ///         "titulo": "Evento da comunidade",
    ///         "comentario": "Não percam o evento de sábado às 14h no centro comunitário!",
    ///         "tipoPost": "Anúncio",
    ///         "data": "2024-05-31T14:15:00Z"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    [HttpGet("ConsultarTodosMurals")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosMurals()
    {
        var murals = await _muralService.ConsultarTodos();
        return Ok(murals);
    }

    /// <summary>
    ///     Consultar um único post do mural.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único post pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar todos os dados de um post específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do post (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Mural/ConsultarMuralId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6e10",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "titulo": "Bem-vindos ao nosso mural!",
    ///     "comentario": "Este é o primeiro post do nosso mural comunitário.",
    ///     "tipoPost": "Texto",
    ///     "data": "2024-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    [HttpGet("ConsultarMuralId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarMuralId(string id)
    {
        var mural = await _muralService.ConsultarId(id);

        if (mural == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }

        return Ok(mural);
    }

    // View para atualizar um usuário
    [HttpGet("Atualizar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Atualizar()
    {
        //var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "IdMural")?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Error");
        }

        var mural = await _muralService.ConsultarId(userIdString);
        if (mural == null)
        {
            return NotFound();
        }

        return View(mural);
    }

    [HttpPost("Atualizar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Atualizar(Mural mural)
    {
        if (!ModelState.IsValid)
        {
            return View(mural);
        }

        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "IdMural")?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Error");
        }

        var muralExistente = await _muralService.ConsultarId(userIdString);

        if (muralExistente == null)
        {
            return NotFound();
        }

        muralExistente.IdUsuario = mural.IdUsuario;
        muralExistente.Titulo = mural.Titulo;
        muralExistente.Comentario = mural.Comentario;
        muralExistente.TipoPost = mural.TipoPost;
        muralExistente.Data = mural.Data;

        await _muralService.Atualizar(muralExistente);

        TempData["SuccessMessage"] = "Dados atualizados com sucesso!";
        //return RedirectToAction("MensagemAtualizacao");
        return View(muralExistente);
    }

    [HttpGet("MensagemAtualizacao")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult MensagemAtualizacao()
    {
        return View();
    }

    /// <summary>
    ///     Atualiza todos os dados do post com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um post no mural
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** de um post no mural.  
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    ///
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/Mural/AtualizarMural/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// ### Exemplo de requisição para atualizar os dados:
    ///
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6e10",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "titulo": "Título Atualizado",
    ///     "comentario": "Comentário completamente atualizado com novas informações.",
    ///     "tipoPost": "Texto",
    ///     "data": "2024-05-31T15:45:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    [HttpPut("AtualizarMural/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarMural(string id, [FromBody] Mural mural)
    {
        if (string.IsNullOrEmpty(id) || mural == null || id != mural.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var muralExistente = await _muralService.ConsultarId(id);

        if (muralExistente == null)
        {
            return NotFound();
        }

        muralExistente.IdUsuario = mural.IdUsuario;
        muralExistente.Titulo = mural.Titulo;
        muralExistente.Comentario = mural.Comentario;
        muralExistente.TipoPost = mural.TipoPost;
        muralExistente.Data = mural.Data;
        await _muralService.Atualizar(muralExistente);

        return Ok(muralExistente); 
    }

    [HttpGet("ConfirmarExcluir/{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ConfirmarExcluir(string id)
    {
        var mural = await _muralService.ConsultarId(id);
        
        if (mural == null)
        {
            return NotFound();
        }

        return View(mural);
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um post existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6e10">ID do post a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um Post
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do post,
    /// sem a necessidade de enviar todas as informações já cadastradas.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    ///
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Mural/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// ### Campos que podem ser atualizados:
    /// - **idUsuario** (string): ID do usuário responsável pelo post
    /// - **titulo** (string): Título do post
    /// - **comentario** (string): Conteúdo/comentário do post
    /// - **tipoPost** (string): Tipo do post
    /// - **data** (datetime): Data e hora do post
    ///
    /// ### Exemplo de requisição:
    /// ```json
    /// {
    ///     "titulo": "Novo título para o post",
    ///     "comentario": "Comentário atualizado apenas"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6e10",
    ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
    ///     "titulo": "Novo título para o post",
    ///     "comentario": "Comentário atualizado apenas",
    ///     "tipoPost": "Texto",
    ///     "data": "2024-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
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

        var muralAtualizado = await _muralService.AtualizarParcial(id, camposParaAtualizar);

        if (muralAtualizado == null)
        {
            return NotFound("Usuário não encontrada.");
        }

        return Ok(muralAtualizado);
    }

    [HttpPost("Excluir")]
    [ValidateAntiForgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Excluir(string id)
    {
        var mural = await _muralService.ConsultarId(id);
        
        if (mural != null)
        {
            // Exclui o usuário do banco de dados
            await _muralService.Excluir(id);

            // Desloga o usuário
            //await _context.SaveChangesAsync();
            await HttpContext.SignOutAsync();
            
            // Redireciona para a página de login ou para onde você preferir
            TempData["SuccessMessage"] = "Usuário excluído com sucesso.";
            return RedirectToAction("MensagemExclusao", "Mural"); 
        }

        TempData["ErrorMessage"] = "Usuário não encontrado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("MensagemExclusao")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult MensagemExclusao()
    {
        return View();
    }

    /// <summary>
    ///     Exclui um post do mural.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6e10">ID do post a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um post do mural
    /// 
    /// Use este endpoint para remover permanentemente um post do mural.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Mural/ExcluirMural/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Post excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    [HttpDelete("ExcluirMural/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirMural(string id)
    {
        var mural = await _muralService.ConsultarId(id);
        
        if (mural == null)
        {
            return NotFound();
        }

        await _muralService.Excluir(id);

        return Ok(new { message = "Usuário excluído com sucesso." });  
    }
}