using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IDoacaoService
    {
        Task<Doacao> Criar(Doacao doacao);
        Task<Doacao> ConsultarId(string id);
        Task<List<Doacao>> ConsultarTodos();
        Task<Doacao?> Atualizar(Doacao doacao);
        Task<Doacao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);
    }
}
