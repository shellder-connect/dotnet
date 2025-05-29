using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface ITipoUsuarioRepository
    {
        Task<TipoUsuario> Criar(TipoUsuario tipoUsuario);
        Task<TipoUsuario> ConsultarId(string id);
        Task<List<TipoUsuario>> ConsultarTodos();
        Task<TipoUsuario?> Atualizar(TipoUsuario tipoUsuario);
        Task<TipoUsuario?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);

    }
}
