using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/EnderecoAbrigo")] 
public class EnderecoAbrigoController : Controller
{
    private readonly IEnderecoAbrigoService _enderecoAbrigoService;
    private readonly ICepService _cepService;

    public EnderecoAbrigoController(IEnderecoAbrigoService enderecoAbrigoService, ICepService cepService)
    {
        _enderecoAbrigoService = enderecoAbrigoService;
        _cepService = cepService;
    }

    /// <summary>
    ///     Consulta o endereço completo a partir de um CEP informado.
    /// </summary>
    /// 
    /// <remarks>
    /// ## Buscar endereço via CEP
    /// 
    /// Use este endpoint para consultar um endereço completo a partir do CEP fornecido.  
    /// A resposta será preenchida automaticamente com base na base de dados do ViaCEP.
    ///
    /// ### Parâmetro obrigatório:
    /// - **cep** (string): Código de Endereçamento Postal (somente números)
    ///
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/EnderecoAbrigo/consultar-cep/{cep}
    /// ```
    ///
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "cep": "01001-000",
    ///     "estado": "SP",
    ///     "cidade": "São Paulo",
    ///     "bairro": "Sé",
    ///     "rua": "Praça da Sé"
    /// }
    /// ```
    ///
    /// </remarks>
    /// <param name="cep">CEP que deseja consultar (somente números)</param>
    /// <response code="200">Endereço encontrado com sucesso</response>
    /// <response code="404">CEP não encontrado</response>
    /// <response code="500">Erro interno ao buscar o endereço</response>
    [HttpGet("consultar-cep/{cep}")]
    public async Task<IActionResult> ConsultarCep(string cep)
    {
        var enderecoAbrigo = await _cepService.ConsultarCepAsync(cep);

        if (enderecoAbrigo == null)
            return NotFound("CEP não encontrado.");

        return Ok(enderecoAbrigo);
    }



    /// <summary>
    ///     Cadastra um novo endereço de preferência para o usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Cadastrar novo endereço de preferência para o usuário
    /// 
    /// Use este endPoint para cadastrar um endereço de peferência para o usuário. Com este endereço, o modelo de IA será capaz de buscar uma  clínica próxima do cliente.
    /// 
    /// ### Campos que devem ser utilizados para criar um novo dia:
    /// - **idUsuario** : IdAbrigo que deseja registrar o endereço
    /// - **cep** string : CEP de preferência e não de residência
    /// - **estado** string : Estado de preferência e não de residência
    /// - **cidade** string : Cidade de preferência e não de residência
    /// - **bairro** string : Bairro de preferência e não de residência
    /// - **rua** string : Rua de preferência e não de residência
    /// 
    /// ### Campos que não devem ser utilizados para criar um novo dia:
    /// - **id** : Id do dia que será gerado automaticamente
    ///
    /// ### Exemplo de requisição:
    /// ```http
    /// POST http://localhost:3001/api/EnderecoAbrigo/CadastrarEnderecoAbrigo
    /// ```
    /// 
    /// ```http
    /// Header: Key = Content-Type e Value = application/json
    /// ```
    ///
    /// ### Exemplo de body para requisição:
    ///
    /// ```json
    ///     {   
    ///         "idUsuario": "67cc95b32811515d372209ce",
    ///         "cep": "12345678",
    ///         "estado": "São Paulo",
    ///         "cidade": "São Paulo",
    ///         "bairro": "Centro",
    ///         "rua": "Cantori 30"
    ///     }
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// 
    /// ```json
    ///     {   
    ///         "id": "67cc95b32811515d37220000",
    ///         "idUsuario": "67cc95b32811515d372209ce",
    ///         "cep": "12345678",
    ///         "estado": "São Paulo",
    ///         "cidade": "São Paulo",
    ///         "bairro": "Centro",
    ///         "rua": "Cantori 30"
    ///     }
    /// ```
    /// 
    /// </remarks>
    /// <response code="201">Endereço criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="500">Erro interno</response>
    [HttpPost("CadastrarEnderecoAbrigo")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CadastrarEnderecoAbrigo([FromBody] EnderecoAbrigoDTO enderecoAbrigoDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(enderecoAbrigoDTO.IdAbrigo))
        return BadRequest("O campo IdAbrigo é obrigatório.");


        var enderecoAbrigo = new EnderecoAbrigo
        {
            CEP = enderecoAbrigoDTO.CEP ?? string.Empty,
            Estado = enderecoAbrigoDTO.Estado,
            Cidade = enderecoAbrigoDTO.Cidade,
            Bairro = enderecoAbrigoDTO.Bairro,
            Rua = enderecoAbrigoDTO.Rua,
            IdAbrigo = enderecoAbrigoDTO.IdAbrigo
        };

        await _enderecoAbrigoService.Criar(enderecoAbrigo);

        return CreatedAtAction(nameof(Consultar), new { id = enderecoAbrigo.Id }, enderecoAbrigo);
    }


