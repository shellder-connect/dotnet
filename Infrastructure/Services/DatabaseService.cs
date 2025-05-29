using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace Project.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IMongoClient mongoClient, ILogger<DatabaseService> logger)
        {
            _database = mongoClient.GetDatabase("Global-Solution");
            _logger = logger;
        }

        public object GetCollection<T>(string collection)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool success, string errorMessage)> ImportarJson(string collectionName, string jsonContent)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                // Tentar parsear o JSON primeiro para verificar se é válido
                var documents = JsonSerializer.Deserialize<BsonDocument[]>(jsonContent);
                
                if (documents == null || documents.Length == 0)
                {
                    return (false, "O arquivo JSON está vazio ou não contém dados válidos");
                }

                await collection.InsertManyAsync(documents);
                return (true, "");
            }
            catch (JsonException jsonEx)
            {
                return (false, $"Erro no formato do JSON: {jsonEx.Message}");
            }
            catch (MongoBulkWriteException bulkEx)
            {
                return (false, $"Erro ao escrever no MongoDB: {bulkEx.WriteErrors.FirstOrDefault()?.Message}");
            }
            catch (MongoException mongoEx)
            {
                return (false, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao importar JSON para a coleção {Collection}", collectionName);
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }
    }
}