using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Project.Models;
using Microsoft.Extensions.Options;
using Project.Domain;

namespace Project.Controllers
{
    public class LocalidadeController : Controller
    {
        private readonly IMongoCollection<Localidade> _localidadeCollection;

        public LocalidadeController(IOptions<ConfigMongoDb> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            
            _localidadeCollection = mongoDatabase.GetCollection<Localidade>("t_localidade");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var localidades = await _localidadeCollection
                    .Find(_ => true)
                    .ToListAsync();

                return View(localidades);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao carregar dados: {ex.Message}";
                TempData["MessageType"] = "danger";
                return View(new List<Localidade>());
            }
        }
    }
}