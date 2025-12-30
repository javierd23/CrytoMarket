using CryptoMarket.Application.Exceptions;
using CryptoMarket.Application.Interfaces;
using System.Net.Http.Json;



namespace CryptoMarket.Infrastructure.Binance
{
    public class BinanceClient : IBinanceClient
    {
        private readonly HttpClient _httpClient;
        
        // private const string BaseUrl = "https://api.binance.com/api/v3";

        public BinanceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        } 

        public async Task<List<string>> GetUsdtSymbolsAsync()
        { 
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ExchangeInfoResponse>(
                "api/v3/exchangeInfo");

                return response!.Symbols
                    .Where(s => s.QuoteAsset == "USDT" && s.Status == "TRADING")
                    .Select(s => s.Symbol)
                    .ToList();
            }
            // Since Binince api is block for some country, I will handle 451 for blocking like US.
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.UnavailableForLegalReasons)
            {
                throw new BinanceBlockedException("Binance API is unavailable in your region (HTTP 451).");

            }
        }

        public async Task<Dictionary<string, decimal>> GetLatestPricesAsync(IEnumerable<string> symbols)
        {
            var tasks = symbols.Select(async symbol =>
            {
                var priceResponse = await _httpClient.GetFromJsonAsync<PriceResponse>(
                    $"/api/v3/ticker/price?symbol={symbol}");

                return (symbol, decimal.Parse(priceResponse!.Price));
            });

            var results = await Task.WhenAll(tasks);

            return results.ToDictionary(r => r.symbol, r => r.Item2);
        }

        public async Task<List<List<object>>> GetCandlesAsync(string symbol, string interval, int limit)
        {
            
            var result = await _httpClient.GetFromJsonAsync<List<List<object>>>(
                $"/api/v3/klines?symbol={symbol}&interval={interval}&limit={limit}");
            
            if (result == null)
                throw new Exception("Binance returned empty kline data.");

            return result!;

        }
    }
}
