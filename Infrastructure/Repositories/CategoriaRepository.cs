using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IMongoCollection<Categoria> _categoriaCollection;

        public CategoriaRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _categoriaCollection = mongoDatabase.GetCollection<Categoria>(settings.Value.CategoriaCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public CategoriaRepository(IMongoCollection<Categoria> categoriaCollection)
        {
            _categoriaCollection = categoriaCollection;
        }

        public async Task<Categoria> Criar(Categoria categoria)
        {
            await _categoriaCollection.InsertOneAsync(categoria);
            return categoria;
        }

        public async Task<Categoria> ConsultarId(string id)
        {
            var filtro = Builders<Categoria>.Filter.Eq(u => u.Id, id);
            return await _categoriaCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Categoria>> ConsultarTodos()
        {
            return await _categoriaCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Categoria?> Atualizar(Categoria categoria)
        {
            var filtro = Builders<Categoria>.Filter.Eq(u => u.Id, categoria.Id);

            var update = Builders<Categoria>.Update
                .Set(u => u.Descricao, categoria.Descricao);

            var resultado = await _categoriaCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return categoria;
            }

            return null;
        }

        public async Task<Categoria?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var categoria = await ConsultarId(id);
            if (categoria == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Categoria>.Update;
            var updateDefinitions = new List<UpdateDefinition<Categoria>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "descricao":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Descricao, campo.Value.ToString()));
                        break;
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return categoria;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Categoria>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _categoriaCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Categoria>.Filter.Eq(u => u.Id, id);
            await _categoriaCollection.DeleteOneAsync(filtro);
        }
    }
}
