using Project.Infrastructure.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/ML")]
public class MLController : Controller
{
    private readonly IMLService _mlService;

    public MLController(IMLService mlService)
    {
        _mlService = mlService;
    }

    /// <summary>
    ///     Treina o modelo de Machine Learning com dados históricos.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Treinar modelo de predição de necessidades por região
    /// 
    /// Use este endpoint para treinar o modelo de Machine Learning com base nos dados históricos de eventos, abrigos, doações e distribuições armazenados no banco de dados.
    /// Se não houver dados suficientes no banco, o sistema automaticamente gerará 1000 registros sintéticos para treinamento.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/ML/TreinarModelo
    /// ```
    /// 
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### O que acontece no treinamento:
    /// - O sistema coleta dados históricos de todas as tabelas (eventos, abrigos, doações, distribuições)
    /// - Se dados insuficientes, gera automaticamente dados sintéticos realistas
    /// - Treina 3 modelos separados para prever: alimentos, medicamentos e cobertores
    /// - Atualiza os modelos em memória para uso nas predições
    /// 
    /// ### Exemplo de resposta ao treinar com sucesso:
    /// ```json
    /// {
    ///     "message": "Modelo treinado com sucesso!",
    ///     "timestamp": "2025-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// ### Exemplo de resposta em caso de erro:
    /// ```json
    /// {
    ///     "message": "Não há dados suficientes para treinamento.",
    ///     "timestamp": "2025-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Modelo treinado com sucesso</response>
    /// <response code="400">Não há dados suficientes para treinamento</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("TreinarModelo")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TreinarModelo()
    {
        var sucesso = await _mlService.TreinarModelo();
        
        if (sucesso)
        {
            return Ok(new { message = "Modelo treinado com sucesso!", timestamp = DateTime.Now });
        }
        
        return BadRequest(new { message = "Não há dados suficientes para treinamento.", timestamp = DateTime.Now });
    }

