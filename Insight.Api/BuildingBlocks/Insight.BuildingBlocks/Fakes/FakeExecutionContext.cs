using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Fakes
{
    public class FakeExecutionContext : IExecutionContext
    {
        private readonly bool adminPrivileges;
        public Guid UserId { get; }

        public string UserName { get; }
        public string Email { get; private set; }

        public FakeExecutionContext() : this(Guid.NewGuid())
        {
        }

        public void SetNewEmail(string email)
        {
            Email = email;
        }

        public Task<ReadOnlyCollection<CustomerPermissionGroup>> GetCustomersPermissionsAsync(bool includeChildren = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<CustomerPermissionGroup>()
            {
                CustomerPermissionGroup.Create(CustomerId.Create(Guid.NewGuid()), CustomerNumber.Empty(), CustomerName.Empty(), new List<CustomerPermission>()
                {
                    CustomerPermission.Admin
                })
            }.AsReadOnly());
        }
        
        public Task<bool> GetAdminPrivileges(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(adminPrivileges);
        }

        public FakeExecutionContext(Guid userId, string userName = "test", string email = "me@me.dk", bool adminPrivileges = false)
        {
            UserId = userId;
            UserName = userName;
            Email = email;

            this.adminPrivileges = adminPrivileges;
        }
    }
}