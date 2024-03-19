using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;

namespace Insight.Customers.Application.GetPossibleCustomerPermissions;

public sealed class GetPossibleCustomerPermissionsQuery : IQuery<GetPossibleCustomerPermissionsResponse>
{
    private GetPossibleCustomerPermissionsQuery() { }

    public static GetPossibleCustomerPermissionsQuery Create()
    {
        return new GetPossibleCustomerPermissionsQuery();
    }
}

internal class GetPossibleCustomerPermissionsQueryHandler : IQueryHandler<GetPossibleCustomerPermissionsQuery, GetPossibleCustomerPermissionsResponse>
{
    private readonly IExecutionContext executionContext;
    private readonly ICustomerHierarchy customerHierarchy;

    public GetPossibleCustomerPermissionsQueryHandler(IExecutionContext executionContext, ICustomerHierarchy customerHierarchy)
    {
        this.executionContext = executionContext;
        this.customerHierarchy = customerHierarchy;
    }

    public async Task<GetPossibleCustomerPermissionsResponse> Handle(GetPossibleCustomerPermissionsQuery request, CancellationToken cancellationToken)
    {
        if(await executionContext.GetAdminPrivileges())
        {
            //Here I need all customer nodes, so the value I got from CustomerHierarchy is ok, do nothing
            var allCustomerNodes = await customerHierarchy.GetCustomerNodes(cancellationToken);
            return new GetPossibleCustomerPermissionsResponse()
            {
                CustomerNodes = CustomerNodesToDto(allCustomerNodes, setAllPermissions: true),
            };
        }

        //Here I can only list the ones I have access to so I must extract the relevant including children
        var chNodes = await customerHierarchy.GetCustomerNodes(cancellationToken);
        var customerIdsIHaveAdminAccessTo = (await executionContext.GetCustomersPermissionsAsync()).Where(c => c.Permissions.Contains(CustomerPermission.Admin)).Select(c => c.CustomerId).ToList();
        var chNodesFiltered = new List<CustomerNode>();
       
        foreach(var customerId in customerIdsIHaveAdminAccessTo)
        {
            var customerFromHierarchy = CustomerHierarchyHelper.FindInListOfCustomerNodes(chNodes, CustomerId.Create(customerId.Value));
            if (customerFromHierarchy != null)
            {
                chNodesFiltered.Add(customerFromHierarchy);
            }
        }
        return new GetPossibleCustomerPermissionsResponse()
        {
            CustomerNodes = CustomerNodesToDto(chNodesFiltered, setAllPermissions: true),
        };
    }

    private static List<GetPossibleCustomerPermissionsCustomerNodeDto> CustomerNodesToDto(IReadOnlyList<CustomerNode> customerNodes, bool setAllPermissions = false)
    {
        var allPermissions = Enum.GetValues(typeof(CustomerPermission))
            .Cast<CustomerPermission>()
            .ToList();
        var customerNodesDto = new List<GetPossibleCustomerPermissionsCustomerNodeDto>();
        foreach (var customer in customerNodes)
        {
            customerNodesDto.Add(new GetPossibleCustomerPermissionsCustomerNodeDto()
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

internal class GetPossibleCustomerPermissionsQueryAuthorizer : IAuthorizer<GetPossibleCustomerPermissionsQuery>
{
    private readonly IExecutionContext executionContext;

    public GetPossibleCustomerPermissionsQueryAuthorizer(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public async Task<AuthorizationResult> Authorize(GetPossibleCustomerPermissionsQuery query,
        CancellationToken cancellation)
    {
        return await Task.FromResult(AuthorizationResult.Succeed());
    }
}