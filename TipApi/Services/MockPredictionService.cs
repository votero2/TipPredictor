using TipApi.Data;
using TipApi.Models;
using TipApi.Models.DTO;

namespace TipApi.Services
{
    public class MockPredictionService : IPredictionService
    {
        private readonly TipDbContext _context;
        public MockPredictionService(TipDbContext context)
        {
            _context = context;
        }
        public async Task<TipPredictionResponseDTO> PredictAsync(TipPredictionRequestDTO request)
        {
            double basePct = 0.15;

            if (request.Smoker.Equals("Yes", StringComparison.OrdinalIgnoreCase)) 
            {
                basePct += 0.01;
            }

            if (request.Time.Equals("Dinner", StringComparison.OrdinalIgnoreCase))
            {
                basePct += 0.01;
            }

            if (request.Size >= 5)
            {
                basePct -= 0.02;
            }
            if (request.TotalBill < 15)
            {
                basePct += 0.02;
            }
            if (basePct < 0.05)
            {
                basePct = 0.05;
            }

            var predictedAmount = Math.Round(request.TotalBill * basePct, 2);
            var predictedPct = Math.Round(basePct, 4);

            var response = new TipPredictionResponseDTO
            {
                PredictedTipPct = predictedPct,
                PredictedTipAmount = predictedAmount,
                Message = "Mock prediction generated successfully."
            };

            var log = new PredictionLog
            {
                TotalBill = request.TotalBill,
                Size = request.Size,
                Day = request.Day,
                Time = request.Time,
                Smoker = request.Smoker,
                PredictedTipPct = predictedPct,
                PredictedTipAmount = predictedAmount,
                CreatedAt = DateTime.Now,
            };

            _context.PredictionLogs.Add(log);
            await _context.SaveChangesAsync();

            return response;
        }
    }
}
