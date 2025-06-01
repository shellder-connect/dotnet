using MongoDB.Driver;
using Project.Models;
using Project.Infrastructure.Interfaces;
using Project.Domain;
using Microsoft.Extensions.Options;

namespace Project.Repositories
{
    public class EnderecoAbrigoRepository : IEnderecoAbrigoRepository
    {
        private readonly IMongoCollection<EnderecoAbrigo> _enderecoAbrigoCollection;

        public EnderecoAbrigoRepository(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _enderecoAbrigoCollection = mongoDatabase.GetCollection<EnderecoAbrigo>(settings.Value.EnderecoAbrigoCollectionName);
        }

        public async Task<EnderecoAbrigo> Criar(EnderecoAbrigo enderecoAbrigo)
        {
            await _enderecoAbrigoCollection.InsertOneAsync(enderecoAbrigo);
            return enderecoAbrigo;
        }

        public async Task<EnderecoAbrigo> ConsultarId(string id)
        {
            var filtro = Builders<EnderecoAbrigo>.Filter.Eq(u => u.Id, id);
            return await _enderecoAbrigoCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<EnderecoAbrigo> ConsultarPorUsuarioId(string usuarioId)
        {
            return await _enderecoAbrigoCollection
                        .Find(e => e.IdAbrigo == usuarioId)
                        .FirstOrDefaultAsync(); 
        }

        public async Task<List<EnderecoAbrigo>> ConsultarTodos()
        {
            //return await _enderecoAbrigoCollection.Find(_ => true).ToListAsync();

            try
                {
                    // Filtrar apenas documentos que tenham a estrutura esperada
                    var filtro = Builders<EnderecoAbrigo>.Filter.Exists(e => e.Id);
                    return await _enderecoAbrigoCollection.Find(filtro).ToListAsync();
                }
                catch (Exception ex)
                {
                    // Log do erro para investigação
                    Console.WriteLine($"Erro ao consultar endereços: {ex.Message}");
                    throw;
                }
        }

        public async Task<EnderecoAbrigo?> Atualizar(EnderecoAbrigo enderecoAbrigo)
        {
            var filtro = Builders<EnderecoAbrigo>.Filter.Eq(u => u.Id, enderecoAbrigo.Id);

            var update = Builders<EnderecoAbrigo>.Update
                .Set(u => u.CEP, enderecoAbrigo.CEP)
                .Set(u => u.Estado, enderecoAbrigo.Estado)
                .Set(u => u.Cidade, enderecoAbrigo.Cidade)
                .Set(u => u.Bairro, enderecoAbrigo.Bairro)
                .Set(u => u.Rua, enderecoAbrigo.Rua);

            var resultado = await _enderecoAbrigoCollection.UpdateOneAsync(filtro, update);

            if (resultado.ModifiedCount > 0)
            {
                return enderecoAbrigo;
            }

            return null;
        }

        public async Task<EnderecoAbrigo?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            var enderecoAbrigo = await ConsultarId(id);
            if (enderecoAbrigo == null)
            {
                return null;
            }

            var updateDefinitionBuilder = Builders<EnderecoAbrigo>.Update;
            var updateDefinitions = new List<UpdateDefinition<EnderecoAbrigo>>();

            foreach (var campo in camposParaAtualizar)
            {
                switch (campo.Key.ToLower())
                {
                    case "cep":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.CEP, campo.Value.ToString()));
                        break;
                    case "estado":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Estado, campo.Value.ToString()));
                        break;
                    case "cidade":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Cidade, campo.Value.ToString()));
                        break;
                    case "bairro":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Bairro, campo.Value.ToString()));
                        break;
                    case "rua":
                        updateDefinitions.Add(updateDefinitionBuilder.Set(c => c.Rua, campo.Value.ToString()));
                        break;
                }
            }

            if (updateDefinitions.Count == 0)
            {
                return enderecoAbrigo;
            }

            // Combinar todas as definições de atualização
            var combinedUpdate = updateDefinitionBuilder.Combine(updateDefinitions);
            
            // Filtro para encontrar o documento
            var filtro = Builders<EnderecoAbrigo>.Filter.Eq(c => c.Id, id);
            
            // Executar a atualização
            var resultado = await _enderecoAbrigoCollection.UpdateOneAsync(filtro, combinedUpdate);

            // Retornar o endereço atualizado se a operação for bem-sucedida
            if (resultado.ModifiedCount > 0)
            {
                // Buscar o endereço atualizada
                return await ConsultarId(id);
            }

            return null;
        }


        public async Task Excluir(string id)
        {
            var filtro = Builders<EnderecoAbrigo>.Filter.Eq(u => u.Id, id);
            await _enderecoAbrigoCollection.DeleteOneAsync(filtro);
        }
    }
}
