using Insight.BuildingBlocks.Infrastructure;

namespace Insight.OutgoingDeclarations.Domain
{
    public interface IOutgoingDeclarationRepository : IRepository<OutgoingDeclaration>,
        IReadonlyRepository<OutgoingDeclaration>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<OutgoingDeclaration>> GetByCustomerNumberAsync(string customerNumberValue,
            CancellationToken cancellationToken = default);
        
        Task<IEnumerable<OutgoingDeclaration>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(IEnumerable<OutgoingDeclaration> Items, int TotalCount, bool HasMore)> GetOutgoingDeclarationsByPageNumberAndPageSize(
            int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<OutgoingDeclaration>>
            GetOutgoingDeclarationsForMany(FilteringParametersSelectMany filteringParameters,
                CancellationToken cancellationToken = default);
    }
}