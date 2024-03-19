using Insight.Customers.Application.GetAvailableCustomersPermissions;
using Insight.Customers.Application.GetPossibleCustomerPermissions;
using Insight.Customers.Application.GetPossibleCustomerPermissionsForGivenUser;
using Microsoft.AspNetCore.Routing;

namespace Insight.Customers.Application;

public static class CustomerEndpointExtensions
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGetAvailableCustomersPermissions();
        endpoint.MapGetPossibleCustomerPermissions();
        endpoint.MapGetPossibleCustomerPermissionsForGivenUser();
        return endpoint;
    }
}