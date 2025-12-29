using CryptoMarket.Domain.Entities;

namespace CryptoMarket.Application.Interfaces
{
    public interface ICandleService
    {
        Task<List<Candle>> GetCandlesAsync(string symbol, string interval, int limit);
    }
}