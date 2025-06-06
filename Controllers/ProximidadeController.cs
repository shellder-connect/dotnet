using Microsoft.AspNetCore.Mvc;
using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProximidadeController : ControllerBase
    {
        private readonly IProximityService _proximityService;
        private readonly ILogger<ProximidadeController> _logger;

        public ProximidadeController(IProximityService proximityService, ILogger<ProximidadeController> logger)
        {
            _proximityService = proximityService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os usuários com seus endereços e coordenadas
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Listar usuários com informações de localização
        /// 
        /// Este endpoint retorna uma lista completa de todos os usuários cadastrados no sistema, incluindo seus endereços completos e coordenadas geográficas (latitude e longitude).
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/usuarios
        /// ```
        ///
        /// ### Funcionalidades:
        /// - Busca todos os usuários do sistema
        /// - Inclui endereços completos formatados
        /// - Mostra coordenadas geográficas (se disponíveis)
        /// - Indica status da geocodificação
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
        ///     "nomeUsuario": "João da Silva",
        ///     "email": "joao@exemplo.com",
        ///     "telefone": "11999998888",
        ///     "cep": "01001000",
        ///     "enderecoCompleto": "Rua das Flores, Centro, São Paulo, SP, Brasil",
        ///     "latitude": -23.5505199,
        ///     "longitude": -46.6333094,
        ///     "status": "Ativo"
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Status possíveis:
        /// - **Ativo**: Coordenadas obtidas com sucesso
        /// - **Erro**: Falha na geocodificação do endereço
        /// - **Sem Endereço**: Usuário não possui endereço cadastrado
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Lista de usuários retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("usuarios")]
        public async Task<ActionResult<List<UsuarioComEnderecoDto>>> GetUsuariosComEnderecos()
        {
            try
            {
                _logger.LogInformation("Buscando usuários com endereços");
                var usuarios = await _proximityService.ProcessarUsuariosComEnderecosAsync();
                
                _logger.LogInformation($"Encontrados {usuarios.Count} usuários");
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários com endereços");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar usuários", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém todos os abrigos com seus endereços e coordenadas
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Listar abrigos com informações de localização e capacidade
        /// 
        /// Este endpoint retorna uma lista completa de todos os abrigos cadastrados no sistema, incluindo endereços, coordenadas geográficas, capacidade e ocupação atual.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/abrigos
        /// ```
        ///
        /// ### Funcionalidades:
        /// - Lista todos os abrigos disponíveis
        /// - Mostra capacidade total e ocupação atual
        /// - Inclui coordenadas para cálculos de proximidade
        /// - Status de geocodificação
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "idAbrigo": "683b055a4d65892ae33c3b1e",
        ///     "cep": "01310100",
        ///     "enderecoCompleto": "Avenida Paulista, 1000, Bela Vista, São Paulo, SP, Brasil",
        ///     "latitude": -23.5618,
        ///     "longitude": -46.6565,
        ///     "capacidadeTotal": 50,
        ///     "ocupacaoAtual": 35,
        ///     "descricao": "Abrigo central para famílias em situação de rua",
        ///     "status": "Ativo"
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Informações importantes:
        /// - **capacidadeTotal**: Número máximo de pessoas que o abrigo comporta
        /// - **ocupacaoAtual**: Número atual de pessoas no abrigo
        /// - **status**: Indica se as coordenadas foram obtidas com sucesso
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Lista de abrigos retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("abrigos")]
        public async Task<ActionResult<List<AbrigoComEnderecoDto>>> GetAbrigosComEnderecos()
        {
            try
            {
                _logger.LogInformation("Buscando abrigos com endereços");
                var abrigos = await _proximityService.ProcessarAbrigosComEnderecosAsync();
                
                _logger.LogInformation($"Encontrados {abrigos.Count} abrigos");
                return Ok(abrigos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar abrigos com endereços");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar abrigos", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Calcula e salva todas as proximidades entre usuários e abrigos
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Processar análise de proximidade completa
        /// 
        /// Este endpoint executa o cálculo completo de proximidade entre todos os usuários e abrigos do sistema. O processo inclui geocodificação automática de endereços e cálculo de distâncias usando a fórmula de Haversine.
        /// 
        /// Requisição via rota:
        /// ```http
        /// POST http://localhost:3001/api/Proximidade/calcular
        /// ```
        ///
        /// ### Processo executado:
        /// 1. **Geocodificação**: Converte endereços em coordenadas (lat/lng)
        /// 2. **Cálculo de distâncias**: Usa fórmula de Haversine para precisão
        /// 3. **Ranking**: Ordena abrigos por proximidade para cada usuário
        /// 4. **Persistência**: Salva resultados no banco de dados
        /// 
        /// ### Funcionalidades:
        /// - Limpa análises antigas antes do novo cálculo
        /// - Processa apenas coordenadas válidas
        /// - Calcula distância em quilômetros
        /// - Gera ranking personalizado para cada usuário
        /// 
        /// ### Exemplo de resposta de sucesso:
        /// ```json
        /// {
        ///   "message": "Proximidades calculadas com sucesso",
        ///   "timestamp": "2025-06-06T13:30:00.000Z"
        /// }
        /// ```
        /// 
        /// ### Exemplo de resposta de erro:
        /// ```json
        /// {
        ///   "message": "Falha no cálculo de proximidades. Verifique os logs para mais detalhes."
        /// }
        /// ```
        /// 
        /// ### Observações:
        /// - **Tempo de processamento**: Pode demorar alguns minutos dependendo da quantidade de dados
        /// - **Pré-requisitos**: Usuários e abrigos devem ter endereços válidos cadastrados
        /// - **API Externa**: Utiliza serviço de geocodificação DistanceMatrix.ai
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Proximidades calculadas com sucesso</response>
        /// <response code="400">Falha no cálculo de proximidades</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("calcular")]
        public async Task<ActionResult> CalcularProximidades()
        {
            try
            {
                _logger.LogInformation("Iniciando cálculo de proximidades");
                var sucesso = await _proximityService.CalcularTodasProximidadesAsync();
                
                if (sucesso)
                {
                    _logger.LogInformation("Cálculo de proximidades concluído com sucesso");
                    return Ok(new { 
                        message = "Proximidades calculadas com sucesso",
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    _logger.LogWarning("Falha no cálculo de proximidades");
                    return BadRequest(new { 
                        message = "Falha no cálculo de proximidades. Verifique os logs para mais detalhes." 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o cálculo de proximidades");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor durante o cálculo", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém todas as proximidades calculadas
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Consultar análises de proximidade completas
        /// 
        /// Este endpoint retorna todas as análises de proximidade que foram calculadas e salvas no sistema, mostrando a distância entre cada usuário e todos os abrigos disponíveis.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/proximidades
        /// ```
        ///
        /// ### Dados retornados:
        /// - Lista completa de proximidades usuário-abrigo
        /// - Distâncias em quilômetros
        /// - Ranking de proximidade para cada usuário
        /// - Informações completas de usuários e abrigos
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
        ///     "nomeUsuario": "João da Silva",
        ///     "emailUsuario": "joao@exemplo.com",
        ///     "enderecoUsuario": "Rua das Flores, Centro, São Paulo, SP, Brasil",
        ///     "idAbrigo": "683b055a4d65892ae33c3b1e",
        ///     "enderecoAbrigo": "Avenida Paulista, 1000, Bela Vista, São Paulo, SP, Brasil",
        ///     "capacidadeAbrigo": 50,
        ///     "ocupacaoAtual": 35,
        ///     "distanciaKm": 2.45,
        ///     "ranking": 1,
        ///     "dataCalculo": "2025-06-06T13:30:00.000Z"
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Campos importantes:
        /// - **ranking**: Posição do abrigo na lista de mais próximos para o usuário
        /// - **distanciaKm**: Distância em linha reta calculada pela fórmula de Haversine
        /// - **capacidadeAbrigo/ocupacaoAtual**: Disponibilidade do abrigo
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Lista de proximidades retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("proximidades")]
        public async Task<ActionResult<List<ProximidadeDto>>> GetTodasProximidades()
        {
            try
            {
                _logger.LogInformation("Buscando todas as proximidades");
                var proximidades = await _proximityService.BuscarProximidadesAsync();
                
                _logger.LogInformation($"Encontradas {proximidades.Count} análises de proximidade");
                return Ok(proximidades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar proximidades");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar proximidades", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém as proximidades de um usuário específico
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Consultar abrigos mais próximos de um usuário
        /// 
        /// Este endpoint retorna todos os abrigos ordenados por proximidade para um usuário específico, permitindo identificar rapidamente as melhores opções de abrigo para uma pessoa.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/proximidades/{idUsuario}
        /// ```
        ///
        /// ### Parâmetros obrigatórios:
        /// - **idUsuario**: ID único do usuário no sistema
        /// 
        /// ### Funcionalidades:
        /// - Lista ordenada por proximidade (mais próximo primeiro)
        /// - Inclui informações de disponibilidade dos abrigos
        /// - Mostra distâncias precisas em quilômetros
        /// - Ranking personalizado para o usuário
        /// 
        /// ### Exemplo de requisição:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/proximidades/6659fbbd3fae4c001fcf6d93
        /// ```
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
        ///     "nomeUsuario": "João da Silva",
        ///     "emailUsuario": "joao@exemplo.com",
        ///     "enderecoUsuario": "Rua das Flores, Centro, São Paulo, SP, Brasil",
        ///     "idAbrigo": "683b055a4d65892ae33c3b1e",
        ///     "enderecoAbrigo": "Avenida Paulista, 1000, Bela Vista, São Paulo, SP, Brasil",
        ///     "capacidadeAbrigo": 50,
        ///     "ocupacaoAtual": 35,
        ///     "distanciaKm": 2.45,
        ///     "ranking": 1,
        ///     "dataCalculo": "2025-06-06T13:30:00.000Z"
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Casos de erro:
        /// - **404**: Usuário não encontrado ou sem análises disponíveis
        /// - **400**: ID do usuário não fornecido ou inválido
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Proximidades do usuário retornadas com sucesso</response>
        /// <response code="400">ID do usuário é obrigatório</response>
        /// <response code="404">Usuário não encontrado ou sem análises de proximidade</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("proximidades/{idUsuario}")]
        public async Task<ActionResult<List<ProximidadeDto>>> GetProximidadesPorUsuario(string idUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idUsuario))
                {
                    return BadRequest(new { message = "ID do usuário é obrigatório" });
                }

                _logger.LogInformation($"Buscando proximidades para usuário {idUsuario}");
                var proximidades = await _proximityService.BuscarProximidadesAsync(idUsuario);
                
                if (!proximidades.Any())
                {
                    _logger.LogWarning($"Nenhuma proximidade encontrada para usuário {idUsuario}");
                    return NotFound(new { 
                        message = "Usuário não encontrado ou sem análises de proximidade disponíveis",
                        idUsuario = idUsuario
                    });
                }

                _logger.LogInformation($"Encontradas {proximidades.Count} proximidades para usuário {idUsuario}");
                return Ok(proximidades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar proximidades do usuário {idUsuario}");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar proximidades do usuário", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém os 3 abrigos mais próximos de um usuário
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Consultar top 3 abrigos mais próximos
        /// 
        /// Este endpoint retorna apenas os 3 abrigos mais próximos de um usuário específico, ideal para recomendações rápidas e decisões urgentes.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/proximidades/{idUsuario}/top3
        /// ```
        ///
        /// ### Parâmetros obrigatórios:
        /// - **idUsuario**: ID único do usuário no sistema
        /// 
        /// ### Casos de uso:
        /// - Recomendações urgentes de abrigo
        /// - Interface simplificada para usuários
        /// - Decisões rápidas em situações de emergência
        /// 
        /// ### Exemplo de requisição:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/proximidades/6659fbbd3fae4c001fcf6d93/top3
        /// ```
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "idUsuario": "6659fbbd3fae4c001fcf6d93",
        ///     "nomeUsuario": "João da Silva",
        ///     "emailUsuario": "joao@exemplo.com",
        ///     "enderecoUsuario": "Rua das Flores, Centro, São Paulo, SP, Brasil",
        ///     "idAbrigo": "683b055a4d65892ae33c3b1e",
        ///     "enderecoAbrigo": "Avenida Paulista, 1000, Bela Vista, São Paulo, SP, Brasil",
        ///     "capacidadeAbrigo": 50,
        ///     "ocupacaoAtual": 35,
        ///     "distanciaKm": 2.45,
        ///     "ranking": 1,
        ///     "dataCalculo": "2025-06-06T13:30:00.000Z"
        ///   },
        ///   {
        ///     "ranking": 2,
        ///     "distanciaKm": 3.12,
        ///     "..." : "..."
        ///   },
        ///   {
        ///     "ranking": 3,
        ///     "distanciaKm": 4.78,
        ///     "..." : "..."
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Benefícios:
        /// - **Performance**: Retorna apenas dados essenciais
        /// - **Relevância**: Foca nas melhores opções
        /// - **Simplicidade**: Ideal para interfaces móveis
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Top 3 proximidades retornadas com sucesso</response>
        /// <response code="400">ID do usuário é obrigatório</response>
        /// <response code="404">Usuário não encontrado ou sem análises de proximidade</response>
        /// <response code="500">Erro interno do servidor</response>>
        [HttpGet("proximidades/{idUsuario}/top3")]
        public async Task<ActionResult<List<ProximidadeDto>>> GetTop3ProximidadesPorUsuario(string idUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idUsuario))
                {
                    return BadRequest(new { message = "ID do usuário é obrigatório" });
                }

                _logger.LogInformation($"Buscando top 3 proximidades para usuário {idUsuario}");
                var proximidades = await _proximityService.BuscarProximidadesAsync(idUsuario);
                var top3 = proximidades.Take(3).ToList();
                
                if (!top3.Any())
                {
                    return NotFound(new { 
                        message = "Usuário não encontrado ou sem análises de proximidade disponíveis",
                        idUsuario = idUsuario
                    });
                }

                _logger.LogInformation($"Retornando top {top3.Count} proximidades para usuário {idUsuario}");
                return Ok(top3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar top 3 proximidades do usuário {idUsuario}");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar top 3 proximidades", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém dados completos para o dashboard
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Dashboard executivo com dados consolidados
        /// 
        /// Este endpoint retorna um conjunto completo de dados para alimentar dashboards e relatórios executivos, incluindo estatísticas, listas de usuários, abrigos e análises de proximidade.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/dashboard
        /// ```
        ///
        /// ### Dados incluídos:
        /// - **Usuários**: Lista completa com endereços e coordenadas
        /// - **Abrigos**: Lista com capacidades e ocupação
        /// - **Proximidades**: Todas as análises calculadas
        /// - **Estatísticas**: Métricas consolidadas do sistema
        /// 
        /// ### Exemplo de resposta:
        /// ```json
        /// {
        ///   "usuarios": [
        ///     {
        ///       "idUsuario": "6659fbbd3fae4c001fcf6d93",
        ///       "nomeUsuario": "João da Silva",
        ///       "email": "joao@exemplo.com",
        ///       "..."
        ///     }
        ///   ],
        ///   "abrigos": [
        ///     {
        ///       "idAbrigo": "683b055a4d65892ae33c3b1e",
        ///       "capacidadeTotal": 50,
        ///       "ocupacaoAtual": 35,
        ///       "..."
        ///     }
        ///   ],
        ///   "proximidade": [
        ///     {
        ///       "ranking": 1,
        ///       "distanciaKm": 2.45,
        ///       "..."
        ///     }
        ///   ],
        ///   "estatisticas": {
        ///     "totalUsuarios": 25,
        ///     "totalAbrigos": 8,
        ///     "usuariosComCoordenadas": 20,
        ///     "abrigosComCoordenadas": 7,
        ///     "totalCalculosProximidade": 160,
        ///     "distanciaMedia": 5.23,
        ///     "distanciaMinima": 0.85,
        ///     "distanciaMaxima": 15.67
        ///   }
        /// }
        /// ```
        /// 
        /// ### Métricas estatísticas:
        /// - **totalUsuarios/totalAbrigos**: Contadores gerais
        /// - **usuariosComCoordenadas/abrigosComCoordenadas**: Taxa de sucesso da geocodificação
        /// - **distanciaMedia/distanciaMinima/distanciaMaxima**: Análise das distâncias calculadas
        /// 
        /// ### Casos de uso:
        /// - Dashboards executivos
        /// - Relatórios de gestão
        /// - Análises de performance do sistema
        /// - Monitoramento de capacidade dos abrigos
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Dashboard gerado com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            try
            {
                _logger.LogInformation("Gerando dashboard completo");
                var dashboard = await _proximityService.GerarDashboardAsync();
                
                _logger.LogInformation($"Dashboard gerado: {dashboard.Estatisticas.TotalUsuarios} usuários, " +
                                     $"{dashboard.Estatisticas.TotalAbrigos} abrigos, " +
                                     $"{dashboard.Estatisticas.TotalCalculosProximidade} cálculos");
                
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar dashboard");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao gerar dashboard", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtém estatísticas resumidas do sistema
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Métricas consolidadas de performance
        /// 
        /// Este endpoint retorna apenas as estatísticas essenciais do sistema de proximidade, ideal para widgets de dashboard ou monitoramento básico.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/estatisticas
        /// ```
        ///
        /// ### Exemplo de resposta:
        /// ```json
        /// {
        ///   "totalUsuarios": 25,
        ///   "totalAbrigos": 8,
        ///   "usuariosComCoordenadas": 20,
        ///   "abrigosComCoordenadas": 7,
        ///   "totalCalculosProximidade": 160,
        ///   "distanciaMedia": 5.23,
        ///   "distanciaMinima": 0.85,
        ///   "distanciaMaxima": 15.67
        /// }
        /// ```
        /// 
        /// ### Métricas incluídas:
        /// - **Contadores**: Total de usuários e abrigos
        /// - **Taxa de sucesso**: Geocodificação bem-sucedida
        /// - **Performance**: Estatísticas de distância
        /// - **Volume**: Total de cálculos realizados
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Estatísticas retornadas com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("estatisticas")]
        public async Task<ActionResult<EstatisticasDto>> GetEstatisticas()
        {
            try
            {
                _logger.LogInformation("Buscando estatísticas do sistema");
                var dashboard = await _proximityService.GerarDashboardAsync();
                
                return Ok(dashboard.Estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar estatísticas");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao buscar estatísticas", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Força atualização das coordenadas de um usuário específico
        /// </summary>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns>Coordenadas atualizadas</returns>
        [HttpPost("atualizar-coordenadas/usuario/{idUsuario}")]
        public async Task<ActionResult> AtualizarCoordenadasUsuario(string idUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idUsuario))
                {
                    return BadRequest(new { message = "ID do usuário é obrigatório" });
                }

                _logger.LogInformation($"Atualizando coordenadas do usuário {idUsuario}");
                
                // Força busca nova coordenada (pode implementar lógica para invalidar cache)
                var coordenada = await _proximityService.BuscarOuCriarCoordenadaUsuarioAsync(idUsuario);
                
                if (coordenada == null)
                {
                    return NotFound(new { 
                        message = "Usuário não encontrado ou sem endereço válido",
                        idUsuario = idUsuario
                    });
                }

                return Ok(new {
                    message = "Coordenadas atualizadas com sucesso",
                    idUsuario = idUsuario,
                    latitude = coordenada.Latitude,
                    longitude = coordenada.Longitude,
                    endereco = coordenada.EnderecoCompleto,
                    status = coordenada.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar coordenadas do usuário {idUsuario}");
                return StatusCode(500, new { 
                    message = "Erro interno do servidor ao atualizar coordenadas", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Health check do serviço de proximidade
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// ## Verificação de saúde do sistema
        /// 
        /// Endpoint simples para verificar se o serviço de proximidade está funcionando corretamente. Útil para monitoramento e health checks automatizados.
        /// 
        /// Requisição via rota:
        /// ```http
        /// GET http://localhost:3001/api/Proximidade/health
        /// ```
        ///
        /// ### Exemplo de resposta:
        /// ```json
        /// {
        ///   "service": "ProximityService",
        ///   "status": "Healthy",
        ///   "timestamp": "2025-06-06T13:30:00.000Z",
        ///   "version": "1.0.0"
        /// }
        /// ```
        /// 
        /// ### Casos de uso:
        /// - Monitoramento automatizado
        /// - Load balancer health checks
        /// - Verificação de disponibilidade
        /// - Testes de conectividade
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Serviço funcionando corretamente</response>
        [HttpGet("health")]
        public ActionResult GetHealth()
        {
            return Ok(new {
                service = "ProximityService",
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
    }
}