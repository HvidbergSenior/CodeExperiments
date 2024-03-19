using Insight.BuildingBlocks.Application.Commands;
using Insight.UserAccess.Application.GetAccessToken;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.LoginUser
{
    public static class CreateLoginUserEndpoint
    {
        public static void MapLoginUserEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.LOGIN_USER_ENDPOINT, async (LoginRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var userName = UserName.Create(request.Username);
                    var password = Password.Create(request.Password);

                    var command = LoginUserCommand.Create(userName, password);
                    var result = await commandBus.Send(command, cancellationToken);

                    return Results.Ok(new AuthenticatedResponse(result.RefreshToken.Value,
                                                        result.AccessToken.Value));
                })
                .Produces<AuthenticatedResponse>()                
                .AllowAnonymous()
                .WithName("Login")
                .WithTags("Authentication");
        }
    }
}
