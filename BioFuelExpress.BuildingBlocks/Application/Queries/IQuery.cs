using MediatR;

namespace BioFuelExpress.BuildingBlocks.Application.Queries
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
