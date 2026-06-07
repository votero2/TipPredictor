using System.Text.Json.Serialization;

namespace TipApi.Models.DTO
{
    public class TipPredictionRequestDTO
    {
        public double TotalBill { get; set; }
        public int Size { get; set; }
        public string Smoker { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }
}
