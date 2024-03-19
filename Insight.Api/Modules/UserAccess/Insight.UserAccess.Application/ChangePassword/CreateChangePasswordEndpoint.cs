using Insight.BuildingBlocks.Application.Commands;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.ChangePassword
{
    public static class CreateChangePasswordEndpoint
    {
        public static void MapChangePasswordEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.CHANGE_PASSWORD_ENDPOINT, async (ChangePasswordRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var userName = UserName.Create(request.UserName);
                    var currentPassword = Password.Create(request.CurrentPassword);
                    var newPassword = Password.Create(request.NewPassword);
                    var confirmPassword = Password.Create(request.ConfirmPassword);

                    var command = ChangePasswordCommand.Create(userName, currentPassword, newPassword, confirmPassword);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()
                //.Produces<CreateProductResponse
                //Dto>()                
                .WithName("ChangePassword")
                .WithTags("Authentication");
        }
    }
}