    [HttpGet("Consultar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Consultar()
    {
        var enderecoAbrigos = await _enderecoAbrigoService.ConsultarTodos(); 
        return View(enderecoAbrigos); 
    }

    /// <summary>
    ///     Consultar o endereço de preferência do usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todos os registros de endereço do banco de dados
    /// 
    /// Use este endPoint se seu objetivo é recuperar todos os registros de endereços do banco de dados
    /// 
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/EnderecoAbrigo/ConsultarTodosEnderecoAbrigo
    /// ```
    ///
    /// ### Campos que disponíveis na requisição:
    /// - **id** : Id do banco de dados, que foi gerado automaticamente.
    /// - **idUsuario** : IdAbrigo que deseja registrar o endereço
    /// - **cep** string : CEP de preferência e não de residência
    /// - **estado** string : Estado de preferência e não de residência
    /// - **cidade** string : Cidade de preferência e não de residência
    /// - **bairro** string : Bairro de preferência e não de residência
    /// - **rua** string : Rua de preferência e não de residência
    /// 
    /// ### Exemplo de body que virá de resposta:
    /// 
    /// ```json
    ///[
    ///    {
    ///        "id": "67cdee51b304fd2aaac177c9",
    ///        "idUsuario": "67cc95b32811515d372209ce",
    ///        "cep": "01739018",
    ///        "estado": "São Paulo",
    ///        "cidade": "São Paulo",
    ///        "bairro": "Vila Andrade",
    ///        "rua": "Cantori 30"
    ///    },
    ///    {
    ///        "id": "67cdee91b304fd2aaac177ca",
    ///        "idUsuario": "67cc95b32811515d372209ce",
    ///        "cep": "05728020",
    ///        "estado": "São Paulo",
    ///        "cidade": "São Paulo",
    ///        "bairro": "Teste",
    ///        "rua": "Teste"
    ///    }
    ///]
    /// ```
    /// </remarks>
    /// 
    /// <response code="200">EnderecoAbrigo consultado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarTodosEnderecoAbrigo")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarTodosEnderecoAbrigo()
    {
        var enderecoAbrigos = await _enderecoAbrigoService.ConsultarTodos(); 
        return Ok(enderecoAbrigos);
    }

    /// <summary>
    ///     Consultar um único registro de preferência de endereço que o usuário cadastrou.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar um único endereço de preferência registrado, sendo consultado pelo ID do banco de dados
    /// 
    /// Use este endpoint quando precisar consultar somente um registro com todos campos específicos.
    /// 
    /// ### Campos que devem ser utilizados para consultar um endereço de preferência:
    /// 
    /// - **id**: ID do banco e não o idUsuario
    ///
    /// ### Exemplo de requisição:
    /// ```http
    /// GET http://localhost:3001/api/EnderecoAbrigo/ConsultarEnderecoAbrigoId/{id}
    /// ```
    ///  
    /// ```json
    /// "id de teste": "68379ecf2843b017d24f581c"
    /// ```
    /// 
    /// ### Exemplo de body que receberemos como resposta:
    /// 
    /// ```json
    ///    {
    ///         "id": "67cdee91b304fd2aaac177ca",
    ///         "idUsuario": "67cc95b32811515d372209ce",
    ///         "cep": "05728020",
    ///         "estado": "São Paulo",
    ///         "cidade": "São Paulo",
    ///         "bairro": "Teste",
    ///         "rua": "Teste"
    ///     }
    /// ```
    /// </remarks>
    /// 
    /// <response code="200">Dia consultado com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarEnderecoAbrigoId/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarEnderecoAbrigoId(string id)
    {
        var enderecoAbrigo = await _enderecoAbrigoService.ConsultarId(id);

        if (enderecoAbrigo == null)
        {
            return NotFound(new { message = "Endereço não encontrado." });
        }

        return Ok(enderecoAbrigo);
    }


    /// <summary>
    ///     Atualiza o endereço completo existente do usuário, com base no ID do usuário e ID do banco de dados.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Atualizar a preferência de endereço cadastrado pelo usuário
    /// 
    /// Use este endpoint se o objetivo for atualizar todos os campos do endereço no cadastro. Se for parcial, utilize outro endPoint.
    /// 
    /// ### Exemplo de requisição
    ///
    /// ```http
    /// PUT http://localhost:3001/api/EnderecoAbrigo/AtualizarEnderecoAbrigo/{id}
    /// ```
    /// 
    /// ```json
    ///     {   
    ///         "id": "67ce4b3d61760e36f862dd59",
    ///         "idUsuario": "67cc95b32811515d372209ce",
    ///         "cep": "05763290",
    ///         "estado": "SP", 
    ///         "cidade": "São Paulo",
    ///         "Bairro": "Vila Madalena",
    ///         "Rua": "Rua do Pão de Queijo"
    ///     }
    /// 
    /// ``` 
    /// 
    /// ### Se preencher alguns campos e outros não, estes campos serão preenchidos com dados inválidos e default criado pelo sistema.
    /// </remarks>
    /// 
    /// 
    /// <param name="id" type="string" example="67cc95b32811515d372209ce">ID do usuário a ser atualizado o endereço.</param>
    /// <param name="enderecoAbrigoDTO">Dados do endereço a serem atualizados.</param>
    /// <response code="200">Endereço atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Endereço não autorizado</response>
    /// <response code="404">Endereço não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("AtualizarEnderecoAbrigo/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarEnderecoAbrigo(string id, [FromBody] EnderecoAbrigoDTO enderecoAbrigoDTO)
    {
        if (string.IsNullOrEmpty(id) || enderecoAbrigoDTO == null || id != enderecoAbrigoDTO.Id)
        {
            return BadRequest("Id do Clinica não corresponde ao fornecido.");
        }

        var enderecoAbrigoExistente = await _enderecoAbrigoService.ConsultarId(id);

        if (enderecoAbrigoExistente == null)
        {
            return NotFound();
        }

        enderecoAbrigoExistente.CEP = enderecoAbrigoDTO.CEP ?? string.Empty;
        enderecoAbrigoExistente.Estado = enderecoAbrigoDTO.Estado;
        enderecoAbrigoExistente.Cidade = enderecoAbrigoDTO.Cidade;
        enderecoAbrigoExistente.Bairro = enderecoAbrigoDTO.Bairro;
        enderecoAbrigoExistente.Rua = enderecoAbrigoDTO.Rua;

        await _enderecoAbrigoService.Atualizar(enderecoAbrigoExistente);

        return Ok(enderecoAbrigoExistente);
    }

    /// <summary>
    ///     Atualiza parcialmente os dados de um endereço existente
    /// </summary>
    /// 
    /// <param name="id" type="string" example="67ce4b3d61760e36f862dd59">ID da endereço a ser atualizada</param>
    /// <param name="camposParaAtualizar">Campos específicos a serem atualizados</param>
    /// 
    /// <remarks>
    /// 
    /// ## Atualização Parcial dos dados
    /// 
    /// Use este endpoint quando precisar atualizar apenas alguns campos específicos de uma tabela,
    /// sem a necessidade de enviar todos os dados.
    /// 
    /// ### Campos que podem ser atualizados:
    /// - **id**: ID gerado pelo banco de dados
    /// - **cep**: CEP do endereço
    /// - **Estado**: Estado do endereço
    /// - **Cidade**: Cidade do endereço de preferência do usuário
    /// - **Bairro**: Bairro do endereço
    /// - **Rua**: Endereço completo com número
    /// 
    /// ### Campos que não podem ser atualizados:
    /// - **IdAbrigo**: IdAbrigo que foi gerado pelo banco de dados
    /// 
    /// ```http
    /// PATCH http://localhost:3001/api/EnderecoAbrigo/AtualizarParcial/{id}
    /// ```
    ///
    /// ### Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "id": "67ce4b3d61760e36f862dd59",
    ///     "email": "novo.email@clinica.com.br"
    /// }
    /// ```
    /// 
    /// Somente os campos incluídos no corpo da requisição serão atualizados.
    /// 
    /// ### Exemplo de resposta da requisição:
    /// 
    /// ```json
    /// {
    ///     "id": "67cdee91b304fd2aaac177ca",
    ///     "idUsuario": "67cc95b32811515d372209ce",
    ///     "cep": "05763290",
    ///     "estado": "São Paulo",
    ///     "cidade": "São Paulo",
    ///     "bairro": "Teste",
    ///     "rua": "Teste"
    /// }
    /// ```
    /// </remarks>
    /// 
    /// <response code="200">Clinica atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Clinica não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPatch("AtualizarParcial/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarClinica(string id, [FromBody] Dictionary<string, object> camposParaAtualizar)
    {
        if (string.IsNullOrEmpty(id) || camposParaAtualizar == null || !camposParaAtualizar.Any())
        {
            return BadRequest("Id ou campos para atualização são necessários.");
        }

        var clinicaAtualizada = await _enderecoAbrigoService.AtualizarParcial(id, camposParaAtualizar);

        if (clinicaAtualizada == null)
        {
            return NotFound("Clinica não encontrada.");
        }

        return Ok(clinicaAtualizada);
    }


    [HttpPost("Excluir")]
    [ValidateAntiForgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Excluir(string id)
    {
        var enderecoAbrigo = await _enderecoAbrigoService.ConsultarPorUsuarioId(id);
        
        if (enderecoAbrigo != null)
        {
            await _enderecoAbrigoService.Excluir(id);
            
            TempData["SuccessMessage"] = "EnderecoAbrigo excluído com sucesso.";
            //return RedirectToAction("MensagemExclusao", "EnderecoAbrigo"); 
        }

        TempData["ErrorMessage"] = "EnderecoAbrigo não encontrado.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Exclui um endereço do usuário.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Excluir um endereço de preferência do usuário cadastrado
    /// 
    /// Use este endoPoint se seu objetivo é excluir um cadastro contendo o endereço de preferência cadastrado errado. 
    /// 
    /// ### Exemplo da requisição para excluir:
    ///
    /// ```http
    /// DELETE http://localhost:3001/api/EnderecoAbrigo/ExcluirEnderecoAbrigo/{id}
    /// ```
    /// 
    /// ```json
    ///     {
    ///         "id": "67cf3f8f8d3a256253f2dab5",
    ///     }
    /// ```
    /// 
    /// ### Exemplo da resposta para excluir uma clínica:
    /// 
    /// ```json
    ///     {
    ///         "message": "Endereço de preferência excluído com sucesso."
    ///     }
    /// ``` 
    /// 
    /// Uma vez excluida da base, não tem reversão desta ação.
    /// 
    /// </remarks>
    /// 
    /// <param name="id" type="string" example="67cdee51b304fd2aaac177c9">ID do endereço a ser excluído.</param>
    /// <response code="200">Endereço excluído com sucesso</response>
    /// <response code="401">Usuário não autorizado</response>
    /// <response code="404">Endereço não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("ExcluirEnderecoAbrigo/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExcluirEnderecoAbrigo(string id)
    {
        if (string.IsNullOrEmpty(id))
        return BadRequest("ID do endereço não pode ser vazio.");

        var enderecoAbrigo = await _enderecoAbrigoService.ConsultarId(id);
        if (enderecoAbrigo == null)
            return NotFound("Endereço não encontrado.");

        await _enderecoAbrigoService.Excluir(id);
        return Ok(new { message = "Endereço excluído com sucesso." });
    }

}