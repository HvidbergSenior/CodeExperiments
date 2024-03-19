using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class PublishAllocationsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public PublishAllocationsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanPublishAllocations()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            var filteringParameters = Any.IncomingFilteringParameters();
            const int seededDeclarations = 2;

            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarations, filteringParameters, webAppFixture, IncomingDeclarationState.New, false);
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, FilteringParameters.Empty(),false, Guid.NewGuid());
            var ids = new List<FuelTransactionId>();
            foreach (var fuelTransaction in seededTransactionsToFind)
            {
                ids.Add(fuelTransaction.FuelTransactionId);
            }
                
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Locked, webAppFixture, ids, seededIncomingDeclarations.First().IncomingDeclarationId.Value, seededIncomingDeclarations.Last().IncomingDeclarationId.Value,"Martin", "Denmark", "RapeSeed");

            seededIncomingDeclarations.First()
                .AddAllocation(seededAllocationDraft.First().Allocations.First().Key.Value, 1000);

            var seededCustomers = await CustomerTestData.SeedWithCustomer(1, webAppFixture);

            //Act
            await api.PublishAllocations();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
    }
}