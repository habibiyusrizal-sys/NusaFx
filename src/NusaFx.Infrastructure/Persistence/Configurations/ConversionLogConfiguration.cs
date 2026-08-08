using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NusaFx.Application.Entities;

namespace NusaFx.Infrastructure.Persistence.Configurations;

public class ConversionLogConfiguration : IEntityTypeConfiguration<ConversionLog>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<ConversionLog> builder)
    {
        builder.ToTable("ConversionLog");

        builder.HasKey(c => c.Id).HasName("PK_ConversionLog");

        builder.Property(c => c.Id).HasDefaultValueSql("NEWID()");

        builder.Property(c => c.FromCurrency).IsRequired().HasColumnType("char(3)");

        builder.Property(c => c.ToCurrency).IsRequired().HasColumnType("char(3)");

        builder.Property(c => c.Amount).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(c => c.LiveRate).HasColumnType("decimal(18,6)").IsRequired();

        builder.Property(c => c.LiveResult).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(c => c.AiResult).HasColumnType("decimal(18,2)");

        builder.Property(c => c.Difference).HasColumnType("decimal(18,2)");

        builder.Property(c => c.ConvertedAt).HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();

        builder.HasCheckConstraint("CHK_ConversionLog_Amount_Positive", "Amount >= 0");

        builder.HasIndex(c => c.ConvertedAt).HasDatabaseName("IX_ConversionLog_ConvertedAt");

        builder
            .HasIndex(c => new
            {
                c.FromCurrency,
                c.ToCurrency,
                c.ConvertedAt,
            })
            .HasDatabaseName("IX_ConversionLog_CurrencyPair");
    }
}
