using System;

namespace NusaFx.Application.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public Transaction(string description, decimal amount, string currency)
    {
        Id = Guid.Empty;
        Description = description;
        Amount = amount;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
    }
}
