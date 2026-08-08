using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NusaFx.Application.Entities;

namespace NusaFx.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");

        builder.HasKey(t => t.Id).HasName("PK_Transaction");

        builder.Property(t => t.Id).HasDefaultValueSql("NEWID()");

        builder.Property(t => t.Description).IsRequired().HasMaxLength(255);

        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(t => t.Currency).IsRequired().HasColumnType("char(3)");

        builder.Property(t => t.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();

        builder.HasCheckConstraint("CHK_Transaction_Amount_Positive", "Amount > 0");
        builder.HasCheckConstraint("CHK_Transaction_Currency_Allowed", "Currency IN ('USD','MYR')");

        builder.HasIndex(t => t.CreatedAt).HasDatabaseName("IX_Transaction_CreatedAt");
    }
}
