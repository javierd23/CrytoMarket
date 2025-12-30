using CryptoMarket.Application.Interfaces;
using CryptoMarket.Domain.Entities;
using CryptoMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;


namespace CryptoMarket.Application.Services
{
     public class CandleService : ICandleService
    {
        private readonly IBinanceClient _binanceClient;
        private readonly AppDbContext _db;

        public CandleService(
            IBinanceClient binanceClient,
            AppDbContext db
           )
        {
            _binanceClient = binanceClient!;
            _db = db!;
        }

        public async Task<List<Candle>> GetCandlesAsync(string symbol, string interval, int limit)
        {
            //confirmed that the symbol provided is a real one...
            var exist =  await _db.Symbols.AnyAsync(s => s.Name == symbol);
            if (!exist)
                {
                    
                    throw new KeyNotFoundException($"Symbol '{symbol}' is not supported or does not exist.");
                }

            var rawCandles = await _binanceClient.GetCandlesAsync(symbol, interval, limit);

            var candles = rawCandles.Select(c => 
            {
                var elements = c.Cast<JsonElement>().ToArray();

                return new Candle
                {
                    Symbol = symbol,
                    OpenTime = DateTimeOffset
                        .FromUnixTimeMilliseconds(elements[0].GetInt64())
                        .UtcDateTime,
                    
                    Open = decimal.Parse(elements[1].GetString()!, CultureInfo.InvariantCulture),
                    High = decimal.Parse(elements[2].GetString()!, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(elements[3].GetString()!, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(elements[4].GetString()!, CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(elements[5].GetString()!, CultureInfo.InvariantCulture)
                };
            }).ToList();

            _db.Candles.AddRange(candles);
            await _db.SaveChangesAsync();

            return candles;
        }
    }
}
