using Insight.UserAccess.Application.ChangePassword;
using Insight.UserAccess.Application.GetAccessToken;
using Insight.UserAccess.Application.LoginUser;
using Insight.UserAccess.Application.RegisterUser;
using Microsoft.AspNetCore.Routing;
using Insight.UserAccess.Application.UpdateUser;
using Insight.UserAccess.Application.BlockUser;
using Insight.UserAccess.Application.ForgotPassword;
using Insight.UserAccess.Application.UnblockUser;
using Insight.UserAccess.Application.GetAllUsers;
using Insight.UserAccess.Application.GetAllUsersForAdmin;
using Insight.UserAccess.Application.ResetPassword;

namespace Insight.UserAccess.Application
{
    public static class UserAccessEndpointExtensions
    {
        public static IEndpointRouteBuilder MapUserAccessEndpoints(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapRegisterUserEndpoint();
            endpoint.MapLoginUserEndpoint();
            endpoint.MapChangePasswordEndpoint();
            endpoint.MapBlockUserEndpoint();
            endpoint.MapUnblockUserEndpoint();
            endpoint.MapGetAccessTokenEndpoint();
            endpoint.MapGetAllUsersEndpoint();
            endpoint.MapUpdateUserEndpoint();
            endpoint.MapGetAllUsersAdminEndpoint();
            endpoint.MapForgotPasswordEndpoint();
            endpoint.MapResetPasswordEndpoint();

            return endpoint;
        }
    }
}
