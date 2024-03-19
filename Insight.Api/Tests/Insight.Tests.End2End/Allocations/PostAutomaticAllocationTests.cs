using Insight.FuelTransactions.Domain;
using Insight.Services.AllocationEngine.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class PostAutomaticAllocationTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public PostAutomaticAllocationTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanPostAllocationAutomatically()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
          
            var incomingFilteringParameters = Any.IncomingFilteringParameters();
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Unlocked, webAppFixture, new List<FuelTransactionId>());
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, FilteringParameters.Empty(),false, Guid.NewGuid());
           
            var filteringParameters = Any.AutoAllocationFilteringParameters(seededTransactionsToFind.First());

            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(10, incomingFilteringParameters, webAppFixture);

            //Act
            await api.PostAllocationsAutomatically(filteringParameters.StartDate, filteringParameters.EndDate, "", "", "");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
        
        [Fact]
        public async Task UserCanPostAllocationAutomaticallyWithFilteringProduct()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
          
            var incomingFilteringParameters = Any.IncomingFilteringParameters();
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Unlocked, webAppFixture, new List<FuelTransactionId>());
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, FilteringParameters.Empty(),false, Guid.NewGuid());
           
            var filteringParameters = Any.AutoAllocationFilteringParameters(seededTransactionsToFind.First());

            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(10, incomingFilteringParameters, webAppFixture);

            //Act
            await api.PostAllocationsAutomatically(filteringParameters.StartDate, filteringParameters.EndDate, filteringParameters.FilterProductName.Value, "", "");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
        [Fact]
        public async Task UserCanPostAllocationAutomaticallyWithFilteringCompany()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
          
            var incomingFilteringParameters = Any.IncomingFilteringParameters();
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Unlocked, webAppFixture, new List<FuelTransactionId>());
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, FilteringParameters.Empty(),false, Guid.NewGuid());
           
            var filteringParameters = Any.AutoAllocationFilteringParameters(seededTransactionsToFind.First());

            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(10, incomingFilteringParameters, webAppFixture);

            //Act
            await api.PostAllocationsAutomatically(filteringParameters.StartDate, filteringParameters.EndDate, "", filteringParameters.FilterCompanyName.Value,"");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
        [Fact]
        public async Task UserCanPostAllocationAutomaticallyWithFilteringCustomer()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
          
            var incomingFilteringParameters = Any.IncomingFilteringParameters();
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Unlocked, webAppFixture, new List<FuelTransactionId>());
            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, FilteringParameters.Empty(),false, Guid.NewGuid());
           
            var filteringParameters = Any.AutoAllocationFilteringParameters(seededTransactionsToFind.First());

            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(10, incomingFilteringParameters, webAppFixture);

            //Act
            await api.PostAllocationsAutomatically(filteringParameters.StartDate, filteringParameters.EndDate, "", "", filteringParameters.FilterCustomerName.Value);
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
        }
    }
}