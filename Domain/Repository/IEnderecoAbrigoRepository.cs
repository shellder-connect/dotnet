using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface IEnderecoAbrigoRepository
    {
        Task<EnderecoAbrigo> Criar(EnderecoAbrigo enderecoAbrigo);
        Task<EnderecoAbrigo> ConsultarPorUsuarioId(string id);
        Task<List<EnderecoAbrigo>> ConsultarTodos();
        Task<EnderecoAbrigo?> Atualizar(EnderecoAbrigo enderecoAbrigo);
        Task<EnderecoAbrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar);
        Task Excluir(string id);

    }
}
