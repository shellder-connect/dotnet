using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IProximityRepository
    {
        #region Operações com Usuários
        Task<List<Usuario>> BuscarTodosUsuarios();
        Task<Usuario?> BuscarUsuarioPorId(string id);
        #endregion

        #region Operações com Endereços
        Task<List<Endereco>> BuscarTodosEnderecos();
        Task<Endereco?> BuscarEnderecoPorId(string id);
        Task<Endereco?> BuscarEnderecoPorUsuario(string idUsuario);
        #endregion

        #region Operações com Abrigos
        Task<List<Abrigo>> BuscarTodosAbrigos();
        Task<Abrigo?> BuscarAbrigoPorId(string id);
        #endregion

        #region Operações com Endereços de Abrigos
        Task<List<EnderecoAbrigo>> BuscarTodosEnderecosAbrigos();
        Task<EnderecoAbrigo?> BuscarEnderecoAbrigoPorId(string id);
        Task<EnderecoAbrigo?> BuscarEnderecoAbrigoPorAbrigo(string idAbrigo);
        #endregion

        #region Operações com Coordenadas
        Task<Coordenada> CriarCoordenada(Coordenada coordenada);
        Task<Coordenada?> BuscarCoordenadaPorReferencia(string tipo, string referenciaId);
        Task<List<Coordenada>> BuscarCoordenadasPorTipo(string tipo);
        Task<Coordenada?> AtualizarCoordenada(Coordenada coordenada);
        #endregion

        #region Operações com Análise de Proximidade
        Task<bool> LimparAnaliseProximidade();
        Task<bool> CriarAnaliseProximidade(List<AnaliseProximidade> analises);
        Task<List<AnaliseProximidade>> BuscarAnaliseProximidade(string? idUsuario = null);
        Task<List<AnaliseProximidade>> BuscarTop3ProximidadePorUsuario(string idUsuario);
        #endregion

        #region Operações com Relatório de Proximidade
        Task<RelatorioProximidade> CriarRelatorioProximidade(RelatorioProximidade relatorio);
        Task<RelatorioProximidade?> BuscarUltimoRelatorioProximidade();
        Task<List<RelatorioProximidade>> BuscarTodosRelatoriosProximidade();
        #endregion

        #region Operações Auxiliares
        Task<long> ContarUsuarios();
        Task<long> ContarAbrigos();
        Task<long> ContarCoordenadasValidas(string tipo);
        Task<long> ContarAnaliseProximidade();
        #endregion
    }
}