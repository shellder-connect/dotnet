using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IMLRepository
    {
        Task<List<DadosTreinamento>> ObterDadosParaTreinamento();
        Task<List<DadosTreinamento>> ObterDadosPorLocalizacao(string localizacao);
        Task<List<string>> ObterLocalizacoesAtivas();
    }
}