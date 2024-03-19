using Insight.BuildingBlocks.Application.Commands;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.BlockUser
{
    public static class CreateBlockUserEndpoint
    {
        public static void MapBlockUserEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.BLOCK_USER_ENDPOINT, async (BlockUserRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var userName = UserName.Create(request.UserName);

                    var command = BlockUserCommand.Create(userName);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()        
                .WithName("BlockUser")
                .WithTags("Authentication");
        }
    }
}
