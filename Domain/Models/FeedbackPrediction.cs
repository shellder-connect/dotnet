using Microsoft.ML.Data;

namespace Project.Models
{
    public class FeedbackPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsPositive { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
