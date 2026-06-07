using Microsoft.EntityFrameworkCore;
using TipApi.Models;
using TipApi.Models.DTO;

namespace TipApi.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly HttpClient _httpClient;

        public PredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TipPredictionResponseDTO?> PredictAsync(TipPredictionRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync(
               "http://127.0.0.1:8000/predict",
               request
               );

            var responseText = await response.Content.ReadAsStringAsync();

            

            if (!response.IsSuccessStatusCode) { 
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseText);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TipPredictionResponseDTO>();


            return result;
        }
    }
}
