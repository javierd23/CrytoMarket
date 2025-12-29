using CryptoMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CryptoMarket.Tests.Helpers
{
    public static class DbContextHelper
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
