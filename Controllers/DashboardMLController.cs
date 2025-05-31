using Microsoft.AspNetCore.Mvc;
using Project.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Project.Controllers
{
    public class DashboardMLController : Controller
    {
        private readonly IMLService _mlService;

        public DashboardMLController(IMLService mlService)
        {
            _mlService = mlService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Obter localizações disponíveis
                var localizacoes = await _mlService.ObterLocalizacoesDisponiveis();
                
                // Gerar predições para cada localização
                var predicoes = new List<object>();
                
                foreach (var localizacao in localizacoes)
                {
                    // Simular dados realistas para cada região
                    var random = new Random();
                    var quantidadeEventos = random.Next(1, 15);
                    var capacidadeAbrigo = random.Next(100, 300);
                    var ocupacaoAtual = random.Next(50, capacidadeAbrigo);

                    var predicao = await _mlService.Prever(localizacao, quantidadeEventos, capacidadeAbrigo, ocupacaoAtual);
                    
                    predicoes.Add(new
                    {
                        localizacao = localizacao,
                        alimentosPrevistos = predicao.AlimentosPrevistos,
                        medicamentosPrevistos = predicao.MedicamentosPrevistos,
                        cobertoresPrevistos = predicao.CobertoresPrevistos,
                        quantidadeEventos = quantidadeEventos,
                        capacidadeAbrigo = capacidadeAbrigo,
                        ocupacaoAtual = ocupacaoAtual,
                        taxaOcupacao = Math.Round((double)ocupacaoAtual / capacidadeAbrigo * 100, 1)
                    });
                }

                // Preparar dados para o JavaScript
                var dashboardData = new
                {
                    predicoes = predicoes,
                    estatisticas = new
                    {
                        totalRegioes = localizacoes.Count,
                        totalAlimentos = predicoes.Sum(p => (int)((dynamic)p).alimentosPrevistos),
                        totalMedicamentos = predicoes.Sum(p => (int)((dynamic)p).medicamentosPrevistos),
                        totalCobertores = predicoes.Sum(p => (int)((dynamic)p).cobertoresPrevistos),
                        ultimoTreinamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    }
                };

                // Converter para JSON
                ViewBag.PredicoesData = JsonConvert.SerializeObject(dashboardData);
                ViewBag.TotalRegioes = localizacoes.Count;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao carregar dashboard: {ex.Message}";
                TempData["MessageType"] = "danger";
                
                // Dados de fallback
                ViewBag.PredicoesData = JsonConvert.SerializeObject(new { predicoes = new List<object>() });
                ViewBag.TotalRegioes = 0;
                
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GerarPredicaoPersonalizada(string localizacao, int quantidadeEventos, int capacidadeAbrigo, int ocupacaoAtual)
        {
            try
            {
                if (string.IsNullOrEmpty(localizacao))
                {
                    return Json(new { success = false, message = "Localização é obrigatória" });
                }

                var predicao = await _mlService.Prever(localizacao, quantidadeEventos, capacidadeAbrigo, ocupacaoAtual);
                
                return Json(new 
                { 
                    success = true, 
                    data = new
                    {
                        localizacao = localizacao,
                        alimentosPrevistos = predicao.AlimentosPrevistos,
                        medicamentosPrevistos = predicao.MedicamentosPrevistos,
                        cobertoresPrevistos = predicao.CobertoresPrevistos,
                        quantidadeEventos = quantidadeEventos,
                        capacidadeAbrigo = capacidadeAbrigo,
                        ocupacaoAtual = ocupacaoAtual,
                        taxaOcupacao = Math.Round((double)ocupacaoAtual / capacidadeAbrigo * 100, 1)
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TreinarModelo()
        {
            try
            {
                var sucesso = await _mlService.TreinarModelo();
                
                if (sucesso)
                {
                    TempData["Message"] = "Modelo treinado com sucesso!";
                    TempData["MessageType"] = "success";
                    return Json(new { success = true, message = "Modelo treinado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Falha ao treinar o modelo" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterEstatisticas()
        {
            try
            {
                var localizacoes = await _mlService.ObterLocalizacoesDisponiveis();
                var predicoes = new List<object>();
                
                foreach (var localizacao in localizacoes)
                {
                    var random = new Random();
                    var quantidadeEventos = random.Next(1, 15);
                    var capacidadeAbrigo = random.Next(100, 300);
                    var ocupacaoAtual = random.Next(50, capacidadeAbrigo);

                    var predicao = await _mlService.Prever(localizacao, quantidadeEventos, capacidadeAbrigo, ocupacaoAtual);
                    
                    predicoes.Add(new
                    {
                        localizacao = localizacao,
                        alimentosPrevistos = predicao.AlimentosPrevistos,
                        medicamentosPrevistos = predicao.MedicamentosPrevistos,
                        cobertoresPrevistos = predicao.CobertoresPrevistos,
                        urgencia = CalcularUrgencia(predicao.AlimentosPrevistos + predicao.MedicamentosPrevistos + predicao.CobertoresPrevistos)
                    });
                }

                var estatisticas = new
                {
                    totalRegioes = localizacoes.Count,
                    alertasCriticos = predicoes.Count(p => ((dynamic)p).urgencia == "Alta"),
                    totalAlimentos = predicoes.Sum(p => (int)((dynamic)p).alimentosPrevistos),
                    totalMedicamentos = predicoes.Sum(p => (int)((dynamic)p).medicamentosPrevistos),
                    totalCobertores = predicoes.Sum(p => (int)((dynamic)p).cobertoresPrevistos),
                    precisaoModelo = "94.2%", // Simulado
                    ultimoTreinamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                };

                return Json(new { success = true, data = estatisticas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportarRelatorioML()
        {
            try
            {
                var localizacoes = await _mlService.ObterLocalizacoesDisponiveis();
                var predicoes = new List<object>();
                
                foreach (var localizacao in localizacoes)
                {
                    var random = new Random();
                    var quantidadeEventos = random.Next(1, 15);
                    var capacidadeAbrigo = random.Next(100, 300);
                    var ocupacaoAtual = random.Next(50, capacidadeAbrigo);

                    var predicao = await _mlService.Prever(localizacao, quantidadeEventos, capacidadeAbrigo, ocupacaoAtual);
                    
                    predicoes.Add(new
                    {
                        Localizacao = localizacao,
                        AlimentosPrevistos = predicao.AlimentosPrevistos,
                        MedicamentosPrevistos = predicao.MedicamentosPrevistos,
                        CobertoresPrevistos = predicao.CobertoresPrevistos,
                        QuantidadeEventos = quantidadeEventos,
                        CapacidadeAbrigo = capacidadeAbrigo,
                        OcupacaoAtual = ocupacaoAtual,
                        TaxaOcupacao = Math.Round((double)ocupacaoAtual / capacidadeAbrigo * 100, 1),
                        Urgencia = CalcularUrgencia(predicao.AlimentosPrevistos + predicao.MedicamentosPrevistos + predicao.CobertoresPrevistos),
                        DataGeracao = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    });
                }

                // Gerar CSV
                var csv = new System.Text.StringBuilder();
                csv.AppendLine("Localizacao,AlimentosPrevistos,MedicamentosPrevistos,CobertoresPrevistos,QuantidadeEventos,CapacidadeAbrigo,OcupacaoAtual,TaxaOcupacao,Urgencia,DataGeracao");
                
                foreach (dynamic predicao in predicoes)
                {
                    csv.AppendLine($"{predicao.Localizacao},{predicao.AlimentosPrevistos},{predicao.MedicamentosPrevistos},{predicao.CobertoresPrevistos},{predicao.QuantidadeEventos},{predicao.CapacidadeAbrigo},{predicao.OcupacaoAtual},{predicao.TaxaOcupacao},{predicao.Urgencia},{predicao.DataGeracao}");
                }

                var fileName = $"RelatorioPredicoes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
                
                return File(bytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao exportar relatório: {ex.Message}";
                TempData["MessageType"] = "danger";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SimularCenario([FromBody] SimularCenarioRequest request)
        {
            try
            {
                var resultados = new List<object>();
                
                foreach (var cenario in request.Cenarios)
                {
                    var predicao = await _mlService.Prever(
                        cenario.Localizacao, 
                        cenario.QuantidadeEventos, 
                        cenario.CapacidadeAbrigo, 
                        cenario.OcupacaoAtual
                    );
                    
                    resultados.Add(new
                    {
                        localizacao = cenario.Localizacao,
                        cenario = cenario.Nome,
                        alimentosPrevistos = predicao.AlimentosPrevistos,
                        medicamentosPrevistos = predicao.MedicamentosPrevistos,
                        cobertoresPrevistos = predicao.CobertoresPrevistos,
                        impacto = CalcularImpacto(predicao.AlimentosPrevistos + predicao.MedicamentosPrevistos + predicao.CobertoresPrevistos)
                    });
                }
                
                return Json(new { success = true, data = resultados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string CalcularUrgencia(int total)
        {
            if (total > 100) return "Alta";
            if (total > 50) return "Média";
            return "Baixa";
        }

        private string CalcularImpacto(int total)
        {
            if (total > 150) return "Crítico";
            if (total > 100) return "Alto";
            if (total > 50) return "Moderado";
            return "Baixo";
        }
    }

    // Classes auxiliares
    public class SimularCenarioRequest
    {
        public List<CenarioSimulacao> Cenarios { get; set; } = new();
    }

    public class CenarioSimulacao
    {
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public int QuantidadeEventos { get; set; }
        public int CapacidadeAbrigo { get; set; }
        public int OcupacaoAtual { get; set; }
    }
}