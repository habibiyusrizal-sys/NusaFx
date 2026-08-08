using System;
using Microsoft.EntityFrameworkCore;
using NusaFx.Application.Entities;
using NusaFx.Infrastructure.Persistence.Configurations;

namespace NusaFx.Infrastructure.Persistence;

public class NusaFxDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; private set; }
    public DbSet<ConversionLog> ConversionLogs { get; private set; }

    public NusaFxDbContext(DbContextOptions<NusaFxDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new ConversionLogConfiguration());
    }
}
