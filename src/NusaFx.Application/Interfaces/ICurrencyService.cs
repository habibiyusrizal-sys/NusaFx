using System;
using NusaFx.Application.Common;
using NusaFx.Application.Common.Models;

namespace NusaFx.Application.Interfaces;

public interface ICurrencyService
{
    Task<Result<CurrencyConversionResponse>> ConvertCurrencyAsync(
        string fromCurrency,
        string toCurrency,
        decimal amount
    );
}
