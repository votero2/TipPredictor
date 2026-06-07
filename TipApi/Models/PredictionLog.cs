using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipApi.Models
{
    public class PredictionLog
    {
        public int Id { get; set; }
        public Guid RequestId { get; set; } = Guid.NewGuid();
        public double TotalBill { get; set; }
        public int Size { get; set; }
        public string Smoker { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public double PredictedTipPct { get; set; }
        public double PredictedTipAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
