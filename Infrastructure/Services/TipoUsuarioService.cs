using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public TipoUsuarioService(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        public async Task<TipoUsuario> Criar(TipoUsuario tipoUsuario)
        {
            return await _tipoUsuarioRepository.Criar(tipoUsuario);
        }

        public async Task<List<TipoUsuario>> ConsultarTodos()
        {
            var tipoUsuarios = await _tipoUsuarioRepository.ConsultarTodos();
            return tipoUsuarios.ToList(); 
        }

        public async Task<TipoUsuario> ConsultarId(string id)
        {
            return await _tipoUsuarioRepository.ConsultarId(id);
        }

        public async Task<TipoUsuario?> Atualizar(TipoUsuario tipoUsuario)
        {
            return await _tipoUsuarioRepository.Atualizar(tipoUsuario);
        }

        public async Task<TipoUsuario?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _tipoUsuarioRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _tipoUsuarioRepository.Excluir(id);
        }
    }
}
