using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TipApi.Data;
using TipApi.Models;
using TipApi.Models.DTO;
using TipApi.Services;

namespace TipApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        private readonly TipDbContext _context;

        public PredictionController(IPredictionService predictionService, TipDbContext context)
        {
            _predictionService = predictionService;
            _context = context;
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _context.PredictionLogs
                              .OrderByDescending(x => x.CreatedAt)
                              .ToListAsync();
            return Ok(logs);
        }

        [HttpPost]
        public async Task<ActionResult<TipPredictionResponseDTO>> Predict([FromBody] TipPredictionRequestDTO request)
        {

            var json1 = JsonSerializer.Serialize(request);
            Console.WriteLine("Request from react");
            Console.WriteLine(json1);

            var result = await _predictionService.PredictAsync(request);

            

            var json = JsonSerializer.Serialize(request);
            Console.WriteLine(json);

            if (result == null) {
              return   BadRequest("Prediction failed");
            }

            var log = new PredictionLog
            {
                TotalBill = request.TotalBill,
                Size = request.Size,
                Smoker = request.Smoker,
                Day = request.Day,
                Time = request.Time,
                PredictedTipPct = result.PredictedTipPct,
                PredictedTipAmount = result.PredictedTipAmount
            };

            _context.PredictionLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(result);

        }
    }
}
