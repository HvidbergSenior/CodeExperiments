using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;

namespace Insight.UserAccess.Domain.User
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindBySubjectId(Guid subjectId, CancellationToken cancellationToken);

        Task<User?> GetBySubjectId(Guid subjectId, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersByCustomerId(Guid customerId, CancellationToken cancellationToken);

        Task<User?> FindByUserName(string userName, CancellationToken cancellationToken);
        Task<User?> FindByEmail(string email, CancellationToken cancellationToken);

        Task SetAdminPrivileges(UserId userId, AdminPrivileges adminPrivileges, CancellationToken cancellationToken);

        Task UpdateCustomers(CustomerId customerId, CustomerNumber newCustomerNumber, CustomerName newCustomerName, CancellationToken cancellationToken);
        Task<(IEnumerable<User> Items, int TotalCount, bool hasMore)> GetAllUsersWithPermissionsForCustomersAsync(IEnumerable<CustomerId>? adminForCustomerIds, PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken);
    }
}