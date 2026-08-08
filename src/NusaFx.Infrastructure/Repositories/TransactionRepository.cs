using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NusaFx.Application.Entities;
using NusaFx.Application.Interfaces;
using NusaFx.Infrastructure.Persistence;

namespace NusaFx.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly NusaFxDbContext _dbContext;

    public TransactionRepository(NusaFxDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        using var transactionScope = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            await transactionScope.CommitAsync();
        }
        catch
        {
            await transactionScope.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteTransactionAsync(Guid id)
    {
        var transaction = await _dbContext.Transactions.FindAsync(id);
        if (transaction == null)
        {
            throw new KeyNotFoundException($"Transaction with ID {id} not found.");
        }

        using var transactionScope = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            await transactionScope.CommitAsync();
            return true;
        }
        catch
        {
            await transactionScope.RollbackAsync();
            throw;
        }
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid id)
    {
        var transaction = await _dbContext.Transactions.FindAsync(id);
        if (transaction == null)
        {
            throw new KeyNotFoundException($"Transaction with ID {id} not found.");
        }
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize)
    {
        return await _dbContext
            .Transactions.OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Transaction> UpdateTransactionAsync(Transaction updatedTransaction)
    {
        using var transactionScope = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _dbContext.Transactions.Update(updatedTransaction);
            await _dbContext.SaveChangesAsync();
            await transactionScope.CommitAsync();

            return updatedTransaction;
        }
        catch
        {
            await transactionScope.RollbackAsync();
            throw;
        }
    }
}
