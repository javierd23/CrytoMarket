using CryptoMarket.Application.Services;
using CryptoMarket.Domain.Entities;
using CryptoMarket.Tests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using CryptoMarket.Application.Interfaces;
using Xunit;

public class SymbolServiceTests
{
    [Fact]
    public async Task GetSymbolsAsync_ShouldFetchFromBinance_WhenDbEmpty()
    {
        // Arrange
        var db = DbContextHelper.Create();

        var binanceMock = new Mock<IBinanceClient>();
        binanceMock.Setup(x => x.GetUsdtSymbolsAsync())
            .ReturnsAsync(new List<string> { "BTCUSDT", "ETHUSDT" });

        var cache = new MemoryCache(new MemoryCacheOptions());

        var service = new SymbolService(db, binanceMock.Object, cache);

        // Act
        var result = await service.GetSymbolsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Name == "BTCUSDT");
        Assert.Contains(result, s => s.Name == "ETHUSDT");
    }

    [Fact]
    public async Task GetSymbolsAsync_ShouldUseCache_OnSecondCall()
    {
        // Arrange
        var db = DbContextHelper.Create();

        var binanceMock = new Mock<IBinanceClient>();
        binanceMock.Setup(x => x.GetUsdtSymbolsAsync())
            .ReturnsAsync(new List<string> { "BTCUSDT" });

        var cache = new MemoryCache(new MemoryCacheOptions());

        var service = new SymbolService(db, binanceMock.Object, cache);

        // Act
        await service.GetSymbolsAsync();
        await service.GetSymbolsAsync();

        // Assert
        binanceMock.Verify(x => x.GetUsdtSymbolsAsync(), Times.Once);
    }
}
