namespace Insight.UserAccess.Application
{
    public static class UserAccessEndpointUrls
    {
        public const string LOGIN_USER_ENDPOINT = "/api/authentication/login";
        public const string CHANGE_PASSWORD_ENDPOINT = "/api/users/changepassword";
        public const string BLOCK_USER_ENDPOINT = "/api/users/blockuser";
        public const string UNBLOCK_USER_ENDPOINT = "/api/users/unblockuser";
        public const string GET_ACCESS_TOKEN_ENDPOINT = "/api/authentication/refresh";
        public const string FORGOT_PASSWORD_ENDPOINT = "/api/users/forgotpassword";
        public const string RESET_PASSWORD_ENDPOINT = "/api/users/resetpassword";
    }
}
