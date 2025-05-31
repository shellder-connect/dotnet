using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class LocalidadeService : ILocalidadeService
    {
        private readonly ILocalidadeRepository _localidadeRepository;

        public LocalidadeService(ILocalidadeRepository localidadeRepository)
        {
            _localidadeRepository = localidadeRepository;
        }


        public async Task<List<Localidade>> ConsultarTodos()
        {
            var localidades = await _localidadeRepository.ConsultarTodos();
            return localidades.ToList(); 
        }

    }
}
