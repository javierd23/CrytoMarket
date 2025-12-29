


namespace CryptoMarket.Application.Interfaces
{
    public interface IBinanceClient
    {
        Task<List<string>> GetUsdtSymbolsAsync();
        Task<Dictionary<string, decimal>> GetLatestPricesAsync(IEnumerable<string> symbols);
        Task<List<List<object>>> GetCandlesAsync(string symbol, string interval, int limit);
    }
}
                                            