using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IMuralRepository
    {
        Task<Mural> Criar(Mural mural);
        Task<Mural> ConsultarId(string id);
        Task<List<Mural>> ConsultarTodos();
        Task<Mural?> Atualizar(Mural mural);
        Task<Mural?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);

    }
}
