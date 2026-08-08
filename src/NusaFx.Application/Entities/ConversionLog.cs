using System;

namespace NusaFx.Application.Entities;

public class ConversionLog
{
    public Guid Id { get; private set; }
    public string FromCurrency { get; private set; }
    public string ToCurrency { get; private set; }
    public decimal Amount { get; private set; }
    public decimal LiveRate { get; private set; }
    public decimal LiveResult { get; private set; }
    public decimal AiResult { get; private set; }
    public decimal Difference { get; private set; }
    public DateTime ConvertedAt { get; private set; }

    private ConversionLog() { }

    public ConversionLog(
        string fromCurrency,
        string toCurrency,
        decimal amount,
        decimal liveRate,
        decimal liveResult,
        decimal aiResult
    )
    {
        Id = Guid.Empty;
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Amount = amount;
        LiveRate = liveRate;
        LiveResult = liveResult;
        AiResult = aiResult;
        Difference = Math.Abs(liveResult - aiResult);
        ConvertedAt = DateTime.UtcNow;
    }
}
