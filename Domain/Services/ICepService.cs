using Project.Models;
using System.Threading.Tasks;

namespace Project.Infrastructure.Interfaces
{
    public interface ICepService
    {
        Task<EnderecoDTO?> ConsultarCepAsync(string cep);
    }
}
