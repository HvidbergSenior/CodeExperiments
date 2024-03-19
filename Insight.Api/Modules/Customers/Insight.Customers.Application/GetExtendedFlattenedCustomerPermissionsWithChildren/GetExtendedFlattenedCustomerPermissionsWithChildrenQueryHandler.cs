using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.Customers.Integration.GetExtendedFlattenedCustomerPermissionsWithChildren;

namespace Insight.Customers.Application.GetExtendedFlattenedCustomerPermissionsWithChildren;

internal class GetExtendedFlattenedCustomerPermissionsWithChildrenQueryHandler : IQueryHandler<GetExtendedFlattenedCustomerPermissionsWithChildrenQuery, GetExtendedFlattenedCustomerPermissionsWithChildrenDto>
{
    private readonly ICustomerHierarchy customerHierarchy;

    public GetExtendedFlattenedCustomerPermissionsWithChildrenQueryHandler(ICustomerHierarchy customerHierarchy)
    {
        this.customerHierarchy = customerHierarchy;
    }

    public async Task<GetExtendedFlattenedCustomerPermissionsWithChildrenDto> Handle(GetExtendedFlattenedCustomerPermissionsWithChildrenQuery request, CancellationToken cancellationToken)
    {
        return new GetExtendedFlattenedCustomerPermissionsWithChildrenDto(GetCustomerPermissionsExtended(await customerHierarchy.GetCustomerNodes(cancellationToken),
            request.CustomerPermissions.AsReadOnly(), new()));
    }

    private static List<CustomerPermissionGroup> GetCustomerPermissionsExtended(IReadOnlyList<CustomerNode> customerNodes, ReadOnlyCollection<CustomerPermissionGroup> customerPermissions, List<CustomerPermission> customerPermissionsDownwards)
    {
        var customerPermissionsExtended = new List<CustomerPermissionGroup>();
        foreach (var customer in customerNodes)
        {
            var customerPermissionsDownwardsExtended = new List<CustomerPermission>();
            customerPermissionsDownwardsExtended.AddRange(customerPermissionsDownwards);
            var permissionOnUser = customerPermissions.FirstOrDefault(c => c.CustomerId.Value == customer.CustomerId.Value);
            if (permissionOnUser != null)
            {
                //User has this permission, send it to permissions downwards
                customerPermissionsDownwardsExtended.AddRange(permissionOnUser.Permissions);
                customerPermissionsDownwardsExtended = customerPermissionsDownwardsExtended.Distinct().ToList();
            }
            if (customerPermissionsDownwardsExtended.Any())
            {
                customerPermissionsExtended.Add(CustomerPermissionGroup.Create(
                    Insight.BuildingBlocks.Domain.CustomerId.Create(customer.CustomerId.Value), Insight.BuildingBlocks.Domain.CustomerNumber.Create(customer.CustomerNumber.Value),
                    Insight.BuildingBlocks.Domain.CustomerName.Create(customer.CustomerName.Value),
                    customerPermissionsDownwardsExtended
                    ));
            }

            if (customer.Children.Any())
            {
                customerPermissionsExtended.AddRange(GetCustomerPermissionsExtended(customer.Children, customerPermissions, customerPermissionsDownwardsExtended));
            }
        }

        return customerPermissionsExtended;
    }
}

internal class GetExtendedFlattenedCustomerPermissionsWithChildrenQueryAuthorizer : IAuthorizer<GetExtendedFlattenedCustomerPermissionsWithChildrenQuery>
{
    public GetExtendedFlattenedCustomerPermissionsWithChildrenQueryAuthorizer()
    {

    }

    public async Task<AuthorizationResult> Authorize(GetExtendedFlattenedCustomerPermissionsWithChildrenQuery query,
        CancellationToken cancellation)
    {
        return await Task.FromResult(AuthorizationResult.Succeed());
    }
}