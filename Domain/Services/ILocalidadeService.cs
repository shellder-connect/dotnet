using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface ILocalidadeService
    {
        Task<List<Localidade>> ConsultarTodos();
    }
}
