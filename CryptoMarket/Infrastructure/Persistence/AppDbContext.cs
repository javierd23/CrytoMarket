using Microsoft.EntityFrameworkCore;
using CryptoMarket.Domain.Entities;


namespace CryptoMarket.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Symbol> Symbols => Set<Symbol>();
        public DbSet<Price> Prices => Set<Price>();
        public DbSet<Candle> Candles => Set<Candle>();
    }
}
