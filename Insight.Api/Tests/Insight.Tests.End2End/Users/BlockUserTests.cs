using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users;

[Collection("End2End")]
public class BlockUserTests
{
    private readonly Api api;
    private readonly WebAppFixture webAppFixture;
    
    public BlockUserTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Theory]
    [InlineData(UserRole.Admin, true)]
    [InlineData(UserRole.User, false)]
    public async Task OnlyAdmin_CanBlockUser(UserRole userRole, bool canBlock)
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
            await api.BlockUser(userName);
        }
        catch (Exception)
        {
            exceptionFound = true;
        }
        finally
        {
            api.Client.RemoveToken();

            //Assert
            Assert.Equal(canBlock, !exceptionFound);
        }
    }

    [Fact]
    public async Task ToAvoidLoosingAllAdminsICantBlockMyself()
    {
        //Arrange
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var (userNameAdmin, passwordAdmin) = await api.RegisterUser(role: UserRole.Admin);

        var token = await api.LoginUser(userNameAdmin, passwordAdmin);
        api.Client.SetToken(token.AccessToken);

        //Act
        var exceptionFound = false;
        try
        {
            await api.BlockUser(userNameAdmin);
        }
        catch (Exception)
        {
            exceptionFound = true;
        }
        finally
        {
            api.Client.RemoveToken();

            //Assert
            Assert.True(exceptionFound);
        }
    }

    [Fact]
    public async Task BlockedUserCantLogin()
    {
        //Arrange
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);

        var (userName, password) = await api.RegisterUser();

        //Act
        await api.BlockUser(userName);
        var exceptionFound = false;
        try
        {
            await api.LoginUser(userName, password);
        }
        catch (Exception)
        {
            exceptionFound = true;
        }
        finally
        {
            api.Client.RemoveToken();

            //Assert
            Assert.True(exceptionFound);
        }
    }
}