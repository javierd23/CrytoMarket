using CryptoMarket.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CryptoMarket.Infrastructure.Persistence;
using CryptoMarket.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoMarket.Application.Services
{
    public class SymbolService : ISymbolService
    {
        private const string CacheKey = "symbols_cache";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);

        private readonly AppDbContext? _db;
        private readonly IBinanceClient? _binanceClient;
        private readonly IMemoryCache? _cache;

        public SymbolService(
            AppDbContext db, 
            IBinanceClient binanceClient,
            IMemoryCache cache)
        {
            _db = db;
            _binanceClient = binanceClient;
            _cache = cache;
        }

        public async Task<List<Symbol>> GetSymbolsAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<Symbol>? cached))
            {
                return cached!;
            }

            var symbols = await _db.Symbols.AsNoTracking().ToListAsync();

            if (!symbols.Any())
            {
                await RefreshSymbolsAsync();
                symbols = await _db.Symbols.AsNoTracking().ToListAsync();
            }

            _cache.Set(CacheKey, symbols, CacheTtl);

            return symbols;
        }

        public async Task RefreshSymbolsAsync()
        {
            var symbolsFromBinance = await _binanceClient.GetUsdtSymbolsAsync();

            _db.Symbols.RemoveRange(_db.Symbols);
            await _db.SaveChangesAsync();

            var entities = symbolsFromBinance
                .Select(s => new Symbol { Name = s })
                .ToList();

            await _db.Symbols.AddRangeAsync(entities);
            await _db.SaveChangesAsync();

            _cache.Remove(CacheKey);
        }

    }
}
