using Insight.BuildingBlocks.Infrastructure;

namespace Insight.Stations.Domain
{
    public interface IStationRepository : IRepository<Station>, IReadonlyRepository<Station>
    {
        Task<Station?> GetByStationNumberAsync(StationNumber stationNumber, CancellationToken cancellationToken = default);
    }
}
