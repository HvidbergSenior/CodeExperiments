using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.UserAccess.Domain.User;
using Marten;
using Marten.Linq.MatchesSql;
using Marten.Pagination;

namespace Insight.UserAccess.Infrastructure
{
    public class UserRepository : MartenDocumentRepository<User>, IUserRepository
    {
        private readonly IDocumentSession documentSession;
        public UserRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public Task<User?> FindBySubjectId(Guid subjectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
           var query =  await Query().Where(u => u.CustomerPermissions.Any(p => p.CustomerId.Value == customerId)).ToListAsync();
           return query;
        }

        public async Task<User?> FindByUserName(string userName, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(u => string.Equals(u.UserName.Value,userName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public async Task<User?> FindByEmail(string email, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(u => string.Equals(u.Email.Value,email, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public Task<User?> GetBySubjectId(Guid subjectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetAdminPrivileges(UserId userId, AdminPrivileges adminPrivileges, CancellationToken cancellationToken)
        {
            var user = await GetById(userId.Value, cancellationToken);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.SetAdminPrivileges(adminPrivileges);
        }

        public async Task UpdateCustomers(CustomerId customerId, CustomerNumber newCustomerNumber, CustomerName newCustomerName, CancellationToken cancellationToken)
        {
            //List of users that have the customer with id customerId and a different customerName for newCustomerName
            var usersToUpdate = (await Query().Where(u => u.CustomerPermissions.Any(p => p.CustomerId.Value == customerId.Value && p.CustomerName.Value != newCustomerName.Value)).ToListAsync());

            if (usersToUpdate.Any())
            {
                //Go trough each user to update
                foreach (var userToUpdate in usersToUpdate)
                {
                    //Create new list of CustomerPermissionGroup value objects and add the old ones and the modified ones there
                    var newCustomersPermissions = new List<CustomerPermissionGroup>();
                    foreach (var customerToUpdate in userToUpdate.CustomerPermissions)
                    {
                        if (customerToUpdate.CustomerId.Value != customerId.Value)
                        {
                            //If it's another id just add it as it is, no need to modify it
                            newCustomersPermissions.Add(customerToUpdate);
                        }
                        else
                        {
                            //For the id that changed name create a new customer permission with the new name and add it
                            newCustomersPermissions.Add(CustomerPermissionGroup.Create(customerId, newCustomerNumber, newCustomerName,
                                customerToUpdate.Permissions));
                        }
                    }
                    userToUpdate.SetCustomerPermissions(newCustomersPermissions);
                }
                documentSession.Update(usersToUpdate.ToArray());
                await documentSession.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<(IEnumerable<User> Items, int TotalCount, bool hasMore)> GetAllUsersWithPermissionsForCustomersAsync(IEnumerable<CustomerId>? adminForCustomerIds, PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken)
        {
            var pageNumber = paginationParameters.Page;
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var query = Query();
            if (adminForCustomerIds != null)
            {
                var matchSQL =
                    $"d.customer_ids && '{{{string.Join(",", adminForCustomerIds.Select(c => c.Value.ToString()))}}}'";
                query = query.Where(query => query.MatchesSql(matchSQL));
            }

            if (filteringParameters.UserStatus != UserStatus.BlockedAndActive)
            {
                var blockedValue = filteringParameters.UserStatus == UserStatus.Active ? false : true;
                query = query.Where(d => d.Blocked.Value == blockedValue);
            }
            
            if (!filteringParameters.EmailFiltering.Value.ToUpperInvariant().Equals(EmailFiltering.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Email.Value.ToUpperInvariant().Contains(filteringParameters.EmailFiltering.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            //CustomerNumber and CustomerName is the same value from FrontEnd (= AccountId). 
            if (!filteringParameters.CustomerNumber.Value.ToUpperInvariant().Equals(CustomerNumber.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase) || !filteringParameters.CustomerName.Value.ToUpperInvariant().Equals(CustomerName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.CustomerPermissions.Any(d => d.CustomerName.Value.Contains(filteringParameters.CustomerName.Value, StringComparison.OrdinalIgnoreCase) || d.CustomerName.Value.ToUpperInvariant().Contains(filteringParameters.CustomerName.Value, StringComparison.OrdinalIgnoreCase) || d.CustomerNumber.Value.Contains(filteringParameters.CustomerNumber.Value, StringComparison.OrdinalIgnoreCase) || d.CustomerNumber.Value.ToUpperInvariant().Contains(filteringParameters.CustomerNumber.Value, StringComparison.OrdinalIgnoreCase)));
            }
         
            query = sortingParameters.IsOrderDescending
                ? query.OrderByDescending(sortingParameters.OrderByProperty.FirstCharToUpper())
                : query.OrderBy(sortingParameters.OrderByProperty.FirstCharToUpper());

            var pagedList = await query.ToPagedListAsync(pageNumber, paginationParameters.PageSize, cancellationToken);

            return (pagedList, Convert.ToInt32(pagedList.TotalItemCount), pagedList.HasNextPage);
        }
    }
}