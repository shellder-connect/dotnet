using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly IDoacaoRepository _doacaoRepository;

        public DoacaoService(IDoacaoRepository doacaoRepository)
        {
            _doacaoRepository = doacaoRepository;
        }

        public async Task<Doacao> Criar(Doacao doacao)
        {
            return await _doacaoRepository.Criar(doacao);
        }

        public async Task<List<Doacao>> ConsultarTodos()
        {
            var doacaos = await _doacaoRepository.ConsultarTodos();
            return doacaos.ToList(); 
        }

        public async Task<Doacao> ConsultarId(string id)
        {
            return await _doacaoRepository.ConsultarId(id);
        }

        public async Task<Doacao?> Atualizar(Doacao doacao)
        {
            return await _doacaoRepository.Atualizar(doacao);
        }

        public async Task<Doacao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _doacaoRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _doacaoRepository.Excluir(id);
        }
    }
}
