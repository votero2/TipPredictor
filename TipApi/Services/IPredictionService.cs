using TipApi.Models.DTO;

namespace TipApi.Services
{
    public interface IPredictionService
    {
        Task<TipPredictionResponseDTO?> PredictAsync(TipPredictionRequestDTO request);
    }
}
