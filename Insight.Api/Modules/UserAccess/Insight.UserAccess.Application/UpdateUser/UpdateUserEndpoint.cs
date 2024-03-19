using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace Insight.UserAccess.Application.UpdateUser;

public static class UpdateUserEndpoint
{
    public static void MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint
            .MapPut(UserEndpointUrls.UPDATE_USER_ENDPOINT, async (
                UpdateUserRequest request,
                ICommandBus commandBus,
                IMemoryCache memoryCache,
                CancellationToken cancellationToken) =>
            {
                var updateUserParameters = CreateUserUpdateParameters(request);
                var customerPermissionGroups = request.CustomerPermissions.Select(c => CustomerPermissionGroup.Create(CustomerId.Create(c.CustomerId), CustomerNumber.Create(c.CustomerNumber), CustomerName.Create(c.CustomerName), c.Permissions));

                var uname = request.Username.ToLowerInvariant();
                
                var userNameCacheName = "UserName_" + uname;

                memoryCache.Remove(userNameCacheName);
                
                var command = UpdateUserCommand.Create(updateUserParameters, customerPermissionGroups);
                
                var result = await commandBus.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .Produces<UpdateUserResponse>()
            .WithName("UpdateUser")
            .WithTags("Users");
    }

    private static UserUpdateParameters CreateUserUpdateParameters(UpdateUserRequest request)
    {
        var userUpdateParameters = UserUpdateParameters.Create(
            UserName.Create(request.Username),
            UserId.Create(Guid.Parse(request.UserId)),
            FirstName.Create(request.FirstName),
            LastName.Create(request.LastName),
            Email.Create(request.Email),
            request.Role,
            request.Status
        );
        return userUpdateParameters;
    }
}