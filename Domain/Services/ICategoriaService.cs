using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface ICategoriaService
    {
        Task<Categoria> Criar(Categoria categoria);
        Task<Categoria> ConsultarId(string id);
        Task<List<Categoria>> ConsultarTodos();
        Task<Categoria?> Atualizar(Categoria categoria);
        Task<Categoria?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);
    }
}
