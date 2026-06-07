using Microsoft.EntityFrameworkCore;
using TipApi.Models;

namespace TipApi.Data
{
    public class TipDbContext : DbContext
    {
        public TipDbContext(DbContextOptions<TipDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<TipRecord> Tips => Set<TipRecord>();
        public DbSet<PredictionLog> PredictionLogs => Set<PredictionLog>();
    }
}
