using Insight.FuelTransactions.Domain;
using Insight.Services.AllocationEngine.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class UnlockAllocationsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public UnlockAllocationsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanUnlockAllocations()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Locked, webAppFixture, new List<FuelTransactionId>());

            //Act
            await api.UnlockAllocations();

            api.Client.RemoveToken();

            //Assert
        }
    }
}