using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IOptions<ConfigMongoDb> settings, ILogger<DatabaseService> logger)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _logger = logger;
        }

        public async Task<(bool success, string errorMessage)> ImportarJson(string collection, string jsonContent)
        {
            try
            {
                // Primeiro, verificar se é um array ou objeto único
                using var jsonDoc = JsonDocument.Parse(jsonContent);
                
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    // É um array - processar cada elemento
                    var documents = new List<BsonDocument>();
                    
                    foreach (var element in jsonDoc.RootElement.EnumerateArray())
                    {
                        var jsonString = element.GetRawText();
                        var bsonDoc = BsonDocument.Parse(jsonString);
                        documents.Add(bsonDoc);
                    }
                    
                    if (documents.Count > 0)
                    {
                        await mongoCollection.InsertManyAsync(documents);
                        _logger.LogInformation($"Importados {documents.Count} documentos para {collection}");
                        return (true, $"Importados {documents.Count} documentos com sucesso!");
                    }
                    else
                    {
                        return (false, "Array JSON está vazio");
                    }
                }
                else if (jsonDoc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    // É um objeto único
                    var bsonDoc = BsonDocument.Parse(jsonContent);
                    await mongoCollection.InsertOneAsync(bsonDoc);
                    _logger.LogInformation($"Importado 1 documento para {collection}");
                    return (true, "Documento importado com sucesso!");
                }
                else
                {
                    return (false, "JSON deve ser um objeto ou array de objetos");
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Erro ao fazer parse do JSON");
                return (false, $"Erro no formato do JSON: {jsonEx.Message}");
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao inserir no MongoDB");
                return (false, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na importação");
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string content, string errorMessage)> ExportarJson(string collection)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var documents = await mongoCollection.Find(_ => true).ToListAsync();

                if (!documents.Any())
                {
                    return (false, string.Empty, "Nenhum documento encontrado na coleção");
                }

                var json = documents.ToJson();
                _logger.LogInformation($"Exportados {documents.Count} documentos de {collection}");
                
                return (true, json, string.Empty);
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao exportar do MongoDB");
                return (false, string.Empty, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na exportação");
                return (false, string.Empty, $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, List<object> dados, string errorMessage)> ConsultarDocumentos(string collection, int limit = 10)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var documents = await mongoCollection.Find(_ => true).Limit(limit).ToListAsync();

                var dados = new List<object>();
                foreach (var doc in documents)
                {
                    // Converter BsonDocument para Dictionary para facilitar exibição
                    var dict = new Dictionary<string, object?>();
                    foreach (var element in doc.Elements)
                    {
                        dict[element.Name] = ConvertBsonValue(element.Value);
                    }
                    dados.Add(dict);
                }

                _logger.LogInformation($"Consultados {dados.Count} documentos de {collection}");
                return (true, dados, string.Empty);
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao consultar MongoDB");
                return (false, new List<object>(), $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na consulta");
                return (false, new List<object>(), $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, Dictionary<string, object> documento, string errorMessage)> BuscarDocumentoPorId(string collection, string id)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var document = await mongoCollection.Find(filter).FirstOrDefaultAsync();

                if (document == null)
                {
                    return (false, new Dictionary<string, object>(), "Documento não encontrado");
                }

                var dictNullable = new Dictionary<string, object?>();
                foreach (var element in document.Elements)
                {
                    dictNullable[element.Name] = ConvertBsonValue(element.Value);
                }

                // Convert Dictionary<string, object?> to Dictionary<string, object>
                var dict = dictNullable
                    .Where(kv => kv.Value != null)
                    .ToDictionary(kv => kv.Key, kv => kv.Value!);

                _logger.LogInformation($"Documento {id} encontrado em {collection}");
                return (true, dict, string.Empty);
            }
            catch (FormatException)
            {
                return (false, new Dictionary<string, object>(), "ID inválido");
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao buscar no MongoDB");
                return (false, new Dictionary<string, object>(), $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na busca");
                return (false, new Dictionary<string, object>(), $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage)> AtualizarDocumento(string collection, string id, Dictionary<string, object> campos)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

                var updateBuilder = Builders<BsonDocument>.Update;
                var updates = new List<UpdateDefinition<BsonDocument>>();

                foreach (var campo in campos)
                {
                    if (campo.Key != "_id") // Não permitir atualizar o _id
                    {
                        updates.Add(updateBuilder.Set(campo.Key, campo.Value));
                    }
                }

                if (!updates.Any())
                {
                    return (false, "Nenhum campo válido para atualização");
                }

                var updateDefinition = updateBuilder.Combine(updates);
                var result = await mongoCollection.UpdateOneAsync(filter, updateDefinition);

                if (result.MatchedCount == 0)
                {
                    return (false, "Documento não encontrado");
                }

                if (result.ModifiedCount == 0)
                {
                    return (false, "Nenhuma modificação foi feita");
                }

                _logger.LogInformation($"Documento {id} atualizado em {collection}");
                return (true, string.Empty);
            }
            catch (FormatException)
            {
                return (false, "ID inválido");
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao atualizar no MongoDB");
                return (false, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na atualização");
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage)> ExcluirDocumento(string collection, string id)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var result = await mongoCollection.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return (false, "Documento não encontrado");
                }

                _logger.LogInformation($"Documento {id} excluído de {collection}");
                return (true, string.Empty);
            }
            catch (FormatException)
            {
                return (false, "ID inválido");
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao excluir do MongoDB");
                return (false, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral na exclusão");
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage)> LimparColecao(string collection)
        {
            try
            {
                var mongoCollection = _database.GetCollection<BsonDocument>(collection);
                var result = await mongoCollection.DeleteManyAsync(_ => true);

                _logger.LogWarning($"Coleção {collection} completamente limpa. {result.DeletedCount} documentos removidos");
                return (true, $"{result.DeletedCount} documentos removidos");
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "Erro ao limpar coleção no MongoDB");
                return (false, $"Erro no MongoDB: {mongoEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral ao limpar coleção");
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        // Método auxiliar para converter BsonValue para object
        private object? ConvertBsonValue(BsonValue value)
        {
            if (value == null || value.IsBsonNull)
                return null;

            return value.BsonType switch
            {
                BsonType.String => value.AsString,
                BsonType.Int32 => value.AsInt32,
                BsonType.Int64 => value.AsInt64,
                BsonType.Double => value.AsDouble,
                BsonType.Boolean => value.AsBoolean,
                BsonType.DateTime => value.ToUniversalTime(),
                BsonType.ObjectId => value.AsObjectId.ToString(),
                BsonType.Null => null,
                BsonType.Array => value.AsBsonArray?.Select(ConvertBsonValue).ToList() ?? new List<object?>(),
                BsonType.Document => value.AsBsonDocument?.ToDictionary(e => e.Name, e => ConvertBsonValue(e.Value)) ?? new Dictionary<string, object?>(),
                _ => value.ToString()
            };
        }
    }
}