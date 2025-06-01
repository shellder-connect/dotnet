using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class MuralRepository : IMuralRepository
    {
        private readonly IMongoCollection<Mural> _muralCollection;

        public MuralRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _muralCollection = mongoDatabase.GetCollection<Mural>(settings.Value.MuralCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public MuralRepository(IMongoCollection<Mural> muralCollection)
        {
            _muralCollection = muralCollection;
        }

        public async Task<Mural> Criar(Mural mural)
        {
            await _muralCollection.InsertOneAsync(mural);
            return mural;
        }

        public async Task<Mural> ConsultarId(string id)
        {
            var filtro = Builders<Mural>.Filter.Eq(u => u.Id, id);
            return await _muralCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Mural>> ConsultarTodos()
        {
            return await _muralCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Mural?> Atualizar(Mural mural)
        {
            var filtro = Builders<Mural>.Filter.Eq(u => u.Id, mural.Id);

            var update = Builders<Mural>.Update
                .Set(u => u.IdUsuario, mural.IdUsuario)
                .Set(u => u.Titulo, mural.Titulo)
                .Set(u => u.Comentario, mural.Comentario)
                .Set(u => u.TipoPost, mural.TipoPost)
                .Set(u => u.Data, mural.Data);

            var resultado = await _muralCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return mural;
            }

            return null;
        }

        public async Task<Mural?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var mural = await ConsultarId(id);
            if (mural == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Mural>.Update;
            var updateDefinitions = new List<UpdateDefinition<Mural>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "IdUsuario":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdUsuario, campo.Value.ToString()));
                        break;
                    case "Titulo":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Titulo, campo.Value.ToString()));
                    break;
                    case "Comentario":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Comentario, campo.Value.ToString()));
                        break;
                    case "TipoPost":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.TipoPost, campo.Value.ToString()));
                        break;
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return mural;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Mural>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _muralCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Mural>.Filter.Eq(u => u.Id, id);
            await _muralCollection.DeleteOneAsync(filtro);
        }
    }
}
