using System;

namespace NusaFx.Application.Common;

public sealed class Currency
{
    public string Code { get; }

    private Currency(string code)
    {
        Code = code;
    }

    public static readonly Currency USD = new Currency("USD");
    public static readonly Currency MYR = new Currency("MYR");

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        return code.ToUpper() switch
        {
            "USD" => USD,
            "MYR" => MYR,
            _ => throw new ArgumentException($"Unsupported currency code: {code}"),
        };
    }

    public override string ToString() => Code;

    public override bool Equals(object obj) => obj is Currency other && Code == other.Code;

    public override int GetHashCode() => Code.GetHashCode();
}
