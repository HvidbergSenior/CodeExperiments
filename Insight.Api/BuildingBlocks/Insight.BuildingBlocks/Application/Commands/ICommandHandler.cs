using MediatR;

namespace Insight.BuildingBlocks.Application.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}
