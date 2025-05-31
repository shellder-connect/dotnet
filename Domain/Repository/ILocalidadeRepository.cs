using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface ILocalidadeRepository
    {
        Task<List<Localidade>> ConsultarTodos();

    }
}
