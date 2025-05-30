using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class DistribuicaoRepository : IDistribuicaoRepository
    {
        private readonly IMongoCollection<Distribuicao> _distribuicaoCollection;

        public DistribuicaoRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _distribuicaoCollection = mongoDatabase.GetCollection<Distribuicao>(settings.Value.DistribuicaoCollectionName);
        }

        // 👇 Novo construtor para testes (injeção direta do mock)
        public DistribuicaoRepository(IMongoCollection<Distribuicao> distribuicaoCollection)
        {
            _distribuicaoCollection = distribuicaoCollection;
        }

        public async Task<Distribuicao> Criar(Distribuicao distribuicao)
        {
            await _distribuicaoCollection.InsertOneAsync(distribuicao);
            return distribuicao;
        }

        public async Task<Distribuicao> ConsultarId(string id)
        {
            var filtro = Builders<Distribuicao>.Filter.Eq(u => u.Id, id);
            return await _distribuicaoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Distribuicao>> ConsultarTodos()
        {
            return await _distribuicaoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Distribuicao?> Atualizar(Distribuicao distribuicao)
        {
            var filtro = Builders<Distribuicao>.Filter.Eq(u => u.Id, distribuicao.Id);

            var update = Builders<Distribuicao>.Update
                .Set(u => u.IdDoacao, distribuicao.IdDoacao)
                .Set(u => u.QuantidadeDestinada, distribuicao.QuantidadeDestinada)
                .Set(u => u.DataDestinada, distribuicao.DataDestinada)
                .Set(u => u.IdPessoaAtendida, distribuicao.IdPessoaAtendida);

            var resultado = await _distribuicaoCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return distribuicao;
            }

            return null;
        }

        public async Task<Distribuicao?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var distribuicao = await ConsultarId(id);
            if (distribuicao == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<Distribuicao>.Update;
            var updateDefinitions = new List<UpdateDefinition<Distribuicao>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "IdDoacao":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdDoacao, campo.Value.ToString()));
                        break;
                    case "QuantidadeDestinada":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.QuantidadeDestinada, Convert.ToInt32(campo.Value)));
                        break;
                    case "IdPessoaAtendida":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.IdPessoaAtendida, campo.Value.ToString()));
                        break;
                  
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return distribuicao;
            }

            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            var filtro = Builders<Distribuicao>.Filter.Eq(c => c.Id, id);
            
            var resultado = await _distribuicaoCollection.UpdateOneAsync(filtro, combinedUpdate);

            if (resultado.ModifiedCount > 0)
            {
                return await ConsultarId(id);
            }

            return null;
        }

        public async Task Excluir(string id)
        {
            var filtro = Builders<Distribuicao>.Filter.Eq(u => u.Id, id);
            await _distribuicaoCollection.DeleteOneAsync(filtro);
        }
    }
}
