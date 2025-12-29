using CryptoMarket.Application.Interfaces;
using CryptoMarket.Domain.Entities;
using CryptoMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CryptoMarket.Application.Services
{
    public class CandleService : ICandleService
    {
        private readonly IBinanceClient? _binanceClient;
        private readonly AppDbContext? _db;

        public CandleService(
            IBinanceClient binanceClient,
            AppDbContext db
           )
        {
            binanceClient = _binanceClient!;
            db = _db!;
        }

        public async Task<List<Candle>> GetCandlesAsync(string symbol, string interval, int limit)
        {
            var rawCandles = await _binanceClient.GetCandlesAsync(symbol, interval, limit);

            var candles = rawCandles.Select(c => new Candle
            {
                Symbol = symbol,
                OpenTime = DateTimeOffset
                    .FromUnixTimeMilliseconds(Convert.ToInt64(c[0]))
                    .UtcDateTime,
                Open = Convert.ToDecimal(c[1]),
                High = Convert.ToDecimal(c[2]),
                Low = Convert.ToDecimal(c[3]),
                Close = Convert.ToDecimal(c[4]),
                Volume = Convert.ToDecimal(c[5])
            }).ToList();

            _db.Candles.AddRange(candles);
            await _db.SaveChangesAsync();

            return candles;
        }
    }
}
