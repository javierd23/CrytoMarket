using CryptoMarket.Domain.Entities;

namespace CryptoMarket.Application.Interfaces
{
    public interface ISymbolService
    {
        Task<List<Symbol>> GetSymbolsAsync();
        Task RefreshSymbolsAsync();

    }
}
