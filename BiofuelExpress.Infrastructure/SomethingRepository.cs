using BioFuelExpress.BuildingBlocks.Events;
using BioFuelExpress.BuildingBlocks.Infrastructure.Marten;
using BioFuelExpress.Domain;
using Marten;

namespace BioFuelExpress.Infrastructure
{
    public class SomethingRepository : MartenDocumentRepository<Something>, ISomethingRepository
    {
        public SomethingRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {

        }
    }
}
