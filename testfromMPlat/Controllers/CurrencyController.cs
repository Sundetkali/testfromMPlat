using Microsoft.AspNetCore.Mvc;
using testfromMPlat.Services;

namespace testfromMPlat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly IConfiguration _configuration;
        public CurrencyController(CurrencyService currencyService, IConfiguration configuration)
        {
            _currencyService = currencyService;
            _configuration = configuration;
        }
        [HttpGet("rate_usd_kzt")]
        public async Task<IActionResult> GetUsdToKztRate()
        {
            var url = _configuration["CurrencyConfig:RateUrl"];
            try
            {
                var rate = await _currencyService.GetUsdToKztAsync(url);
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении курса: {ex.Message}");
            }
        }
    }
}
