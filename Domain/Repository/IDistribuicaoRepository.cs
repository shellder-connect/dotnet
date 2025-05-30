using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IDistribuicaoRepository
    {
        Task<Distribuicao> Criar(Distribuicao distribuicao);
        Task<Distribuicao> ConsultarId(string id);
        Task<List<Distribuicao>> ConsultarTodos();
        Task<Distribuicao?> Atualizar(Distribuicao distribuicao);
        Task<Distribuicao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);

    }
}
