using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Integration.GetExtendedFlattenedCustomerPermissionsWithChildren;

public sealed class GetExtendedFlattenedCustomerPermissionsWithChildrenQuery : IQuery<GetExtendedFlattenedCustomerPermissionsWithChildrenDto>
{
    public List<CustomerPermissionGroup> CustomerPermissions { get; private set; }

    private GetExtendedFlattenedCustomerPermissionsWithChildrenQuery(List<CustomerPermissionGroup> customerPermissions)
    {
        CustomerPermissions = customerPermissions;
    }

    public static GetExtendedFlattenedCustomerPermissionsWithChildrenQuery Create(List<CustomerPermissionGroup> customerPermissions)
    {
        return new GetExtendedFlattenedCustomerPermissionsWithChildrenQuery(customerPermissions);
    }
}