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
        /// <returns>Lista de usuários com endereços</returns>
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
        /// <returns>Lista de abrigos com endereços</returns>
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
        /// <returns>Confirmação do processamento</returns>
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
        /// <returns>Lista de proximidades com ranking</returns>
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
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns>Lista de abrigos mais próximos do usuário</returns>
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
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns>Top 3 abrigos mais próximos</returns>
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
        /// <returns>Dashboard com usuários, abrigos, proximidades e estatísticas</returns>
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
        /// <returns>Estatísticas básicas</returns>
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
        /// <returns>Status do serviço</returns>
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