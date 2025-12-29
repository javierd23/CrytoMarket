using CryptoMarket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMarket.Controllers
{
    [ApiController]
    [Route("api/prices")]
    public class PricesController : ControllerBase
    {
        private readonly IPriceService? _priceService;

        public PricesController(IPriceService priceService)
        {
            priceService = _priceService!;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrices([FromQuery] string symbols)
        {
            if (string.IsNullOrWhiteSpace(symbols))
                return BadRequest("You need to enter at least on Symbol");

            var symbolsList = symbols
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToUpper());

            var prices = await _priceService.GetLatestPricesAsync(symbolsList);
            
            return Ok(prices);
            
        }
    }
}
