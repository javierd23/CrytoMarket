using Microsoft.AspNetCore.Mvc;
using CryptoMarket.Application.Interfaces;
using CryptoMarket.Application.Services;


namespace CryptoMarket.Controllers
{
    [ApiController]
    [Route("api/candles")]
    public class CandlesController : ControllerBase
    {
        private readonly ICandleService _candleService;

        public CandlesController(ICandleService candleService)
        {
            _candleService = candleService!;
        }

        [HttpGet]
        public async Task<IActionResult> GetCandles(
            string symbol,
            [FromQuery] string interval = "1m",
            [FromQuery] int limit = 100)
        {

            var candles = await _candleService.GetCandlesAsync(symbol.ToUpper(), interval, limit);

            return Ok(candles);
        } 
    }
}
