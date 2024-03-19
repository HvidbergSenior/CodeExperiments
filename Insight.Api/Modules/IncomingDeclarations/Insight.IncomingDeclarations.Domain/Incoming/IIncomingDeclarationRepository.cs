using Insight.BuildingBlocks.Infrastructure;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public interface IIncomingDeclarationRepository : IRepository<IncomingDeclaration>, IReadonlyRepository<IncomingDeclaration>
    {
        Task<(IEnumerable<IncomingDeclaration> Items, int TotalCount, bool HasMore)> GetIncomingDeclarationsByPageNumberAndPageSize(
            int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters,
            IEnumerable<IncomingDeclarationState> incomingDeclarationStates,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomingDeclaration>> GetTemporaryIncomingDeclarationsByUploadIdAsync(IncomingDeclarationUploadId incomingDeclarationUploadId, CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomingDeclaration>> GetByIncomingDeclarationIdsAndStateAsync(IncomingDeclarationId[] incomingDeclarationIds, IncomingDeclarationState incomingDeclarationState, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomingDeclaration>> GetAllByProductCountryDispatchLocationDispatchDateAndCapacityAsync(Product product, Country country, PlaceOfDispatch placeOfDispatch, DateOnly startDate, DateOnly endDate, bool isOrderByDescending, OrderByProperty orderByProperty, CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomingDeclaration>> GetAllByDateRangeAndFilterAsync(DateOnly startDate, DateOnly endDate, string product, string company, CancellationToken cancellationToken = default);
        public Task DeleteTemporaryIncomingDeclarationsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomingDeclaration>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}