using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TipApi.Data;
using TipApi.Models;

namespace TipApi.Services
{
    public class CsvImportService
    {
        private readonly TipDbContext _context;

        public CsvImportService(TipDbContext context)
        {
            _context = context;
        }


        public async Task<int> ImportTipsAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CSV file not found.", filePath);
            }

            //Avoid importing twice
            if(await _context.Tips.AnyAsync())
            {
                return 0;
            }

            var lines = await File.ReadAllLinesAsync(filePath);
            var records = new List<TipRecord>();

            foreach (var line in lines.Skip(1)) //Skip header
            {
                var parts = line.Split(',');

                if(parts.Length < 7)
                {
                    continue;
                }

                if (!double.TryParse(parts[0],NumberStyles.Any, CultureInfo.InvariantCulture, out var totalBill))
                {
                    continue;
                }

                if (!double.TryParse(parts[1],NumberStyles.Any, CultureInfo.InvariantCulture, out var tip))
                {
                    continue;
                }


                if (!int.TryParse(parts[6], out var size))
                {
                    continue;
                }


                records.Add(new TipRecord
                {
                    TotalBill = totalBill,
                    Tip = tip,
                    Sex = parts[2].Trim(),
                    Smoker = parts[3].Trim(),
                    Day = parts[4].Trim(),
                    Time = parts[5].Trim(),
                    Size = size
                });
               
            }

            _context.Tips.AddRange(records);
            await _context.SaveChangesAsync();

            return records.Count;
        }
    }
}
