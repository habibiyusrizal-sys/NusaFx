using System;
using System.Text.Json.Serialization;

namespace NusaFx.Application.Common.Models;

public class ExchangeRateApiResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("base_code")]
    public string BaseCode { get; set; }

    [JsonPropertyName("target_code")]
    public string TargetCode { get; set; }

    [JsonPropertyName("conversion_rate")]
    public decimal ConversionRate { get; set; }

    [JsonPropertyName("conversion_result")]
    public decimal ConversionResult { get; set; }
}
