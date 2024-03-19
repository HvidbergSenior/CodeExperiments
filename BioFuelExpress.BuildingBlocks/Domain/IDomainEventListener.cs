using MediatR;

namespace BioFuelExpress.BuildingBlocks.Domain
{
    public interface IDomainEventListener<in TRequest> : INotificationHandler<TRequest> where TRequest : IDomainEvent
    {
    }
}
