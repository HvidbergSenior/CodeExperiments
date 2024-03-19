using Insight.UserAccess.Domain.User;

namespace Insight.Tests.End2End.Users;

using Xunit;

[Collection("End2End")]
public class UnblockUserTests
{
    private readonly Api api;
    private readonly WebAppFixture webAppFixture;

    public UnblockUserTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Theory]
    [InlineData(UserRole.Admin, true)]
    [InlineData(UserRole.User, false)]
    public async Task OnlyAdmin_CanUnblockUser(UserRole userRole, bool canUnblock)
    {
        //Arrange
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var (userName, password) = await api.RegisterUser();
        var (userName2, password2) = await api.RegisterUser(role: userRole);

        var token = await api.LoginUser(userName2, password2);
        api.Client.SetToken(token.AccessToken);

        //Act
        var exceptionFound = false;
        try
        {
            await api.UnblockUser(userName);
        }
        catch (Exception)
        {
            exceptionFound = true;
        }
        finally
        {
            api.Client.RemoveToken();

            //Assert
            Assert.Equal(canUnblock, !exceptionFound);
        }
    }

    [Fact]
    public async Task UnblockedUserCanLoginAgain()
    {
        //Arrange
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var (userName, password) = await api.RegisterUser();

        //Act
        await api.LoginUser(userName, password);
        await api.BlockUser(userName);
        var exceptionFoundWhenBlocked = false;
        var exceptionFoundWhenUnblocked = false;
        try
        {
            await api.LoginUser(userName, password);
        }
        catch (Exception)
        {
            exceptionFoundWhenBlocked = true;
        }

        await api.UnblockUser(userName);
        try
        {
            await api.LoginUser(userName, password);
        }
        catch (Exception)
        {
            exceptionFoundWhenUnblocked = true;
        }

        api.Client.RemoveToken();

        //Assert
        Assert.True(exceptionFoundWhenBlocked);
        Assert.False(exceptionFoundWhenUnblocked);
    }
}