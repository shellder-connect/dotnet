using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;

        public FeedbackService(IFeedbackRepository feedbackRepository, ISentimentAnalysisService sentimentAnalysisService)
        {
            _feedbackRepository = feedbackRepository;
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public async Task<Feedback> Criar(Feedback feedback)
        {
            // Realiza a análise de sentimento se houver comentário
            if (!string.IsNullOrWhiteSpace(feedback.Comentario))
            {
                var prediction = _sentimentAnalysisService.Predict(feedback.Comentario);
                feedback.Sentimento = prediction.IsPositive ? "Positivo" : "Negativo";
            }
            else
            {
                feedback.Sentimento = "Indefinido";
            }

            return await _feedbackRepository.Criar(feedback);
        }

        public async Task<List<Feedback>> ConsultarTodos()
        {
            var feedbacks = await _feedbackRepository.ConsultarTodos();
            return feedbacks.ToList(); 
        }

        public async Task<Feedback> ConsultarId(string id)
        {
            return await _feedbackRepository.ConsultarId(id);
        }


        public async Task<Feedback?> Atualizar(Feedback feedback)
        {
            return await _feedbackRepository.Atualizar(feedback);
        }

        public async Task<Feedback?> AtualizarParcial(string id, Dictionary<string, object> camposParaAtualizar)
        {
            return await _feedbackRepository.AtualizarParcial(id, camposParaAtualizar);
        }

        public async Task Excluir(string id)
        {
            await _feedbackRepository.Excluir(id);
        }
    }
}
