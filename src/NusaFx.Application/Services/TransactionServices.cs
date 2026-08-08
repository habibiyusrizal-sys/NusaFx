using System;
using System.Collections.Generic;
using NusaFx.Application.Common;
using NusaFx.Application.Entities;
using NusaFx.Application.Interfaces;

namespace NusaFx.Application.Services;

public class TransactionServices : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionServices(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<Transaction>> CreateTransactionAsync(
        string description,
        decimal amount,
        string currency
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(description))
                return Result<Transaction>.Failure("Description cannot be empty.");

            if (amount <= 0)
                return Result<Transaction>.Failure("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(currency))
                return Result<Transaction>.Failure("Currency cannot be empty.");

            if (
                !Currency.FromCode(currency).Equals(Currency.USD)
                && !Currency.FromCode(currency).Equals(Currency.MYR)
            )
                return Result<Transaction>.Failure(
                    "Unsupported currency. Only USD and MYR are supported."
                );

            await _transactionRepository.AddTransactionAsync(
                new Transaction(description, amount, currency)
            );

            return Result<Transaction>.Success(new Transaction(description, amount, currency));
        }
        catch (Exception ex)
        {
            return Result<Transaction>.Failure(
                $"An error occurred while creating the transaction: {ex.Message}"
            );
        }
    }

    public async Task<Result<bool>> DeleteTransactionAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return Result<bool>.Failure("Invalid transaction ID.");

            Transaction transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
                return Result<bool>.Failure("Transaction not found.");

            bool deleted = await _transactionRepository.DeleteTransactionAsync(id);

            return deleted
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Failed to delete the transaction.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(
                $"An error occurred while deleting the transaction: {ex.Message}"
            );
        }
    }

    public async Task<Result<Transaction>> GetTransactionByIdAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return Result<Transaction>.Failure("Invalid transaction ID.");

            Transaction transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
                return Result<Transaction>.Failure("Transaction not found.");

            return Result<Transaction>.Success(transaction);
        }
        catch (Exception ex)
        {
            return Result<Transaction>.Failure(
                $"An error occurred while fetching the transaction: {ex.Message}"
            );
        }
    }

    public async Task<PagedResult<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return PagedResult<Transaction>.Failure(
                    "Page number and page size must be greater than zero."
                );

            IEnumerable<Transaction> transactions =
                await _transactionRepository.GetTransactionsAsync(pageNumber, pageSize);

            int totalCount = transactions.Count(); // This should ideally be fetched from the database for accuracy

            return PagedResult<Transaction>.Create(transactions, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            return PagedResult<Transaction>.Failure(
                $"An error occurred while fetching transactions: {ex.Message}"
            );
        }
    }

    public async Task<Result<Transaction>> UpdateTransactionAsync(
        Guid id,
        string description,
        decimal amount,
        string currency
    )
    {
        try
        {
            if (id == Guid.Empty)
                return Result<Transaction>.Failure("Invalid transaction ID.");

            Transaction currentTransaction = await _transactionRepository.GetTransactionByIdAsync(
                id
            );
            Transaction updatedTransaction = new Transaction(description, amount, currency);

            if (currentTransaction == null)
                return Result<Transaction>.Failure("Transaction not found.");

            Transaction newUpdateTransaction = await _transactionRepository.UpdateTransactionAsync(
                updatedTransaction
            );
            return Result<Transaction>.Success(newUpdateTransaction);
        }
        catch (Exception ex)
        {
            return Result<Transaction>.Failure(
                $"An error occurred while updating the transaction: {ex.Message}"
            );
        }
    }
}
