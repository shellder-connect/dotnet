using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class AbrigoRepository : IAbrigoRepository
    {
        private readonly IMongoCollection<Abrigo> _abrigoCollection;

        public AbrigoRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _abrigoCollection = mongoDatabase.GetCollection<Abrigo>(settings.Value.AbrigoCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public AbrigoRepository(IMongoCollection<Abrigo> abrigoCollection)
        {
            _abrigoCollection = abrigoCollection;
        }

        public async Task<Abrigo> Criar(Abrigo abrigo)
        {
            await _abrigoCollection.InsertOneAsync(abrigo);
            return abrigo;
        }

        public async Task<Abrigo> ConsultarId(string id)
        {
            var filtro = Builders<Abrigo>.Filter.Eq(u => u.Id, id);
            return await _abrigoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Abrigo>> ConsultarTodos()
        {
            return await _abrigoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Abrigo?> Atualizar(Abrigo abrigo)
        {
            var filtro = Builders<Abrigo>.Filter.Eq(u => u.Id, abrigo.Id);

            var update = Builders<Abrigo>.Update
                .Set(u => u.Descricao, abrigo.Descricao);

            var resultado = await _abrigoCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return abrigo;
            }

            return null;
        }

        public async Task<Abrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var abrigo = await ConsultarId(id);
            if (abrigo == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Abrigo>.Update;
            var updateDefinitions = new List<UpdateDefinition<Abrigo>>();

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
                return abrigo;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Abrigo>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _abrigoCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Abrigo>.Filter.Eq(u => u.Id, id);
            await _abrigoCollection.DeleteOneAsync(filtro);
        }
    }
}
