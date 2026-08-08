using System;
using NusaFx.Application.Common;
using NusaFx.Application.Entities;

namespace NusaFx.Application.Interfaces;

public interface ITransactionService
{
    Task<Result<Transaction>> CreateTransactionAsync(
        string description,
        decimal amount,
        string currency
    );

    Task<PagedResult<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize);
    Task<Result<Transaction>> GetTransactionByIdAsync(Guid id);
    Task<Result<Transaction>> UpdateTransactionAsync(
        Guid id,
        string description,
        decimal amount,
        string currency
    );
    Task<Result<bool>> DeleteTransactionAsync(Guid id);
}
