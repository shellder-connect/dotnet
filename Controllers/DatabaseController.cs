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
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" }; // Adicione suas coleções aqui
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
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                if (json_file == null || json_file.Length == 0)
                {
                    TempData["Message"] = "Por favor, selecione um arquivo JSON válido.";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                // Verificar extensão do arquivo
                if (!Path.GetExtension(json_file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Message"] = "Por favor, selecione um arquivo com extensão .json";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                // Verificar tamanho do arquivo (máximo 10MB)
                if (json_file.Length > 10 * 1024 * 1024)
                {
                    TempData["Message"] = "O arquivo é muito grande. Máximo permitido: 10MB.";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
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

                // Limpar BOM e caracteres especiais
                jsonContent = jsonContent.Trim('\uFEFF', '\u200B', '\r', '\n', ' ', '\t');

                // Validar se não está vazio
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    TempData["Message"] = "O arquivo JSON está vazio.";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                // Validar estrutura JSON
                try
                {
                    using var jsonDoc = JsonDocument.Parse(jsonContent);
                    
                    // Verificar se é array ou objeto
                    if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        var count = jsonDoc.RootElement.GetArrayLength();
                        _logger.LogInformation($"JSON válido detectado: Array com {count} elementos para importar em {collection}");
                        
                        if (count == 0)
                        {
                            TempData["Message"] = "O array JSON está vazio.";
                            TempData["MessageType"] = "warning";
                            ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                            return View();
                        }
                    }
                    else if (jsonDoc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        _logger.LogInformation($"JSON válido detectado: Objeto único para importar em {collection}");
                    }
                    else
                    {
                        TempData["Message"] = "JSON deve conter um objeto ou array de objetos.";
                        TempData["MessageType"] = "danger";
                        ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                        return View();
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "JSON inválido no upload para {Collection}", collection);
                    TempData["Message"] = $"JSON inválido: {jsonEx.Message}";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                // Importar para MongoDB
                var (success, message) = await _databaseService.ImportarJson(collection, jsonContent);
                
                if (success)
                {
                    _logger.LogInformation("Importação bem-sucedida para {Collection}: {Message}", collection, message);
                    TempData["Message"] = $"✅ {message}";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    _logger.LogWarning("Falha na importação para {Collection}: {ErrorMessage}", collection, message);
                    TempData["Message"] = $"❌ Erro na importação: {message}";
                    TempData["MessageType"] = "danger";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado na importação para {Collection}", collection ?? "unknown");
                TempData["Message"] = $"❌ Erro inesperado: {ex.Message}";
                TempData["MessageType"] = "danger";
            }
            
            // Recarregar as collections para o dropdown
            ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
            return View();
        }

        // GET: Database/ExportarDataset
        public IActionResult ExportarDataset()
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            return View();
        }

        // POST: Database/ExportarDataset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportarDataset(string collection_exportar)
        {
            try
            {
                if (string.IsNullOrEmpty(collection_exportar))
                {
                    TempData["Message"] = "Por favor, selecione uma coleção para exportar.";
                    TempData["MessageType"] = "danger";
                    ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
                    return View();
                }

                // Chamar o serviço de exportação (implementar no DatabaseService)
                var (success, content, errorMessage) = await _databaseService.ExportarJson(collection_exportar);
                
                if (success && !string.IsNullOrEmpty(content))
                {
                    var fileName = $"{collection_exportar}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    var contentBytes = Encoding.UTF8.GetBytes(content);
                    
                    return File(contentBytes, "application/json", fileName);
                }
                else
                {
                    TempData["Message"] = $"Erro ao exportar: {errorMessage}";
                    TempData["MessageType"] = "danger";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na exportação de {Collection}", collection_exportar ?? "unknown");
                TempData["Message"] = $"Erro inesperado: {ex.Message}";
                TempData["MessageType"] = "danger";
            }
            
            ViewBag.Collections = new List<string> { "t_usuario", "t_produto", "t_pedido", "t_localidade", "t_feedback" };
            return View();
        }

        // GET: Database/Consultar
        public IActionResult Consultar()
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            return View(new List<object>());
        }

        // POST: Database/Consultar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Consultar(string collection_select, int limit = 5)
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };

            try
            {
                if (string.IsNullOrEmpty(collection_select))
                {
                    TempData["Message"] = "Por favor, selecione uma coleção.";
                    TempData["MessageType"] = "danger";
                    return View(new List<object>());
                }

                // Limitar entre 1 e 20 registros para evitar travamento
                limit = Math.Max(1, Math.Min(20, limit));

                _logger.LogInformation($"Consultando {limit} documentos da coleção {collection_select}");

                var (success, dados, errorMessage) = await _databaseService.ConsultarDocumentos(collection_select, limit);

                if (success)
                {
                    ViewBag.Collection = collection_select;
                    ViewBag.Count = dados?.Count ?? 0;
                    ViewBag.Limit = limit;

                    _logger.LogInformation($"Consulta bem-sucedida: {dados?.Count ?? 0} documentos retornados");
                    return View(dados ?? new List<object>());
                }
                else
                {
                    _logger.LogWarning($"Erro na consulta: {errorMessage}");
                    TempData["Message"] = $"Erro ao consultar: {errorMessage}";
                    TempData["MessageType"] = "danger";
                    return View(new List<object>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na consulta de {Collection}", collection_select ?? "unknown");
                TempData["Message"] = $"Erro inesperado: {ex.Message}";
                TempData["MessageType"] = "danger";
                return View(new List<object>());
            }
        }

        // GET: Database/Editar
        public IActionResult Editar()
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            return View(new Dictionary<string, object>());
        }

        // POST: Database/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(string collection, string id, string buscar, string salvar, Dictionary<string, string> campos)
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            ViewBag.Collection = collection;
            ViewBag.Id = id;

            if (!string.IsNullOrEmpty(buscar))
            {
                try
                {
                    if (string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(id))
                    {
                        TempData["Message"] = "Coleção e ID são obrigatórios para buscar.";
                        TempData["MessageType"] = "danger";
                        return View(new Dictionary<string, object>());
                    }

                    var (success, documento, errorMessage) = await _databaseService.BuscarDocumentoPorId(collection, id);
                    
                    if (success && documento != null)
                    {
                        return View(documento);
                    }
                    else
                    {
                        TempData["Message"] = $"Documento não encontrado: {errorMessage}";
                        TempData["MessageType"] = "warning";
                        return View(new Dictionary<string, object>());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao buscar documento {Id} em {Collection}", id, collection);
                    TempData["Message"] = $"Erro ao buscar documento: {ex.Message}";
                    TempData["MessageType"] = "danger";
                    return View(new Dictionary<string, object>());
                }
            }
            else if (!string.IsNullOrEmpty(salvar))
            {
                try
                {
                    if (string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(id))
                    {
                        TempData["Message"] = "Coleção e ID são obrigatórios para salvar.";
                        TempData["MessageType"] = "danger";
                        return View(new Dictionary<string, object>());
                    }

                    if (campos == null || !campos.Any())
                    {
                        TempData["Message"] = "Nenhum campo foi enviado para atualização.";
                        TempData["MessageType"] = "warning";
                        return View(new Dictionary<string, object>());
                    }

                    // Converter para formato adequado para o MongoDB
                    var camposParaAtualizar = campos.Where(c => !string.IsNullOrEmpty(c.Value))
                                                   .ToDictionary(c => c.Key, c => (object)c.Value);

                    var (success, errorMessage) = await _databaseService.AtualizarDocumento(collection, id, camposParaAtualizar);
                    
                    if (success)
                    {
                        TempData["Message"] = "✅ Documento atualizado com sucesso!";
                        TempData["MessageType"] = "success";
                    }
                    else
                    {
                        TempData["Message"] = $"Erro ao salvar: {errorMessage}";
                        TempData["MessageType"] = "danger";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar documento {Id} em {Collection}", id, collection);
                    TempData["Message"] = $"Erro ao salvar: {ex.Message}";
                    TempData["MessageType"] = "danger";
                }
            }
            
            return View(new Dictionary<string, object>());
        }

        // GET: Database/Excluir
        public IActionResult Excluir()
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            return View();
        }

        // POST: Database/Excluir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(string collection_excluir, string id_excluir)
        {
            ViewBag.Collections = new List<string> { "t_usuario", "t_tipo_usuario", "t_categoria" };
            
            try
            {
                if (string.IsNullOrEmpty(collection_excluir) || string.IsNullOrEmpty(id_excluir))
                {
                    TempData["Message"] = "Coleção e ID são obrigatórios para exclusão.";
                    TempData["MessageType"] = "danger";
                    return View();
                }

                var (success, errorMessage) = await _databaseService.ExcluirDocumento(collection_excluir, id_excluir);
                
                if (success)
                {
                    TempData["Message"] = "✅ Documento excluído com sucesso!";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = $"Erro ao excluir: {errorMessage}";
                    TempData["MessageType"] = "danger";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir documento {Id} de {Collection}", id_excluir, collection_excluir);
                TempData["Message"] = $"Erro inesperado: {ex.Message}";
                TempData["MessageType"] = "danger";
            }
            
            return View();
        }

        // Método adicional para limpar coleção completa (cuidado!)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LimparColecao(string collection_limpar)
        {
            try
            {
                if (string.IsNullOrEmpty(collection_limpar))
                {
                    return Json(new { success = false, message = "Coleção não especificada." });
                }

                var (success, errorMessage) = await _databaseService.LimparColecao(collection_limpar);
                
                if (success)
                {
                    _logger.LogWarning("Coleção {Collection} foi completamente limpa", collection_limpar);
                    return Json(new { success = true, message = $"Coleção {collection_limpar} limpa com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao limpar coleção {Collection}", collection_limpar);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}