using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class RegistroEventoRepository : IRegistroEventoRepository
    {
        private readonly IMongoCollection<RegistroEvento> _registroEventoCollection;

        public RegistroEventoRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _registroEventoCollection = mongoDatabase.GetCollection<RegistroEvento>(settings.Value.RegistroEventoCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public RegistroEventoRepository(IMongoCollection<RegistroEvento> registroEventoCollection)
        {
            _registroEventoCollection = registroEventoCollection;
        }

        public async Task<RegistroEvento> Criar(RegistroEvento registroEvento)
        {
            await _registroEventoCollection.InsertOneAsync(registroEvento);
            return registroEvento;
        }

        public async Task<RegistroEvento> ConsultarId(string id)
        {
            var filtro = Builders<RegistroEvento>.Filter.Eq(u => u.Id, id);
            return await _registroEventoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<RegistroEvento>> ConsultarTodos()
        {
            return await _registroEventoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<RegistroEvento?> Atualizar(RegistroEvento registroEvento)
        {
            var filtro = Builders<RegistroEvento>.Filter.Eq(u => u.Id, registroEvento.Id);

            var update = Builders<RegistroEvento>.Update
                .Set(u => u.Descricao, registroEvento.Descricao)
                .Set(u => u.DataHora, registroEvento.DataHora)
                .Set(u => u.IdUsuario, registroEvento.IdUsuario)
                .Set(u => u.Localizacao, registroEvento.Localizacao)
                .Set(u => u.IdAbrigo, registroEvento.IdAbrigo);

            var resultado = await _registroEventoCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return registroEvento;
            }

            return null;
        }

        public async Task<RegistroEvento?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var registroEvento = await ConsultarId(id);
            if (registroEvento == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<RegistroEvento>.Update;
            var updateDefinitions = new List<UpdateDefinition<RegistroEvento>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "Descricao":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Descricao, campo.Value.ToString()));
                        break;
                    case "DataHora":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.DataHora, Convert.ToDateTime(campo.Value)));
                        break;
                    case "IdUsuario":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdUsuario, campo.Value.ToString()));
                        break;
                    case "Localizacao":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Localizacao, campo.Value.ToString()));
                        break;
                    case "IdAbrigo":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdAbrigo, campo.Value.ToString()));
                        break;
                  
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return registroEvento;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<RegistroEvento>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _registroEventoCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<RegistroEvento>.Filter.Eq(u => u.Id, id);
            await _registroEventoCollection.DeleteOneAsync(filtro);
        }
    }
}
