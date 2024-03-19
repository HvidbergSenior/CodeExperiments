using MediatR;

namespace BioFuelExpress.BuildingBlocks.Application.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}
