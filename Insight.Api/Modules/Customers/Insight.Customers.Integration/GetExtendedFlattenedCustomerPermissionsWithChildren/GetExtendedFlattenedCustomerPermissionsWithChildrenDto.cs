using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Integration.GetExtendedFlattenedCustomerPermissionsWithChildren;

public class GetExtendedFlattenedCustomerPermissionsWithChildrenDto
{
    public List<CustomerPermissionGroup> CustomerPermissionsExtended { get; private set; }

    public GetExtendedFlattenedCustomerPermissionsWithChildrenDto(List<CustomerPermissionGroup> customerPermissionsExtended)
    {
        CustomerPermissionsExtended = customerPermissionsExtended;
    }
}