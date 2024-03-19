using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Stations.Domain;
using Marten;

namespace Insight.Stations.Infrastructure
{
    public class StationRepository : MartenDocumentRepository<Station>, IStationRepository
    {
        public StationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public async Task<Station?> GetByStationNumberAsync(StationNumber stationNumber, CancellationToken cancellationToken = default)
        {
            return await Query().FirstOrDefaultAsync(x => x.StationNumber.Value == stationNumber.Value, cancellationToken);
        }
    }
}
