using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Integration.GetPermissionsForUserName;

public class GetPermissionsForUserNameDto
{
    public List<CustomerPermissionGroup> CustomerPermissions { get; private set; }

    public GetPermissionsForUserNameDto(List<CustomerPermissionGroup> customerPermissions)
    {
        CustomerPermissions = customerPermissions;
    }
}