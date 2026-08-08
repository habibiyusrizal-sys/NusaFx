using System;
using NusaFx.Application.Entities;

namespace NusaFx.Application.Interfaces;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize);
    Task<Transaction> GetTransactionByIdAsync(Guid id);
    Task<Transaction> UpdateTransactionAsync(Transaction updatedTransaction);
    Task<bool> DeleteTransactionAsync(Guid id);
}
