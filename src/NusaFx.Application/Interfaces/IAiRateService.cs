using System;

namespace NusaFx.Application.Interfaces;

public interface IAiRateService
{
    Task<decimal> GetAiRateAsync(string fromCurrency, string toCurrency);
}
