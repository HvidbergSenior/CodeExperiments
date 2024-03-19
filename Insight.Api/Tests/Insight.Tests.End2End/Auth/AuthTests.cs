using FluentAssertions;
using Insight.UserAccess.Application.GetAccessToken;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Auth
{
    [Collection("End2End")]
    public class AuthTests
    {

        private readonly Api api;
        private readonly WebAppFixture webAppFixture;

        public AuthTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanRenewAccessToken_WithValidRefreshToken()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            // // Login
            // AuthenticatedResponse tokenResponse = await api.LoginUser(UserName, Password);

            // Renew Access token
            AuthenticatedResponse newToken = await api.RenewAccessToken(token);

            newToken.RefreshToken.Should().Be(token.RefreshToken);
            newToken.AccessToken.Should().NotBe(token.AccessToken);
        }
    }
}
