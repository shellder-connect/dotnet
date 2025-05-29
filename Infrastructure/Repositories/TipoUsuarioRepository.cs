using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly IMongoCollection<TipoUsuario> _tipoUsuarioCollection;

        public TipoUsuarioRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _tipoUsuarioCollection = mongoDatabase.GetCollection<TipoUsuario>(settings.Value.TipoUsuarioCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public TipoUsuarioRepository(IMongoCollection<TipoUsuario> tipoUsuarioCollection)
        {
            _tipoUsuarioCollection = tipoUsuarioCollection;
        }

        public async Task<TipoUsuario> Criar(TipoUsuario tipoUsuario)
        {
            await _tipoUsuarioCollection.InsertOneAsync(tipoUsuario);
            return tipoUsuario;
        }

        public async Task<TipoUsuario> ConsultarId(string id)
        {
            var filtro = Builders<TipoUsuario>.Filter.Eq(u => u.Id, id);
            return await _tipoUsuarioCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<TipoUsuario>> ConsultarTodos()
        {
            return await _tipoUsuarioCollection.Find(_ => true).ToListAsync();
        }

        public async Task<TipoUsuario?> Atualizar(TipoUsuario tipoUsuario)
        {
            var filtro = Builders<TipoUsuario>.Filter.Eq(u => u.Id, tipoUsuario.Id);

            var update = Builders<TipoUsuario>.Update
                .Set(u => u.Descricao, tipoUsuario.Descricao);

            var resultado = await _tipoUsuarioCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return tipoUsuario;
            }

            return null;
        }

        public async Task<TipoUsuario?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var tipoUsuario = await ConsultarId(id);
            if (tipoUsuario == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<TipoUsuario>.Update;
            var updateDefinitions = new List<UpdateDefinition<TipoUsuario>>();

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
                return tipoUsuario;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<TipoUsuario>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _tipoUsuarioCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<TipoUsuario>.Filter.Eq(u => u.Id, id);
            await _tipoUsuarioCollection.DeleteOneAsync(filtro);
        }
    }
}
