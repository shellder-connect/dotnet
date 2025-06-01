using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class EnderecoAbrigoService : IEnderecoAbrigoService
    {
        private readonly IEnderecoAbrigoRepository _enderecoAbrigoRepository;

        public EnderecoAbrigoService(IEnderecoAbrigoRepository enderecoAbrigoRepository)
        {
            _enderecoAbrigoRepository = enderecoAbrigoRepository;
        }

        public async Task<EnderecoAbrigo> Criar(EnderecoAbrigo enderecoAbrigo)
        {
            return await _enderecoAbrigoRepository.Criar(enderecoAbrigo);
        }

        public async Task<List<EnderecoAbrigo>> ConsultarTodos()
        {
            var enderecoAbrigos = await _enderecoAbrigoRepository.ConsultarTodos();
            return enderecoAbrigos.ToList(); 
        }

        public async Task<EnderecoAbrigo> ConsultarPorUsuarioId(string usuarioId)
        {
            return await _enderecoAbrigoRepository.ConsultarPorUsuarioId(usuarioId);
           
        }
        public async Task<EnderecoAbrigo?> ConsultarId(string id)
        {
            var enderecoAbrigos = await _enderecoAbrigoRepository.ConsultarTodos();
            return enderecoAbrigos.FirstOrDefault(e => e.Id == id);
        }


        public async Task<EnderecoAbrigo?> Atualizar(EnderecoAbrigo enderecoAbrigo)
        {
            return await _enderecoAbrigoRepository.Atualizar(enderecoAbrigo);
        }


        public async Task<EnderecoAbrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _enderecoAbrigoRepository.AtualizarParcial(id, camposParaAtualizar);
        }


        public async Task Excluir(string id)
        {
            await _enderecoAbrigoRepository.Excluir(id);
        }
    }
}
