using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Application
{
    public interface IExecutionContext
    {   
        public string UserName { get; }
        public Task<ReadOnlyCollection<CustomerPermissionGroup>> GetCustomersPermissionsAsync(
            bool includeChildren = false, CancellationToken cancellationToken = default);
        
        public Task<bool> GetAdminPrivileges(CancellationToken cancellationToken = default);
    }
}
