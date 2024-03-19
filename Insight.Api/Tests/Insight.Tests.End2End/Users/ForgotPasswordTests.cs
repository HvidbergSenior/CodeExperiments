using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users;

[Collection("End2End")]
public class ForgotPasswordTest
{
    private readonly Api api;
    private readonly WebAppFixture webAppFixture;

    public ForgotPasswordTest(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Fact]
    public async Task ForgotPasswordWithUserNameCanBeUsedToChangePassword()
    {
        //Arrange
        var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(token.AccessToken);
        var newPassword = Guid.NewGuid().ToString().ToUpper() + "asd!";
        var newPasswordForgetPassword = Guid.NewGuid().ToString().ToUpper() + "asd!";

        //Act
        var (userName, _) = await api.RegisterUser(role: UserRole.User);
        var myMessages = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName)).ToList();
        var (userNameFromEmail, tokenFromEmail) = UserTestsHelper.GetUserNameAndTokenFromMessage(myMessages.FirstOrDefault()!);
        await api.ResetPassword(userNameFromEmail, tokenFromEmail, newPassword);
        await api.LoginUser(userNameFromEmail, newPassword);
        await api.ForgotPassword(userNameFromEmail, null);
        var myMessagesForForgotPassword = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName) && m.Content.Contains("forgotten your password")).ToList();
        var (userNameFromForgetEmail, tokenFromForgetEmail) = UserTestsHelper.GetUserNameAndTokenFromMessage(myMessagesForForgotPassword.FirstOrDefault()!);
        await api.ResetPassword(userNameFromForgetEmail, tokenFromForgetEmail, newPasswordForgetPassword);
        await api.LoginUser(userNameFromEmail, newPasswordForgetPassword);
        // Remove token
        api.Client.RemoveToken();

        //Assert
        //Implicit assertion if all the API calls to reset password and login didn't fail it means it worked
        Assert.Single(myMessages);
        Assert.Single(myMessagesForForgotPassword);
        Assert.Equal(userName, userNameFromEmail);
    }

    [Fact]
    public async Task ForgotPasswordWithEmailCanBeUsedToChangePassword()
    {
        //Arrange
        var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(token.AccessToken);
        var newPassword = Guid.NewGuid().ToString().ToUpper() + "asd!";
        var newPasswordForgetPassword = Guid.NewGuid().ToString().ToUpper() + "asd!";

        //Act
        var (userName, _) = await api.RegisterUser(role: UserRole.User);
        var myMessages = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName)).ToList();
        var (userNameFromEmail, tokenFromEmail) = UserTestsHelper.GetUserNameAndTokenFromMessage(myMessages.FirstOrDefault()!);
        await api.ResetPassword(userNameFromEmail, tokenFromEmail, newPassword);
        await api.LoginUser(userNameFromEmail, newPassword);
        var userEmail = myMessages.FirstOrDefault()!.To.FirstOrDefault()!;
        await api.ForgotPassword(null, userEmail);
        var myMessagesForForgotPassword = webAppFixture.GetAllEmailMessages().Where(m => m.To.Contains(userName) && m.Content.Contains("forgotten your password")).ToList();
        var (userNameFromForgetEmail, tokenFromForgetEmail) = UserTestsHelper.GetUserNameAndTokenFromMessage(myMessagesForForgotPassword.FirstOrDefault()!);
        await api.ResetPassword(userNameFromForgetEmail, tokenFromForgetEmail, newPasswordForgetPassword);
        await api.LoginUser(userNameFromEmail, newPasswordForgetPassword);
        // Remove token
        api.Client.RemoveToken();

        //Assert
        //Implicit assertion if all the API calls to reset password and login didn't fail it means it worked
        Assert.Single(myMessages);
        Assert.Single(myMessagesForForgotPassword);
        Assert.Equal(userName, userNameFromEmail);
    }
}