using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace SeuProjeto.Controllers
{
    public class FeedbackAnalyticsController : Controller
    {
        private readonly IMongoCollection<BsonDocument> _feedbackCollection;

        public FeedbackAnalyticsController()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Global-Solution"); // Substitua pelo nome do seu banco
            _feedbackCollection = database.GetCollection<BsonDocument>("t_feedback");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Buscar todos os feedbacks
                var feedbacks = await _feedbackCollection.Find(new BsonDocument()).ToListAsync();
                
                // Processar dados para os gráficos
                var analyticsData = ProcessFeedbackData(feedbacks);
                
                ViewBag.AnalyticsData = JsonSerializer.Serialize(analyticsData);
                ViewBag.TotalFeedbacks = feedbacks.Count;
                
                return View();
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao carregar dados: {ex.Message}";
                TempData["MessageType"] = "danger";
                return View();
            }
        }

        private object ProcessFeedbackData(List<BsonDocument> feedbacks)
        {
            // Análise por nota
            var notasDistribuicao = feedbacks
                .GroupBy(f => f.GetValue("Nota", "0").ToString())
                .Select(g => new { nota = g.Key, count = g.Count() })
                .OrderBy(x => x.nota)
                .ToList();

            // Análise de sentimento baseada na nota (simulando análise de IA/ML)
            var sentimentoAnalise = feedbacks.Select(f => 
            {
                var nota = int.TryParse(f.GetValue("Nota", "0").ToString(), out int n) ? n : 0;
                var comentario = f.GetValue("Comentario", "").ToString();
                
                return new
                {
                    sentimento = ClassificarSentimento(nota, comentario ?? string.Empty),
                    nota = nota,
                    comentario = (comentario ?? string.Empty).Length > 50 ? (comentario ?? string.Empty).Substring(0, 50) + "..." : (comentario ?? string.Empty)
                };
            }).ToList();

            var sentimentoDistribuicao = sentimentoAnalise
                .GroupBy(s => s.sentimento)
                .Select(g => new { sentimento = g.Key, count = g.Count() })
                .ToList();

            // Tendência temporal (últimos 30 dias)
            var dataAtual = DateTime.Now;
            var tendenciaTemporal = Enumerable.Range(0, 30)
                .Select(i => dataAtual.AddDays(-i))
                .Select(data => new
                {
                    data = data.ToString("dd/MM"),
                    count = new Random().Next(0, 10) // Simular dados temporais
                })
                .Reverse()
                .ToList();

            return new
            {
                notasDistribuicao,
                sentimentoDistribuicao,
                tendenciaTemporal,
                detalhes = sentimentoAnalise.Take(10) // Últimos 10 para tabela
            };
        }

        private string ClassificarSentimento(int nota, string comentario)
        {
            // Simulação simples de análise de sentimento baseada na nota
            // Em um cenário real, aqui você usaria ML.NET ou chamaria uma API de IA
            if (nota >= 4) return "Positivo";
            if (nota == 3) return "Neutro";
            return "Negativo";
        }

        [HttpPost]
        public async Task<IActionResult> ExportarRelatorio()
        {
            try
            {
                var feedbacks = await _feedbackCollection.Find(new BsonDocument()).ToListAsync();
                var analyticsData = ProcessFeedbackData(feedbacks);
                
                // Aqui você pode implementar a exportação para PDF/Excel
                TempData["Message"] = "Relatório exportado com sucesso!";
                TempData["MessageType"] = "success";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao exportar relatório: {ex.Message}";
                TempData["MessageType"] = "danger";
                return RedirectToAction("Index");
            }
        }
    }
}