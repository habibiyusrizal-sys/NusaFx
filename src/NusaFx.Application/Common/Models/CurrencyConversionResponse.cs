using System;

namespace NusaFx.Application.Common.Models;

public class CurrencyConversionResponse
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
    public decimal ConvertedAmount { get; set; }
    public decimal Rate { get; set; }
    public string Source { get; set; }
    public string SourceApi { get; set; }
}
