using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using NusaFx.Application.Common;
using NusaFx.Application.Common.Models;
using NusaFx.Application.Entities;
using NusaFx.Application.Interfaces;

namespace NusaFx.Application.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IDistributedCache _cache;
    private readonly HttpClient _httpClient;
    private readonly IConversionLog _conversionLog;
    private readonly IConfiguration _configuration;

    public CurrencyService(
        IDistributedCache cache,
        HttpClient httpClient,
        IConversionLog conversionLog,
        IConfiguration configuration
    )
    {
        _cache = cache;
        _httpClient = httpClient;
        _conversionLog = conversionLog;
        _configuration = configuration;
    }

    public async Task<Result<CurrencyConversionResponse>> ConvertCurrencyAsync(
        string fromCurrency,
        string toCurrency,
        decimal amount
    )
    {
        try
        {
            // Validate currency codes
            if (
                string.IsNullOrWhiteSpace(fromCurrency)
                || string.IsNullOrWhiteSpace(toCurrency)
                || fromCurrency.Length != 3
                || toCurrency.Length != 3
            )
            {
                return Result<CurrencyConversionResponse>.Failure(
                    "Invalid currency code provided."
                );
            }

            fromCurrency = fromCurrency.Trim().ToUpperInvariant();
            toCurrency = toCurrency.Trim().ToUpperInvariant();

            string cacheKey = $"rate:{fromCurrency}:{toCurrency}";
            string cachedRate = null;

            try
            {
                cachedRate = await _cache.GetStringAsync(cacheKey);
            }
            catch (Exception cacheEx)
            {
                // Redis unavailable — log and continue
                return Result<CurrencyConversionResponse>.Failure(
                    $"Redis unavailable: {cacheEx.Message}"
                );
            }

            decimal rate;
            string source;

            if (
                !string.IsNullOrEmpty(cachedRate)
                && decimal.TryParse(cachedRate, out var parsedRate)
            )
            {
                rate = parsedRate;
                source = "cache";
            }
            else
            {
                string apiKey = _configuration.GetSection("ApiKeys")["ExchangerateApiKey"];

                ExchangeRateApiResponse response;
                try
                {
                    response = await _httpClient.GetFromJsonAsync<ExchangeRateApiResponse>(
                        $"https://v6.exchangerate-api.com/v6/{apiKey}/pair/{fromCurrency}/{toCurrency}"
                    );
                }
                catch (HttpRequestException httpEx)
                {
                    return Result<CurrencyConversionResponse>.Failure(
                        $"API request failed: {httpEx.Message}"
                    );
                }

                if (response == null || response.Result != "success")
                {
                    return Result<CurrencyConversionResponse>.Failure(
                        "Currency conversion API returned an error."
                    );
                }

                rate = response.ConversionRate;
                source = "api";

                try
                {
                    await _cache.SetStringAsync(
                        cacheKey,
                        rate.ToString(),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        }
                    );
                }
                catch (Exception cacheEx)
                {
                    Console.WriteLine($"Cache write failed: {cacheEx.Message}");
                }
            }

            decimal convertedAmount = amount * rate;

            await _conversionLog.AddConversionLogAsync(
                new ConversionLog(
                    fromCurrency,
                    toCurrency,
                    amount,
                    rate,
                    convertedAmount,
                    convertedAmount
                )
            );

            return Result<CurrencyConversionResponse>.Success(
                new CurrencyConversionResponse
                {
                    From = fromCurrency,
                    To = toCurrency,
                    Amount = amount,
                    Rate = rate,
                    ConvertedAmount = convertedAmount,
                    Source = source,
                    SourceApi = "Exchangerate-API",
                }
            );
        }
        catch (Exception ex)
        {
            return Result<CurrencyConversionResponse>.Failure(
                $"An unexpected error occurred while converting currency: {ex.Message}"
            );
        }
    }
}
