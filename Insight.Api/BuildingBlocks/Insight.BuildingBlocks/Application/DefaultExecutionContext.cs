using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Application
{
    public class DefaultExecutionContext : IExecutionContext
    {
        public Guid UserId { get => Guid.Empty; }

        public string UserName => "default";

        public Task<ReadOnlyCollection<CustomerPermissionGroup>> GetCustomersPermissionsAsync(bool includeChildren = false, CancellationToken cancellationToken = default)
        {
            var emptyList = Array.Empty<CustomerPermissionGroup>().AsReadOnly();
            return Task.FromResult(emptyList);
        }
      
        public Task<bool> GetAdminPrivileges(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}