using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Project.Infrastructure.Interfaces;
using Project.Models;

namespace Project.Application.Services
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<FeedbackData, FeedbackPrediction> _predictionEngine;

        public SentimentAnalysisService()
        {
            _mlContext = new MLContext();

            var baseDir = AppContext.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
            var dataPath = Path.Combine(projectRoot, "feedback_data.csv");

            if (!File.Exists(dataPath))
                throw new FileNotFoundException("Arquivo de dados para treino não encontrado.", dataPath);

            var dataView = _mlContext.Data.LoadFromTextFile<FeedbackInput>(
                dataPath,
                separatorChar: ',',
                hasHeader: true);

            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features", inputColumnName: nameof(FeedbackInput.Comment));

            var trainer = _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            _model = trainingPipeline.Fit(dataView);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<FeedbackData, FeedbackPrediction>(_model);
        }

        public FeedbackPrediction Predict(string comment)
        {
            var input = new FeedbackData { Comment = comment };
            return _predictionEngine.Predict(input);
        }

        private class FeedbackInput
        {
            [LoadColumn(0)]
            public string? Comment { get; set; }

            [LoadColumn(1)]
            public bool Label { get; set; }
        }
    }
}
