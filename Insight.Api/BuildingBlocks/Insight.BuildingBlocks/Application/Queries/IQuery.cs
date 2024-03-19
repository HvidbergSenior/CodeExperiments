using MediatR;

namespace Insight.BuildingBlocks.Application.Queries
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
