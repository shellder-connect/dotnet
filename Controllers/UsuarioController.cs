using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/Usuario")]
public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
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
    public async Task<IActionResult> Criar([Bind("Id,Nome,Email,Senha,IdTipoUsuario,Telefone,IdEndereco,DataNascimento,Documento,Status")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            await _usuarioService.Criar(usuario);
            TempData["SuccessMessage"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Mensagem");
        }
        return View(usuario);
    }

    /// <summary>
    ///     Cria um novo usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo usuário no banco de dados
    /// 
    /// Use este endpoint para cadastrar um usuário. Somente usuários cadastrados podem efetuar login na plataforma.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/Usuario/CadastrarUsuario
    /// ```
    ///
    /// ```http
    /// Content-Type: application/json
    /// ```
    ///
    /// ### Campos que devem ser utilizados para criar um novo usuário:
    /// - **nome** (string): Nome completo do usuário
    /// - **email** (string): Email principal de contato
    /// - **senha** (string): Senha de acesso (deve ser armazenada com hash)
    /// - **idTipoUsuario** (string): Tipo de usuário (Ex: "Comum", "Administrador", etc.)
    /// - **telefone** (string): Telefone para contato
    /// - **idEndereco** (string): ID de referência do endereço (relacionamento com outra coleção)
    /// - **dataNascimento** (string): Data de nascimento no formato ISO (Ex: "1990-05-28")
    /// - **documento** (string): Documento oficial como CPF ou RG
    /// - **status** (string): Status da conta (Ex: "Ativo", "Inativo")
    /// 
    /// ### Campos que não devem ser enviados:
    /// - **id**: Gerado automaticamente pelo banco de dados
    /// 
    /// ### Exemplo de body para requisição:
    /// ```json
    /// {
    ///     "nome": "João da Silva",
    ///     "email": "joao@exemplo.com",
    ///     "senha": "senhaSegura123",
    ///     "idTipoUsuario": "Comum",
    ///     "telefone": "11999998888",
    ///     "idEndereco": "6659d12f3fae4c001fcf6d92",
    ///     "dataNascimento": "1990-05-28",
    ///     "documento": "12345678910",
    ///     "status": "Ativo"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta ao cadastrar com sucesso:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "nome": "João da Silva",
    ///     "email": "joao@exemplo.com",
    ///     "senha": "senhaHash",
    ///     "idTipoUsuario": "Comum",
    ///     "telefone": "11999998888",
    ///     "idEndereco": "6659d12f3fae4c001fcf6d92",
    ///     "dataNascimento": "1990-05-28",
    ///     "documento": "12345678910",
    ///     "status": "Ativo"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("CadastrarUsuario")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarUsuario([FromBody] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            await _usuarioService.Criar(usuario);
            return CreatedAtAction(nameof(ConsultarTodosUsuarios), new { id = usuario.Id }, usuario); 
        }
        return BadRequest(ModelState); 
    }


    // Rota de View
    [HttpGet("Consultar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Consultar()
    {
        var usuarios = await _usuarioService.ConsultarTodos(); 
        return View(usuarios); 
    }

    /// <summary>
    ///     Consultar a lista com todos os usuários.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de usuários do banco de dados
    /// 
    /// Use este endpoint se o seu objetivo é recuperar todos os registros de usuários armazenados no banco de dados.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Usuario/ConsultarTodosUsuarios
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    ///
    /// ### Campos disponíveis na resposta:
    /// - **id** (string): Identificador único do usuário (gerado automaticamente pelo banco)
    /// - **nome** (string): Nome completo do usuário
    /// - **email** (string): Email principal de contato
    /// - **senha** (string): Senha de acesso (deverá estar armazenada com hash)
    /// - **idTipoUsuario** (string): Tipo de usuário (Ex: "Comum", "Administrador", etc.)
    /// - **telefone** (string): Telefone de contato
    /// - **idEndereco** (string): ID de referência ao endereço do usuário
    /// - **dataNascimento** (string): Data de nascimento no formato ISO (Ex: "1990-05-28")
    /// - **documento** (string): Documento oficial como CPF ou RG
    /// - **status** (string): Status da conta do usuário (Ex: "Ativo", "Inativo")
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6d93",
    ///         "nome": "João da Silva",
    ///         "email": "joao@exemplo.com",
    ///         "senha": "senhaHash",
    ///         "idTipoUsuario": "Comum",
    ///         "telefone": "11999998888",
    ///         "idEndereco": "6659d12f3fae4c001fcf6d92",
    ///         "dataNascimento": "1990-05-28",
    ///         "documento": "12345678910",
    ///         "status": "Ativo"
    ///     },
    ///     {
    ///         "id": "6659fbbd3fae4c001fcf6e00",
    ///         "nome": "Maria Oliveira",
    ///         "email": "maria@exemplo.com",
    ///         "senha": "outraSenhaHash",
    ///         "idTipoUsuario": "Administrador",
    ///         "telefone": "11988887777",
    ///         "idEndereco": "6659d13a3fae4c001fcf6d95",
    ///         "dataNascimento": "1985-11-12",
    ///         "documento": "98765432100",
    ///         "status": "Ativo"
    ///     }
    /// ]
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    /// <response code="400">Requisição malformada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosUsuarios")]
    [Produces("application/json")]
    public async Task<IActionResult> ConsultarTodosUsuarios()
    {
        var usuarios = await _usuarioService.ConsultarTodos();
        return Ok(usuarios);
    }

    /// <summary>
    ///     Consultar um único registro de usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único usuário pelo ID
    /// 
    /// Use este endpoint quando precisar recuperar todos os dados de um usuário específico, informando o ID armazenado no banco de dados.
    /// 
    /// ### Parâmetro necessário:
    /// - **id** (string): ID do usuário (gerado automaticamente pelo MongoDB)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/Usuario/ConsultarUsuarioId/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "nome": "João da Silva",
    ///     "email": "joao@exemplo.com",
    ///     "senha": "senhaHash",
    ///     "idTipoUsuario": "Comum",
    ///     "telefone": "11999998888",
    ///     "idEndereco": "6659d12f3fae4c001fcf6d92",
    ///     "dataNascimento": "1990-05-28",
    ///     "documento": "12345678910",
    ///     "status": "Ativo"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Usuário consultado com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarUsuarioId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarUsuarioId(string id)
    {
        var usuario = await _usuarioService.ConsultarId(id);

        if (usuario == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }

        return Ok(usuario);
    }

    // View para atualizar um usuário
    [HttpGet("Atualizar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Atualizar()
    {
        //var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Error");
        }

        var usuario = await _usuarioService.ConsultarId(userIdString);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost("Atualizar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Atualizar(Usuario usuario)
    {
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Error");
        }

        var usuarioExistente = await _usuarioService.ConsultarId(userIdString);

        if (usuarioExistente == null)
        {
            return NotFound();
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Senha = usuario.Senha;
        usuarioExistente.IdTipoUsuario = usuario.IdTipoUsuario;
        usuarioExistente.Telefone = usuario.Telefone;
        usuarioExistente.IdEndereco = usuario.IdEndereco;
        usuarioExistente.DataNascimento = usuario.DataNascimento;
        usuarioExistente.Documento = usuario.Documento;
        usuarioExistente.Status = usuario.Status;

        await _usuarioService.Atualizar(usuarioExistente);

        TempData["SuccessMessage"] = "Dados atualizados com sucesso!";
        //return RedirectToAction("MensagemAtualizacao");
        return View(usuarioExistente);
    }

    [HttpGet("MensagemAtualizacao")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult MensagemAtualizacao()
    {
        return View();
    }

    /// <summary>
    ///     Atualiza todos os dados do usuário com base no ID.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar todas as informações de um usuário no banco de dados
    /// 
    /// Use este endpoint para sobrescrever **todos os campos** do cadastro de um usuário.  
    /// ⚠️ Se for necessário atualizar apenas alguns campos, utilize o endpoint de **atualização parcial (PATCH)**.
    ///
    /// ### Todos os campos devem ser preenchidos:
    /// - Campos não enviados serão sobrescritos com valores nulos ou padrão.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PUT http://localhost:3001/api/Usuario/AtualizarUsuario/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// ### Exemplo de requisição para atualizar os dados:
    ///
    /// ```json
    /// {
    ///     "id": "6659fbbd3fae4c001fcf6d93",
    ///     "descricao": "Adm",
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do usuário no banco de dados.</param>
    /// <param name="usuario">Objeto contendo os dados completos a serem atualizados.</param>
    /// 
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarUsuario/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarUsuario(string id, [FromBody] Usuario usuario)
    {
        if (string.IsNullOrEmpty(id) || usuario == null || id != usuario.Id)
        {
            return BadRequest("Id não corresponde ao fornecido.");
        }

        var usuarioExistente = await _usuarioService.ConsultarId(id);

        if (usuarioExistente == null)
        {
            return NotFound();
        }


        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Senha = usuario.Senha;
        usuarioExistente.IdTipoUsuario = usuario.IdTipoUsuario;
        usuarioExistente.Telefone = usuario.Telefone;
        usuarioExistente.IdEndereco = usuario.IdEndereco;
        usuarioExistente.DataNascimento = usuario.DataNascimento;
        usuarioExistente.Documento = usuario.Documento;
        usuarioExistente.Status = usuario.Status;

        await _usuarioService.Atualizar(usuarioExistente);

        return Ok(usuarioExistente); 
    }

    [HttpGet("ConfirmarExcluir/{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ConfirmarExcluir(string id)
    {
        var usuario = await _usuarioService.ConsultarId(id);
        
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um usuário existente.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do usuário a ser atualizado.</param>
    /// <param name="camposParaAtualizar">Objeto contendo os campos que devem ser atualizados.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial de um Usuário
    /// 
    /// Use este endpoint quando for necessário atualizar apenas **alguns campos** do usuário,
    /// sem a necessidade de enviar todas as informações já cadastradas.
    /// Somente os campos incluídos no corpo da requisição serão modificados.
    ///
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// PATCH http://localhost:3001/api/Usuario/AtualizarParcial/{id}
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// ### Campos que podem ser atualizados:
    /// - **nome** (string): Nome do usuário
    /// - **email** (string): Email de contato
    /// - **senha** (string): Nova senha (deve ser criptografada)
    /// - **telefone** (string): Número de telefone
    /// - **idEndereco** (string): ID de endereço
    /// - **dataNascimento** (string): Data de nascimento no formato ISO
    /// - **documento** (string): Documento como CPF/RG
    /// - **status** (string): Status da conta (ex: "Ativo", "Inativo")
    ///
    /// ⚠️ Campos que **não podem ser atualizados** por este endpoint:
    /// - **idTipoUsuario**: O tipo de usuário é definido no momento do cadastro e não pode ser alterado aqui.
    ///
    /// ### Exemplo de requisição:
    /// ```json
    /// {
    ///     "IdUsuario": "68378e923f38476b3138e927",
    ///     "telefone": "11999998888"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "id": "68378e923f38476b3138e927",
    ///     "nome": "João da Silva",
    ///     "email": "novo.email@delfos.com.br",
    ///     "senha": "senhaHash",
    ///     "idTipoUsuario": "Comum",
    ///     "telefone": "11999998888",
    ///     "idEndereco": "6659d12f3fae4c001fcf6d92",
    ///     "dataNascimento": "1990-05-28",
    ///     "documento": "12345678910",
    ///     "status": "Ativo"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Usuário não encontrado</response>
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

        var usuarioAtualizado = await _usuarioService.AtualizarParcial(id, camposParaAtualizar);

        if (usuarioAtualizado == null)
        {
            return NotFound("Usuário não encontrada.");
        }

        return Ok(usuarioAtualizado);
    }

    [HttpPost("Excluir")]
    [ValidateAntiForgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Excluir(string id)
    {
        var usuario = await _usuarioService.ConsultarId(id);
        
        if (usuario != null)
        {
            // Exclui o usuário do banco de dados
            await _usuarioService.Excluir(id);

            // Desloga o usuário
            //await _context.SaveChangesAsync();
            await HttpContext.SignOutAsync();
            
            // Redireciona para a página de login ou para onde você preferir
            TempData["SuccessMessage"] = "Usuário excluído com sucesso.";
            return RedirectToAction("MensagemExclusao", "Usuario"); 
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
    ///     Exclui um usuário do banco de dados.
    /// </summary>
    /// 
    /// <param name="id" type="string" example="6659fbbd3fae4c001fcf6d93">ID do usuário a ser excluído.</param>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um usuário do banco de dados
    /// 
    /// Use este endpoint para remover permanentemente um usuário da base de dados.
    /// ⚠️ **A exclusão é irreversível.**
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// DELETE http://localhost:3001/api/Usuario/ExcluirUsuario/{id}
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Usuário excluído com sucesso."
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Usuário excluído com sucesso</response>
    /// <response code="400">ID inválido fornecido</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirUsuario/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> ExcluirUsuario(string id)
    {
        var usuario = await _usuarioService.ConsultarId(id);
        
        if (usuario == null)
        {
            return NotFound();
        }

        await _usuarioService.Excluir(id);

        return Ok(new { message = "Usuário excluído com sucesso." });  
    }
}