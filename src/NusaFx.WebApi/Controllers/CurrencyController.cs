using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NusaFx.Application.Interfaces;
using NusaFx.Application.Services;

namespace NusaFx.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyConversionService;
        private readonly IAiRateService _aiRateService;

        public CurrencyController(
            ICurrencyService currencyConversionService,
            IAiRateService aiRateService
        )
        {
            _currencyConversionService = currencyConversionService;
            _aiRateService = aiRateService;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] decimal amount
        )
        {
            var result = await _currencyConversionService.ConvertCurrencyAsync(from, to, amount);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("convert-with-ai")]
        public async Task<IActionResult> ConvertWithAi(string from, string to, decimal amount)
        {
            var liveResult = await _currencyConversionService.ConvertCurrencyAsync(
                from,
                to,
                amount
            );

            if (!liveResult.IsSuccess)
                return BadRequest(liveResult.ErrorMessage);

            // Call AI model (pseudo-code, replace with Gemini/OpenAI SDK)
            decimal aiRate = await _aiRateService.GetAiRateAsync(from, to);

            decimal liveRate = liveResult.Data.Rate;
            decimal difference = aiRate - liveRate;
            decimal percentDiff = (difference / liveRate) * 100;

            var extendedResponse = new
            {
                liveResult.Data,
                AiRate = aiRate,
                Difference = difference,
                PercentDifference = percentDiff,
            };

            return Ok(extendedResponse);
        }
    }
}
