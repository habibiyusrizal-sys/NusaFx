using System;
using NusaFx.Application.Entities;

namespace NusaFx.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public string ErrorMessage { get; private set; }

    public PagedResult(
        IEnumerable<T> items,
        int totalCount,
        int pageNumber,
        int pageSize,
        string errorMessage
    )
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        ErrorMessage = errorMessage;
    }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int totalCount,
        int pageNumber,
        int pageSize
    )
    {
        return new PagedResult<T>(items, totalCount, pageNumber, pageSize, null);
    }

    public static PagedResult<T> Failure(string errorMessage)
    {
        return new PagedResult<T>(Enumerable.Empty<T>(), 0, 1, 10, errorMessage);
    }
}
