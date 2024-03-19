using Insight.BuildingBlocks.Application.Queries;
using Insight.UserAccess.Domain.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.GetAccessToken
{
    public static class CreateGetAccessTokenEndpoint
    {
        public static void MapGetAccessTokenEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserAccessEndpointUrls.GET_ACCESS_TOKEN_ENDPOINT, async (AccessTokenRequest request,
                    IQueryBus queryBus,
                    CancellationToken cancellationToken) =>
                {
                    var accessToken = AccessToken.Create(request.AccessToken);
                    var refreshToken = RefreshToken.Create(request.RefreshToken);

                    var accessTokenQuery = GetAccessTokenQuery.Create(refreshToken, accessToken);

                    var result = await queryBus.Send<GetAccessTokenQuery, GetAccessTokenResponse>(accessTokenQuery, cancellationToken);

                    var authResponse = new AuthenticatedResponse(refreshToken.Value, result.AccessToken.Value);

                    return Results.Ok(authResponse);
                })
                .AllowAnonymous()
                .Produces<AuthenticatedResponse>()
                .WithName("Refresh")
                .WithTags("Authentication");
        }
    }
}
