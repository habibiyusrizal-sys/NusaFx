using System;
using NusaFx.Application.Entities;

namespace NusaFx.Application.Interfaces;

public interface IConversionLog
{
    Task AddConversionLogAsync(ConversionLog conversionLog);
}
