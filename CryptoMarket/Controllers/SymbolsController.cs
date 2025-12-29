using CryptoMarket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CryptoMarket.Application.Exceptions;


namespace CryptoMarket.Controllers
{
    [ApiController]
    [Route("api/symbols")]
    public class SymbolsController : ControllerBase
    {
        private readonly ISymbolService? _symbolService;

        public SymbolsController(ISymbolService symbolService)
        {
            _symbolService = symbolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSymbols()
        {
            try
            {
                var symbols = await _symbolService.GetSymbolsAsync();
                return Ok(symbols);
            }
            catch(BinanceBlockedException ex)
            {
                return StatusCode(451, new { message = ex.Message });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Refresh()
        {
            await _symbolService.RefreshSymbolsAsync();

            return NoContent();
        }
    }
}
