using CryptoMarket.Application.Interfaces;
using CryptoMarket.Domain.Entities;
using CryptoMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoMarket.Application.Services
{
    public class PriceService : IPriceService
    {
        private const string CachePrefix = "price_";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

        private readonly IBinanceClient _binanceClient;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;

        public PriceService(
            IBinanceClient binanceClient,
            IMemoryCache cache,
            AppDbContext db)
        {
            _binanceClient = binanceClient;
            _cache = cache;
            _db = db;
        }

        public async Task<List<Price>> GetLatestPricesAsync(IEnumerable<string> symbols)
        {
            var symbolList = symbols.Distinct().ToList();
            var result = new List<Price>();
            var symbolsToFetch = new List<string>();

            foreach (var symbol in symbolList)
            {
                if (_cache.TryGetValue(CachePrefix + symbol, out Price? cached))
                {
                    result.Add(cached!);
                }
                else
                {
                    symbolsToFetch.Add(symbol);
                }
            }

            if (symbolsToFetch.Count != 0)
            {
                var pricesFromBinance =
                    await _binanceClient.GetLatestPricesAsync(symbolsToFetch);

                foreach (var kv in pricesFromBinance)
                {
                    var price = new Price
                    {
                        Symbol = kv.Key,
                        Value = kv.Value,
                        Timestamp = DateTime.UtcNow
                    };

                    result.Add(price);

                    _cache.Set(CachePrefix + kv.Key, price, CacheTtl);
                    _db.Prices.Add(price);
                }

                await _db.SaveChangesAsync();
            }

            return result;
        }
    }
}

