using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class MuralService : IMuralService
    {
        private readonly IMuralRepository _muralRepository;

        public MuralService(IMuralRepository muralRepository)
        {
            _muralRepository = muralRepository;
        }

        public async Task<Mural> Criar(Mural mural)
        {
            return await _muralRepository.Criar(mural);
        }

        public async Task<List<Mural>> ConsultarTodos()
        {
            var murals = await _muralRepository.ConsultarTodos();
            return murals.ToList(); 
        }

        public async Task<Mural> ConsultarId(string id)
        {
            return await _muralRepository.ConsultarId(id);
        }

        public async Task<Mural?> Atualizar(Mural mural)
        {
            return await _muralRepository.Atualizar(mural);
        }

        public async Task<Mural?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _muralRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _muralRepository.Excluir(id);
        }
    }
}
