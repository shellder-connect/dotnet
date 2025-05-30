using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class DistribuicaoService : IDistribuicaoService
    {
        private readonly IDistribuicaoRepository _distribuicaoRepository;

        public DistribuicaoService(IDistribuicaoRepository distribuicaoRepository)
        {
            _distribuicaoRepository = distribuicaoRepository;
        }

        public async Task<Distribuicao> Criar(Distribuicao distribuicao)
        {
            return await _distribuicaoRepository.Criar(distribuicao);
        }

        public async Task<List<Distribuicao>> ConsultarTodos()
        {
            var distribuicaos = await _distribuicaoRepository.ConsultarTodos();
            return distribuicaos.ToList(); 
        }

        public async Task<Distribuicao> ConsultarId(string id)
        {
            return await _distribuicaoRepository.ConsultarId(id);
        }

        public async Task<Distribuicao?> Atualizar(Distribuicao distribuicao)
        {
            return await _distribuicaoRepository.Atualizar(distribuicao);
        }

        public async Task<Distribuicao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _distribuicaoRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _distribuicaoRepository.Excluir(id);
        }
    }
}
