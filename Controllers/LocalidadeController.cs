using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Project.Models;
using Microsoft.Extensions.Options;
using Project.Domain;
using MongoDB.Bson;

namespace Project.Controllers
{
    public class LocalidadeController : Controller
    {
        private readonly IMongoCollection<Localidade> _localidadeCollection;
        private readonly IMongoCollection<BsonDocument> _bsonCollection;

        public LocalidadeController(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _localidadeCollection = mongoDatabase.GetCollection<Localidade>("t_localidade");
            _bsonCollection = mongoDatabase.GetCollection<BsonDocument>("t_localidade"); // Para debug
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // DEBUG 1: Verificar documentos como BsonDocument (raw)
                var bsonDocs = await _bsonCollection.Find(_ => true).Limit(3).ToListAsync();
                Console.WriteLine($"\n=== DEBUG BSON DOCUMENTS ===");
                Console.WriteLine($"Total de documentos BSON: {bsonDocs.Count}");
                
                if (bsonDocs.Any())
                {
                    var primeiro = bsonDocs.First();
                    Console.WriteLine($"Primeiro documento BSON:");
                    Console.WriteLine(primeiro.ToJson());
                    
                    Console.WriteLine($"\nCampos disponíveis:");
                    foreach (var element in primeiro.Elements)
                    {
                        Console.WriteLine($"- {element.Name}: {element.Value}");
                    }
                }
                
                var localidades = await _localidadeCollection
                    .Find(_ => true)
                    .ToListAsync();

                Console.WriteLine($"Localidades encontradas: {localidades.Count}");

                return View(localidades);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                TempData["Message"] = $"Erro ao carregar dados: {ex.Message}";
                TempData["MessageType"] = "danger";
                return View(new List<Localidade>());
            }
        }
    }
}