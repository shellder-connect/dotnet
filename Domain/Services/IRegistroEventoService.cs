using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IRegistroEventoService
    {
        Task<RegistroEvento> Criar(RegistroEvento registroEvento);
        Task<RegistroEvento> ConsultarId(string id);
        Task<List<RegistroEvento>> ConsultarTodos();
        Task<RegistroEvento?> Atualizar(RegistroEvento registroEvento);
        Task<RegistroEvento?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);
    }
}
