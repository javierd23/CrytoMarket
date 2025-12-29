using CryptoMarket.Application.Services;
using CryptoMarket.Tests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using CryptoMarket.Application.Interfaces;
using Xunit;

public class PriceServiceTests
{
    [Fact]
    public async Task GetLatestPricesAsync_ShouldCachePrices()
    {
        // Arrange
        var db = DbContextHelper.Create();

        var binanceMock = new Mock<IBinanceClient>();
        binanceMock.Setup(x => x.GetLatestPricesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new Dictionary<string, decimal>
            {
                { "BTCUSDT", 50000 }
            });

        var cache = new MemoryCache(new MemoryCacheOptions());

        var service = new PriceService(binanceMock.Object, cache, db);

        // Act
        await service.GetLatestPricesAsync(new[] { "BTCUSDT" });
        await service.GetLatestPricesAsync(new[] { "BTCUSDT" });

        // Assert
        binanceMock.Verify(
            x => x.GetLatestPricesAsync(It.IsAny<IEnumerable<string>>()),
            Times.Once);
    }
}

