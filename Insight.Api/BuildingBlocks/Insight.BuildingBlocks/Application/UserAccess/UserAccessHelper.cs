using Insight.BuildingBlocks.Domain;
using MediatR.NotificationPublishers;

namespace Insight.BuildingBlocks.Application.UserAccess;

public static class UserAccessHelper
{
    //TODO: Check if all permissions on permissionsToSet can actually be set when we have ownership of permissionsAvailableExtendedWithChildren which should include all children
    public static (bool, string) PermissionsCanBeSet(IEnumerable<CustomerPermissionGroup> permissionsAvailableExtendedWithChildren, IEnumerable<CustomerPermissionGroup> permissionsToSet)
    {
        foreach (var permission in permissionsToSet)
        {
            var permissionCanBeSet = permissionsAvailableExtendedWithChildren.Where(p =>
                    p.CustomerId.Value == permission.CustomerId.Value &&
                    p.Permissions.Contains(CustomerPermission.Admin))
                .Any();
            if (!permissionCanBeSet)
            {
                return (false, $"Permission on customer id {permission.CustomerId.Value} cannot be set.");
            }

        }
        return (true, "");
    }
}