using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace TipApi.Models.DTO
{
    public class TipPredictionResponseDTO
    {
        [JsonPropertyName("predictedTipPct")]
        public double PredictedTipPct { get; set; }
        [JsonPropertyName("rawPredictedTipAmount")]
        public double RawPredictedTipAmount { get; set; }
        [JsonPropertyName("adjustedPredictedTipAmount")]
        public double AdjustedPredictedTipAmount { get; set; }

        [JsonPropertyName("predictedTipAmount")]
        public double PredictedTipAmount { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
