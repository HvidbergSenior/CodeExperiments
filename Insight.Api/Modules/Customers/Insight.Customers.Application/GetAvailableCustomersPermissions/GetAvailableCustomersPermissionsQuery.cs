using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;

namespace Insight.Customers.Application.GetAvailableCustomersPermissions;

public sealed class GetAvailableCustomersPermissionsQuery : IQuery<GetAvailableCustomersPermissionsResponse>
{
    private GetAvailableCustomersPermissionsQuery()
    {

    }

    public static GetAvailableCustomersPermissionsQuery Create()
    {
        return new GetAvailableCustomersPermissionsQuery();
    }
}

internal class GetAvailableCustomersPermissionsQueryHandler : IQueryHandler<GetAvailableCustomersPermissionsQuery, GetAvailableCustomersPermissionsResponse>
{
    private readonly IExecutionContext executionContext;
    private readonly ICustomerRepository customerRepository;
    private readonly ICustomerHierarchy customerHierarchy;

    public GetAvailableCustomersPermissionsQueryHandler(IExecutionContext executionContext, ICustomerRepository customerRepository, ICustomerHierarchy customerHierarchy)
    {
        this.executionContext = executionContext;
        this.customerRepository = customerRepository;
        this.customerHierarchy = customerHierarchy;

    }

    public async Task<GetAvailableCustomersPermissionsResponse> Handle(GetAvailableCustomersPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await GetAvailableCustomersResponse(request, cancellationToken);
    }

    private async Task<GetAvailableCustomersPermissionsResponse> GetAvailableCustomersResponse(GetAvailableCustomersPermissionsQuery request, CancellationToken cancellationToken)
    {
        var chNodes = await customerHierarchy.GetCustomerNodes(cancellationToken);

        if(await executionContext.GetAdminPrivileges())
        {
            //Here I need all customer nodes, so the value I got from CustomerHierarchy is ok, do nothing
            var allCustomerNodes = await customerHierarchy.GetCustomerNodes(cancellationToken);
            return new GetAvailableCustomersPermissionsResponse()
            {
                CustomerNodes = CustomerNodesToDto(allCustomerNodes, setAllPermissions: true),
            };
        }
        
        var customersPermissionIds = (await executionContext.GetCustomersPermissionsAsync(includeChildren: false, cancellationToken)).Select(c => c.CustomerId).ToList();

        var chNodesFiltered = new List<CustomerNode>();

        foreach (var customerId in customersPermissionIds)
        {
            var customerFromHierarchy = CustomerHierarchyHelper.FindInListOfCustomerNodes(chNodes, CustomerId.Create(customerId.Value));
            if (customerFromHierarchy != null)
            {
                chNodesFiltered.Add(customerFromHierarchy);
            }
        }
        //If the PermissionGroup has children, they should not be shown in the list also
        var nodesToRemove = new List<CustomerNode>();
        foreach (var node in chNodesFiltered)
        {
            if (node.Children.Count != 0)
            {
                foreach (var child in node.Children)
                {
                    nodesToRemove.Add(child);
                }
            }
        }
        nodesToRemove.ForEach((node) => chNodesFiltered.Remove(node));

        return new GetAvailableCustomersPermissionsResponse()
        {
            CustomerNodes = CustomerNodesToDto(chNodesFiltered, setAllPermissions: true),
        };
    }

    private static List<CustomerPermissionDto> CustomerNodesToDto(IReadOnlyList<CustomerNode> customerNodes,
        bool setAllPermissions = false)
    {
        var allPermissions = Enum.GetValues(typeof(CustomerPermission))
            .Cast<CustomerPermission>()
            .ToList();
        var customerNodesDto = new List<CustomerPermissionDto>();
        foreach (var customer in customerNodes)
        {

            customerNodesDto.Add(new CustomerPermissionDto()
            {
                CustomerId = customer.CustomerId.Value.ToString(),
                CustomerName = customer.CustomerName.Value,
                CustomerNumber = customer.CustomerNumber.Value,
                ParentCustomerId = customer.Parent != null ? customer.Parent.Value.ToString() : "",
                Permissions = setAllPermissions == true ? allPermissions : new(),
                Children = customer.Children.Any() ? CustomerNodesToDto(customer.Children, setAllPermissions) : new()
            });
        }

        return customerNodesDto;
    }
}

internal class GetAvailableCustomersPermissionsQueryAuthorizer : IAuthorizer<GetAvailableCustomersPermissionsQuery>
{
    private readonly IExecutionContext executionContext;

    public GetAvailableCustomersPermissionsQueryAuthorizer(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public Task<AuthorizationResult> Authorize(GetAvailableCustomersPermissionsQuery query,
        CancellationToken cancellation)
    {
        //Default is user must be logged in I understand, so nothing to check if it reached here all good
        return Task.FromResult(AuthorizationResult.Succeed());
    }
}