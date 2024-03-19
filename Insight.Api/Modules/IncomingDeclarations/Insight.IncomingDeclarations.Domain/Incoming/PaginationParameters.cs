using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming;

public sealed class PaginationParameters : ValueObject
{
    public int Page { get; private set; }
    public int PageSize { get; private set; }

    private PaginationParameters()
    {
        Page = default;
        PageSize = default;
    }

    private PaginationParameters(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public static PaginationParameters Create(int page, int pageSize)
    {
        if (page < 0)
        {
            throw new ArgumentException("Parameter page cannot be less than 0", nameof(page));
        }

        if (pageSize < 0)
        {
            throw new ArgumentException("Parameter pageSize cannot be less than 0", nameof(pageSize));
        }

        return new PaginationParameters(page, pageSize);
    }

    public static PaginationParameters Empty()
    {
        return new PaginationParameters(0, 0);
    }
}