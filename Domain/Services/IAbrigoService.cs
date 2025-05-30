using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IAbrigoService
    {
        Task<Abrigo> Criar(Abrigo abrigo);
        Task<Abrigo> ConsultarId(string id);
        Task<List<Abrigo>> ConsultarTodos();
        Task<Abrigo?> Atualizar(Abrigo abrigo);
        Task<Abrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);
    }
}
