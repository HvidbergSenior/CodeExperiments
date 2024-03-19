using Insight.FuelTransactions.Domain;
using Insight.Services.AllocationEngine.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class PostManualAllocationTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public PostManualAllocationTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanPostAllocationManually()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var filteringParameters = Any.IncomingFilteringParameters();
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Unlocked, webAppFixture, new List<FuelTransactionId>());

            const int seededDeclarationsToFind = 10;

            var seededCustomers = await CustomerTestData.SeedWithCustomer(1, webAppFixture);
            
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParameters, webAppFixture);
           
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, Insight.FuelTransactions.Domain.FilteringParameters.Empty(),false, Guid.NewGuid());
            //Act
            await api.PostAllocationsManually(seededDeclarations.First().IncomingDeclarationId.Value, 0, "Danmark",  seededCustomers.First().CustomerId.Value, filteringParameters.DatePeriod.EndDate, "location", "productName", "productNumber", filteringParameters.DatePeriod.StartDate, "stationName", "company");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
    }
}