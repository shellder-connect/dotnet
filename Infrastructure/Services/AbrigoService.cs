using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class AbrigoService : IAbrigoService
    {
        private readonly IAbrigoRepository _abrigoRepository;

        public AbrigoService(IAbrigoRepository abrigoRepository)
        {
            _abrigoRepository = abrigoRepository;
        }

        public async Task<Abrigo> Criar(Abrigo abrigo)
        {
            return await _abrigoRepository.Criar(abrigo);
        }

        public async Task<List<Abrigo>> ConsultarTodos()
        {
            var abrigos = await _abrigoRepository.ConsultarTodos();
            return abrigos.ToList(); 
        }

        public async Task<Abrigo> ConsultarId(string id)
        {
            return await _abrigoRepository.ConsultarId(id);
        }

        public async Task<Abrigo?> Atualizar(Abrigo abrigo)
        {
            return await _abrigoRepository.Atualizar(abrigo);
        }

        public async Task<Abrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _abrigoRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _abrigoRepository.Excluir(id);
        }
    }
}
