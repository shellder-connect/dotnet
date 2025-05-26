using Project.Models;

namespace Project.Infrastructure.Interfaces
{
    public interface ISentimentAnalysisService
    {
        FeedbackPrediction Predict(string comment);
    }
}
