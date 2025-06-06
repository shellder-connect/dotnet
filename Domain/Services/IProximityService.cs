using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IProximityService
    {
        #region Geocodificação
        Task<(double? lat, double? lng)> ObterCoordenadasAsync(string endereco);
        #endregion

        #region Cálculo de Distância
        double CalcularDistancia(double lat1, double lon1, double lat2, double lon2);
        #endregion

        #region Gerenciamento de Coordenadas
        Task<Coordenada?> BuscarOuCriarCoordenadaUsuarioAsync(string idUsuario);
        Task<Coordenada?> BuscarOuCriarCoordenadaAbrigoAsync(string idAbrigo);
        #endregion

        #region Processamento de Dados
        Task<List<UsuarioComEnderecoDto>> ProcessarUsuariosComEnderecosAsync();
        Task<List<AbrigoComEnderecoDto>> ProcessarAbrigosComEnderecosAsync();
        #endregion

        #region Análise de Proximidade
        Task<bool> CalcularTodasProximidadesAsync();
        Task<List<ProximidadeDto>> BuscarProximidadesAsync(string? idUsuario = null);
        Task<List<ProximidadeDto>> BuscarTop3ProximidadesPorUsuarioAsync(string idUsuario);
        #endregion

        #region Dashboard
        Task<DashboardDto> GerarDashboardAsync();
        #endregion
    }
}