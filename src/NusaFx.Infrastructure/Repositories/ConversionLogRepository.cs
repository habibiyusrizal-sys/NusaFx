using System;
using NusaFx.Application.Entities;
using NusaFx.Application.Interfaces;
using NusaFx.Infrastructure.Persistence;

namespace NusaFx.Infrastructure.Repositories;

public class ConversionLogRepository : IConversionLog
{
    private readonly NusaFxDbContext _dbContext;

    public ConversionLogRepository(NusaFxDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddConversionLogAsync(ConversionLog conversionLog)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await _dbContext.ConversionLogs.AddAsync(conversionLog);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
