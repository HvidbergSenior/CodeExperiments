using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users;

[Collection("End2End")]
public class RegisterUserWithoutGivenPasswordTest
{
    private readonly Api api;
    private readonly WebAppFixture webAppFixture;

    public RegisterUserWithoutGivenPasswordTest(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Fact]
    public async Task RegisteredIssueGetsEmailAboutToken()
    {
        //Arrange
        var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(token.AccessToken);

        //Act
        var (userName, _) = await api.RegisterUser(role: UserRole.User);

        //Assert
        var myMessages = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName)).ToList();
        // Remove token
        api.Client.RemoveToken();
        Assert.Single(myMessages);
    }

    [Fact]
    public async Task RegisteredIssueCanUseTokenToChangePassword()
    {
        //Arrange
        var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(token.AccessToken);
        var newPassword = Guid.NewGuid().ToString().ToUpper() + "asd!";

        //Act
        var (userName, _) = await api.RegisterUser(role: UserRole.User);
        var myMessages = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName)).ToList();
        var (userNameFromEmail, tokenFromEmail) = UserTestsHelper.GetUserNameAndTokenFromMessage(myMessages.FirstOrDefault()!);
        await api.ResetPassword(userNameFromEmail, tokenFromEmail, newPassword);
        await api.LoginUser(userNameFromEmail, newPassword);
        // Remove token
        api.Client.RemoveToken();

        //Assert
        //Implicit assertion if all the API calls to reset password and login didn't fail it means it worked
        Assert.Single(myMessages);
        Assert.Equal(userName, userNameFromEmail);
    }
}