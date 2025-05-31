using Project.Models;

namespace Project.Infrastructure.Interfaces
    {
    public interface IMLService
    {
        Task<bool> TreinarModelo();
        Task<bool> TreinarModeloComCSV(string? caminhoCSV = null);
        Task<PredicaoResultado> Prever(string localizacao, int quantidadeEventos, int capacidadeAbrigo, int ocupacaoAtual);
        Task<List<string>> ObterLocalizacoesDisponiveis();
    }
}