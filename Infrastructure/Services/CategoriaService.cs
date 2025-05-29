using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Categoria> Criar(Categoria categoria)
        {
            return await _categoriaRepository.Criar(categoria);
        }

        public async Task<List<Categoria>> ConsultarTodos()
        {
            var categorias = await _categoriaRepository.ConsultarTodos();
            return categorias.ToList(); 
        }

        public async Task<Categoria> ConsultarId(string id)
        {
            return await _categoriaRepository.ConsultarId(id);
        }

        public async Task<Categoria?> Atualizar(Categoria categoria)
        {
            return await _categoriaRepository.Atualizar(categoria);
        }

        public async Task<Categoria?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _categoriaRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _categoriaRepository.Excluir(id);
        }
    }
}
