using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.UserAccess.Integration.GetPermissionsForUserName;

namespace Insight.Customers.Application.GetPossibleCustomerPermissionsForGivenUser;

public sealed class GetPossibleCustomerPermissionsForGivenUserQuery : IQuery<GetPossibleCustomerPermissionsForGivenUserResponse>
{
    public UserName UserName { get; private set; }

    private GetPossibleCustomerPermissionsForGivenUserQuery()
    {
        UserName = UserName.Empty();
    }

    private GetPossibleCustomerPermissionsForGivenUserQuery(UserName userName)
    {
        UserName = userName;
    }

    public static GetPossibleCustomerPermissionsForGivenUserQuery Create(UserName userName)
    {
        return new GetPossibleCustomerPermissionsForGivenUserQuery(userName);
    }
}

internal class GetPossibleCustomerPermissionsForGivenUserQueryHandler : IQueryHandler<GetPossibleCustomerPermissionsForGivenUserQuery, GetPossibleCustomerPermissionsForGivenUserResponse>
{
    private readonly IExecutionContext executionContext;
    private readonly ICustomerHierarchy customerHierarchy;
    private readonly IQueryBus queryBus;

    public GetPossibleCustomerPermissionsForGivenUserQueryHandler(IExecutionContext executionContext, ICustomerHierarchy customerHierarchy, IQueryBus queryBus)
    {
        this.executionContext = executionContext;
        this.customerHierarchy = customerHierarchy;
        this.queryBus = queryBus;
    }

    public async Task<GetPossibleCustomerPermissionsForGivenUserResponse> Handle(GetPossibleCustomerPermissionsForGivenUserQuery request, CancellationToken cancellationToken)
    {
        var chNodes = await customerHierarchy.GetCustomerNodes(cancellationToken);
        var permissionsForUserNameQuery = GetPermissionsForUserNameQuery.Create(request.UserName.Value);
        var permissionsForUserDto = await queryBus.Send<GetPermissionsForUserNameQuery, GetPermissionsForUserNameDto>(permissionsForUserNameQuery, cancellationToken);
        var permissionsForUser = permissionsForUserDto.CustomerPermissions.AsReadOnly();

        if(await executionContext.GetAdminPrivileges())
        {
            return new GetPossibleCustomerPermissionsForGivenUserResponse()
            {
                CustomerNodes = CustomerNodesToDto(chNodes, permissionsForUser, new()),
            };
        }
        //Here I can only list the ones I have access to so I must extract the relevant including children
        var customerIdsIhaveAdminAccessTo = (await executionContext.GetCustomersPermissionsAsync()).Where(c => c.Permissions.Contains(CustomerPermission.Admin)).Select(c => c.CustomerId).ToList();
        var chNodesFiltered = new List<CustomerNode>();
        foreach(var customerId in customerIdsIhaveAdminAccessTo)
        {
            var customerFromHierarchy = CustomerHierarchyHelper.FindInListOfCustomerNodes(chNodes, CustomerId.Create(customerId.Value));
            if (customerFromHierarchy != null)
            {
                chNodesFiltered.Add(customerFromHierarchy);
            }
        }
        return new GetPossibleCustomerPermissionsForGivenUserResponse()
        {
            CustomerNodes = CustomerNodesToDto(chNodesFiltered, permissionsForUser, new()),
        };
    }

    private static List<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto> CustomerNodesToDto(IReadOnlyList<CustomerNode> customerNodes, ReadOnlyCollection<CustomerPermissionGroup> customerPermissions, List<CustomerPermission> customerPermissionsDownwards)
    {
        var customerNodesDto = new List<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto>();
        var allPermissions = Enum.GetValues(typeof(CustomerPermission))
            .Cast<CustomerPermission>()
            .ToList();
        foreach (var customer in customerNodes)
        {
            var newPermissions = customerPermissions.Where(p => p.CustomerId.Value == customer.CustomerId.Value)
                .FirstOrDefault();
            var customerPermissionsDownwardsExtended = new List<CustomerPermission>();
            customerPermissionsDownwardsExtended.AddRange(customerPermissionsDownwards);
            if (newPermissions != null)
            {
                //Customer has actually permissions for this customer id so I must add the new ones to the one I got from parents
                customerPermissionsDownwardsExtended.AddRange(newPermissions.Permissions);
                customerPermissionsDownwardsExtended = customerPermissionsDownwardsExtended.Distinct().ToList();
            }
            customerNodesDto.Add(new GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto()
            {
                CustomerId = customer.CustomerId.Value.ToString(),
                CustomerName = customer.CustomerName.Value,
                CustomerNumber = customer.CustomerNumber.Value,
                ParentCustomerId = customer.Parent != null ? customer.Parent.Value.ToString() : "",
                PermissionsGiven = customerPermissionsDownwardsExtended,
                PermissionsAvailable = allPermissions,
                Children = customer.Children.Any() ? CustomerNodesToDto(customer.Children, customerPermissions, customerPermissionsDownwardsExtended) : new()
            });
        }

        return customerNodesDto;
    }
}

internal class GetPossibleCustomerPermissionsForGivenUserQueryAuthorizer : IAuthorizer<GetPossibleCustomerPermissionsForGivenUserQuery>
{
    private readonly IExecutionContext executionContext;

    public GetPossibleCustomerPermissionsForGivenUserQueryAuthorizer(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public async Task<AuthorizationResult> Authorize(GetPossibleCustomerPermissionsForGivenUserQuery query,
        CancellationToken cancellation)
    {
        return await Task.FromResult(AuthorizationResult.Succeed());
    }
}