using System;

namespace NusaFx.Application.Common.Models;

public class TransactionRequest
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }

    public TransactionRequest()
    {
        Id = Guid.Empty;
        Description = string.Empty;
        Amount = 0;
        Currency = string.Empty;
    }
}
