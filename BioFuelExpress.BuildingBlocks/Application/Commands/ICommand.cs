using MediatR;

namespace BioFuelExpress.BuildingBlocks.Application.Commands
{
    public interface ICommand<out T> : IRequest<T>
    {
    }

    public interface ICommand : IRequest
    { }
}
