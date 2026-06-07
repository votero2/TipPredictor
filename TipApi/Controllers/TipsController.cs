using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TipApi.Data;
using TipApi.Services;

namespace TipApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipsController : ControllerBase
    {
        private readonly CsvImportService _cvsImportService;
        private readonly TipDbContext _context;

        public TipsController(CsvImportService csvImportService,TipDbContext context)
        {
            _cvsImportService = csvImportService;
            _context = context;
        }


        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles", "tips.csv");
            var count = await _cvsImportService.ImportTipsAsync(filePath);

            return Ok(new
            {
                imported = count,
                message = count == 0 ? "No records imported. Data may already exist." : "Import completed"
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tips = await _context.Tips
                       .OrderBy(t => t.Id)
                       .Take(20)
                       .ToListAsync();
            return Ok(tips);
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var count = await _context.Tips.CountAsync();
            return Ok(new { count });
        }
    }
}
