using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class LocalidadeRepository : ILocalidadeRepository
    {
        private readonly IMongoCollection<Localidade> _localidadeCollection;

        public LocalidadeRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _localidadeCollection = mongoDatabase.GetCollection<Localidade>(settings.Value.LocalidadeCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public LocalidadeRepository(IMongoCollection<Localidade> localidadeCollection)
        {
            _localidadeCollection = localidadeCollection;
        }


        public async Task<List<Localidade>> ConsultarTodos()
        {
            return await _localidadeCollection.Find(_ => true).ToListAsync();
        }

    }
}
