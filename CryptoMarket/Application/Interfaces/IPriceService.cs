using CryptoMarket.Domain.Entities;

namespace CryptoMarket.Application.Interfaces
{
    public interface IPriceService
    {
        Task<List<Price>> GetLatestPricesAsync(IEnumerable<string> symbols);
    }
}
