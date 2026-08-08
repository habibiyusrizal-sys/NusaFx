using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NusaFx.Application.Interfaces;

namespace NusaFx.Application.Services;

public class AiRateService : IAiRateService
{
    public readonly HttpClient _httpClient;
    public readonly IConfiguration _configuration;

    public AiRateService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<decimal> GetAiRateAsync(string fromCurrency, string toCurrency)
    {
        string apiKey = _configuration.GetSection("ApiKeys")["OpenAiKey"];

        var requestBody = new
        {
            model = "gpt-5.4-mini",
            messages = new[]
            {
                new { role = "user", content = "You are a financial assistant." },
                new
                {
                    role = "user",
                    content = $"What is the current exchange rate from {fromCurrency} to {toCurrency}? Reply with only the numeric rate.",
                },
            },
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/chat/completions"
        );
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
            throw new InvalidOperationException(
                "AI provider rate limit exceeded (429 Too Many Requests)."
            );

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        string aiText = doc
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (decimal.TryParse(aiText, out var aiRate))
            return aiRate;

        throw new InvalidOperationException(
            $"AI response could not be parsed into a decimal: {aiText}"
        );
    }
}
