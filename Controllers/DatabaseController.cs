
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Services;

namespace Project.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly IDatabaseService _databaseService;

        public DatabaseController(ILogger<DatabaseController> logger, IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        // GET: Database - Menu Principal do Database
        public IActionResult Index()
        {
            return View();
        }

        // GET: Database/Importar
        public IActionResult Importar()
        {
            // Pode passar uma lista de coleções disponíveis para o dropdown
            ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido" }; // Adicione suas coleções aqui
            return View();
        }

        // POST: Database/Importar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Importar(string collection, IFormFile json_file)
        {
            try
            {
                // Validações
                if (string.IsNullOrEmpty(collection))
                {
                    TempData["Message"] = "Por favor, selecione uma coleção.";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction(nameof(Importar));
                }

                if (json_file == null || json_file.Length == 0)
                {
                    TempData["Message"] = "Por favor, selecione um arquivo JSON válido.";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction(nameof(Importar));
                }

                // Processar arquivo
                string jsonContent;
                using (var reader = new StreamReader(
                    json_file.OpenReadStream(), 
                    Encoding.UTF8, 
                    detectEncodingFromByteOrderMarks: true))
                {
                    jsonContent = await reader.ReadToEndAsync();
                }

                // Remover BOM se existir
                jsonContent = jsonContent.Trim('\uFEFF', '\u200B');

                // Validar JSON
                try
                {
                    using (JsonDocument.Parse(jsonContent)) { }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "JSON inválido");
                    TempData["Message"] = $"JSON inválido: {jsonEx.Message}";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction(nameof(Importar));
                }

                // Importar para MongoDB
                var (success, errorMessage) = await _databaseService.ImportarJson(collection, jsonContent);
                
                if (success)
                {
                    TempData["Message"] = $"Dados importados com sucesso para {collection}!";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = $"Erro na importação: {errorMessage}";
                    TempData["MessageType"] = "danger";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na importação");
                TempData["Message"] = $"Erro inesperado: {ex.Message}";
                TempData["MessageType"] = "danger";
            }
            
            return RedirectToAction(nameof(Importar));
        }

        // GET: Database/ExportarDataset
        public IActionResult ExportarDataset()
        {
            return View();
        }

        // POST: Database/ExportarDataset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportarDataset(string collection_exportar)
        {
            try
            {
                // Sua lógica de exportação aqui
                ViewBag.Message = "Dados exportados com sucesso!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erro ao exportar: {ex.Message}";
            }
            
            return View();
        }

        // GET: Database/Consultar
        public IActionResult Consultar()
        {
            return View(new List<string>());
        }

        // POST: Database/Consultar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Consultar(string collection_select)
        {
            try
            {
                // Sua lógica de consulta aqui
                var dados = new List<string>
                {
                    "Documento 1: {...}",
                    "Documento 2: {...}",
                    "Documento 3: {...}"
                };
                
                return View(dados);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erro ao consultar: {ex.Message}";
                return View(new List<string>());
            }
        }

        // GET: Database/Editar
        public IActionResult Editar()
        {
            return View(new Dictionary<string, object>());
        }

        // POST: Database/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(string collection, string id, string buscar, string salvar)
        {
            if (!string.IsNullOrEmpty(buscar))
            {
                try
                {
                    // Sua lógica de busca do documento aqui
                    var documento = new Dictionary<string, object>
                    {
                        {"nome", "João Silva"},
                        {"email", "joao@email.com"},
                        {"telefone", "(11) 99999-9999"}
                    };
                    
                    ViewBag.Collection = collection;
                    ViewBag.Id = id;
                    
                    return View(documento);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Erro ao buscar documento: {ex.Message}";
                }
            }
            else if (!string.IsNullOrEmpty(salvar))
            {
                try
                {
                    // Sua lógica de salvamento aqui
                    ViewBag.Message = "Documento atualizado com sucesso!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Erro ao salvar: {ex.Message}";
                }
            }
            
            ViewBag.Collection = collection;
            ViewBag.Id = id;
            
            return View(new Dictionary<string, object>());
        }

        // GET: Database/Excluir
        public IActionResult Excluir()
        {
            return View();
        }

        // POST: Database/Excluir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(string collection_excluir, string id_excluir)
        {
            try
            {
                // Sua lógica de exclusão aqui
                ViewBag.Message = "Documento excluído com sucesso!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erro ao excluir: {ex.Message}";
            }
            
            return View();
        }
    }
}