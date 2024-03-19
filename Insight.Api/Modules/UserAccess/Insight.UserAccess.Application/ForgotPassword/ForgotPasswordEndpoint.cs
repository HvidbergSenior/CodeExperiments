using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Exceptions;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.ForgotPassword
{
    public static class CreateForgotPasswordEndpoint
    {
        public static void MapForgotPasswordEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.FORGOT_PASSWORD_ENDPOINT, async (ForgotPasswordRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    if(request.UserName == null && request.Email == null)
                    {
                        throw new BusinessException("You must provide an username or a password to send a link to reset your password");
                    }
                    var userName = request.UserName != null ? UserName.Create(request.UserName) : UserName.Empty();
                    var email = request.Email != null ? Email.Create(request.Email) : Email.Empty();

                    var command = ForgotPasswordCommand.Create(userName, email);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .WithName("ForgotPassword")
                .WithTags("Authentication");
        }
    }
}
