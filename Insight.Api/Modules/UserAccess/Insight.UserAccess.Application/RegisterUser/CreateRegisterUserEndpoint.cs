using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Application.GetAccessToken;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.RegisterUser
{
    public static class CreateRegisterUserEndpoint
    {
        public static void MapRegisterUserEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(UserEndpointUrls.REGISTER_USER_ENDPOINT, async (RegisterUserRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var firstName = FirstName.Create(request.FirstName);
                    var lastName = LastName.Create(request.LastName);
                    var status = UserStatus.Active;
                    var userRole = request.Role;
                    var userName = UserName.Create(request.Username);
                    var email = Email.Create(request.Email);
                    var passwordToUse = request.Password ?? Guid.NewGuid().ToString().ToUpperInvariant() + "!hello";
                    var confirmPasswordToUse = request.ConfirmPassword ?? passwordToUse;
                    var password = Password.Create(passwordToUse);
                    var confirmPassword = Password.Create(confirmPasswordToUse);

                    var customerPermissionGroups = request.CustomerPermissions.Select(c =>
                        CustomerPermissionGroup.Create(CustomerId.Create(c.CustomerId),
                            CustomerNumber.Create(c.CustomerNumber),
                            CustomerName.Create(c.CustomerName),
                            c.Permissions));

                    var command = RegisterUserCommand.Create(userName, email, password, confirmPassword, userRole, customerPermissionGroups, firstName, lastName, status);

                    await commandBus.Send(command, cancellationToken);
                    return Results.Ok();
                })
                .RequireAuthorization()
                .Produces<AuthenticatedResponse>()
                .WithName("RegisterUser")
                .WithTags("Users");
        }
    }
}
