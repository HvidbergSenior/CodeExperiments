using Insight.BuildingBlocks.Application.Commands;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.ResetPassword
{
    public static class CreateResetPasswordEndpoint
    {
        public static void MapResetPasswordEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.RESET_PASSWORD_ENDPOINT, async (ResetPasswordRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var command = ResetPasswordCommand.Create(UserName.Create(request.UserName), ResetPasswordToken.Create(request.Token), Password.Create(request.NewPassword));

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .WithName("ResetPassword")
                .WithTags("Authentication");
        }
    }
}