    /// <summary>
    ///     Treina o modelo de Machine Learning com dados sintéticos.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Treinar modelo com dados sintéticos (1000 registros)
    /// 
    /// Use este endpoint para forçar o treinamento do modelo com dados sintéticos, ignorando os dados do banco.
    /// Útil para testes ou quando há problemas com os dados históricos.
    /// 
    /// Requisição via rota:
    /// ```http
    /// POST http://localhost:3001/api/ML/TreinarComDadosSinteticos
    /// ```
    /// 
    /// ### O que acontece:
    /// - Gera 1000 registros sintéticos com correlações realistas
    /// - Treina 3 modelos separados para prever: alimentos, medicamentos e cobertores
    /// - Dados incluem sazonalidade e variações por região
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Modelo treinado com dados sintéticos!",
    ///     "registros": 1000,
    ///     "timestamp": "2025-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Modelo treinado com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("TreinarComDadosSinteticos")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TreinarComDadosSinteticos()
    {
        try
        {
            var sucesso = await _mlService.TreinarModeloComCSV(); // Sem CSV = dados sintéticos
            
            if (sucesso)
            {
                return Ok(new { 
                    message = "Modelo treinado com dados sintéticos!", 
                    registros = 1000,
                    timestamp = DateTime.Now 
                });
            }
            
            return StatusCode(500, new { message = "Erro ao treinar modelo.", timestamp = DateTime.Now });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, timestamp = DateTime.Now });
        }
    }

    /// <summary>
    ///     Treina o modelo de Machine Learning com upload de arquivo CSV.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Treinar modelo com arquivo CSV customizado
    /// 
    /// Use este endpoint para treinar o modelo enviando um arquivo CSV com dados de treinamento.
    /// 
    /// ### Formato do CSV esperado:
    /// ```csv
    /// Localizacao,QuantidadeEventos,CapacidadeAbrigo,OcupacaoAtual,MesAno,AlimentosNecessarios,MedicamentosNecessarios,CobertoresNecessarios
    /// São Paulo,5,150,120,1,75,25,15
    /// Rio de Janeiro,3,200,180,1,54,18,9
    /// ```
    /// 
    /// ### Requisição via formulário:
    /// ```http
    /// POST http://localhost:3001/api/ML/TreinarComCSV
    /// Content-Type: multipart/form-data
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "message": "Modelo treinado com CSV!",
    ///     "registros": 500,
    ///     "timestamp": "2025-05-31T10:30:00Z"
    /// }
    /// ```
    /// 
    /// </remarks>
    /// 
    /// <param name="arquivo">Arquivo CSV com dados de treinamento</param>
    /// 
    /// <response code="200">Modelo treinado com sucesso</response>
    /// <response code="400">Arquivo inválido ou formato incorreto</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("TreinarComCSV")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TreinarComCSV(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest(new { message = "Arquivo CSV é obrigatório.", timestamp = DateTime.Now });
        }

        if (!arquivo.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new { message = "Apenas arquivos CSV são aceitos.", timestamp = DateTime.Now });
        }

        try
        {
            // Salvar arquivo temporariamente
            var caminhoTemp = Path.GetTempFileName();
            using (var stream = new FileStream(caminhoTemp, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Treinar com o CSV
            var sucesso = await _mlService.TreinarModeloComCSV(caminhoTemp);

            // Limpar arquivo temporário
            if (System.IO.File.Exists(caminhoTemp))
            {
                System.IO.File.Delete(caminhoTemp);
            }

            if (sucesso)
            {
                return Ok(new { 
                    message = "Modelo treinado com CSV!", 
                    arquivo = arquivo.FileName,
                    timestamp = DateTime.Now 
                });
            }
            
            return StatusCode(500, new { message = "Erro ao treinar modelo com CSV.", timestamp = DateTime.Now });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, timestamp = DateTime.Now });
        }
    }

    /// <summary>
    ///     Faz uma predição de necessidades de recursos para uma região específica.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Prever necessidades de recursos por região
    /// 
    /// Use este endpoint para obter uma predição de quantos alimentos, medicamentos e cobertores serão necessários para uma região específica com base nos parâmetros informados.
    /// 
    /// ### Parâmetros de consulta necessários:
    /// - **localizacao** (string): Nome da região/localização (Ex: "São Paulo", "Rio de Janeiro")
    /// - **quantidadeEventos** (int): Número de eventos esperados ou recentes na região
    /// - **capacidadeAbrigo** (int, opcional): Capacidade total do abrigo da região (padrão: 100)
    /// - **ocupacaoAtual** (int, opcional): Ocupação atual do abrigo (padrão: 50)
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/ML/Prever?localizacao=São Paulo&quantidadeEventos=5&capacidadeAbrigo=200&ocupacaoAtual=150
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// {
    ///     "alimentosPrevistos": 25,
    ///     "medicamentosPrevistos": 8,
    ///     "cobertoresPrevistos": 12
    /// }
    /// ```
    /// 
    /// ### Como interpretar a resposta:
    /// - **alimentosPrevistos**: Quantidade estimada de unidades de alimentos necessárias
    /// - **medicamentosPrevistos**: Quantidade estimada de unidades de medicamentos necessárias
    /// - **cobertoresPrevistos**: Quantidade estimada de cobertores/agasalhos necessários
    /// 
    /// </remarks>
    /// 
    /// <param name="localizacao">Nome da região/localização para fazer a predição</param>
    /// <param name="quantidadeEventos">Número de eventos esperados ou recentes na região</param>
    /// <param name="capacidadeAbrigo">Capacidade total do abrigo da região (opcional, padrão: 100)</param>
    /// <param name="ocupacaoAtual">Ocupação atual do abrigo (opcional, padrão: 50)</param>
    /// 
    /// <response code="200">Predição realizada com sucesso</response>
    /// <response code="400">Parâmetros inválidos fornecidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("Prever")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Prever(
        [FromQuery] string localizacao,
        [FromQuery] int quantidadeEventos,
        [FromQuery] int capacidadeAbrigo = 100,
        [FromQuery] int ocupacaoAtual = 50)
    {
        if (string.IsNullOrEmpty(localizacao) || quantidadeEventos < 0)
        {
            return BadRequest(new { message = "Localização é obrigatória e quantidade de eventos deve ser maior ou igual a zero." });
        }

        var predicao = await _mlService.Prever(localizacao, quantidadeEventos, capacidadeAbrigo, ocupacaoAtual);
        
        return Ok(predicao);
    }

    /// <summary>
    ///     Consulta todas as localizações disponíveis para predição.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// ## Consultar todas as localizações com dados disponíveis
    /// 
    /// Use este endpoint para recuperar uma lista de todas as regiões/localizações que possuem dados históricos no sistema e podem ser usadas para fazer predições.
    /// 
    /// ### Exemplo de requisição:
    /// 
    /// Requisição via rota:
    /// ```http
    /// GET http://localhost:3001/api/ML/ConsultarLocalizacoes
    /// ```
    /// ```http
    /// Content-Type: application/json
    /// ```
    /// 
    /// ### Exemplo de resposta:
    /// ```json
    /// [
    ///     "São Paulo",
    ///     "Rio de Janeiro", 
    ///     "Belo Horizonte",
    ///     "Salvador",
    ///     "Curitiba"
    /// ]
    /// ```
    /// 
    /// ### Como usar a resposta:
    /// - Use qualquer uma dessas localizações no endpoint de predição
    /// - Localizações que não estão na lista podem não ter dados suficientes para predições precisas
    /// 
    /// </remarks>
    /// 
    /// <response code="200">Lista de localizações retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("ConsultarLocalizacoes")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultarLocalizacoes()
    {
        var localizacoes = await _mlService.ObterLocalizacoesDisponiveis();
        return Ok(localizacoes);
    }
}