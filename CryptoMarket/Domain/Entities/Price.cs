


namespace CryptoMarket.Domain.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public string? Symbol { get; set; } = null;
        public decimal Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
