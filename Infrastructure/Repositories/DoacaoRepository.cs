using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class DoacaoRepository : IDoacaoRepository
    {
        private readonly IMongoCollection<Doacao> _doacaoCollection;

        public DoacaoRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _doacaoCollection = mongoDatabase.GetCollection<Doacao>(settings.Value.DoacaoCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public DoacaoRepository(IMongoCollection<Doacao> doacaoCollection)
        {
            _doacaoCollection = doacaoCollection;
        }

        public async Task<Doacao> Criar(Doacao doacao)
        {
            await _doacaoCollection.InsertOneAsync(doacao);
            return doacao;
        }

        public async Task<Doacao> ConsultarId(string id)
        {
            var filtro = Builders<Doacao>.Filter.Eq(u => u.Id, id);
            return await _doacaoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Doacao>> ConsultarTodos()
        {
            return await _doacaoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Doacao?> Atualizar(Doacao doacao)
        {
            var filtro = Builders<Doacao>.Filter.Eq(u => u.Id, doacao.Id);

            var update = Builders<Doacao>.Update
                .Set(u => u.Descricao, doacao.Descricao);

            var resultado = await _doacaoCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return doacao;
            }

            return null;
        }

        public async Task<Doacao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var doacao = await ConsultarId(id);
            if (doacao == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Doacao>.Update;
            var updateDefinitions = new List<UpdateDefinition<Doacao>>();

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
                return doacao;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Doacao>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _doacaoCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Doacao>.Filter.Eq(u => u.Id, id);
            await _doacaoCollection.DeleteOneAsync(filtro);
        }
    }
}
