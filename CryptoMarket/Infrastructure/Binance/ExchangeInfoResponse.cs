



namespace CryptoMarket.Infrastructure.Binance
{
    public class ExchangeInfoResponse
    {
        public List<SymbolInfo> Symbols { get; set; } = [];
    }

    public class SymbolInfo
    {
        public string Symbol { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string QuoteAsset { get; set; } = null!;

    }
}
