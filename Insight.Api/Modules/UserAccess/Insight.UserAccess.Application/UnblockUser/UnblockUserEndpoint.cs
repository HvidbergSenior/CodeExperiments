using Insight.BuildingBlocks.Application.Commands;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.UnblockUser
{
    public static class CreateUnblockUserEndpoint
    {
        public static void MapUnblockUserEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.UNBLOCK_USER_ENDPOINT, async (UnblockUserRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var userName = UserName.Create(request.UserName);

                    var command = UnblockUserCommand.Create(userName);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()        
                .WithName("UnblockUser")
                .WithTags("Authentication");
        }
    }
}
