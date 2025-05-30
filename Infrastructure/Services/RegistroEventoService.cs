using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class RegistroEventoService : IRegistroEventoService
    {
        private readonly IRegistroEventoRepository _registroEventoRepository;

        public RegistroEventoService(IRegistroEventoRepository registroEventoRepository)
        {
            _registroEventoRepository = registroEventoRepository;
        }

        public async Task<RegistroEvento> Criar(RegistroEvento registroEvento)
        {
            return await _registroEventoRepository.Criar(registroEvento);
        }

        public async Task<List<RegistroEvento>> ConsultarTodos()
        {
            var registroEventos = await _registroEventoRepository.ConsultarTodos();
            return registroEventos.ToList(); 
        }

        public async Task<RegistroEvento> ConsultarId(string id)
        {
            return await _registroEventoRepository.ConsultarId(id);
        }

        public async Task<RegistroEvento?> Atualizar(RegistroEvento registroEvento)
        {
            return await _registroEventoRepository.Atualizar(registroEvento);
        }

        public async Task<RegistroEvento?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _registroEventoRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _registroEventoRepository.Excluir(id);
        }
    }
}
