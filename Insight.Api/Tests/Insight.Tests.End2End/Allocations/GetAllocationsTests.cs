using FluentAssertions;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;
using FilteringParameters = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class GetAllocationsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public GetAllocationsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanGetAllocations()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var filteringParameters = Any.IncomingFilteringParameters();
            const int seededDeclarations = 2;

            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarations, filteringParameters, webAppFixture, IncomingDeclarationState.New, false);
            
            var seededAllocationDraft = await AllocationDraftsTestData.SeedWithAllocationDraft(AllocationDraftState.Locked, webAppFixture, new List<FuelTransactionId>(), seededIncomingDeclarations.First().IncomingDeclarationId.Value, seededIncomingDeclarations.Last().IncomingDeclarationId.Value, "CustomerName", "Country", "Product", filteringParameters.Company.Value);

            var seededCustomers = await CustomerTestData.SeedWithCustomer(1, webAppFixture);

            //Act
            var allocationsResponse = await api.GetAllocations(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, "Product", filteringParameters.Company.Value, "CustomerName", 0, seededDeclarations, true, "Company");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            allocationsResponse.Allocations.Count.Should().Be(seededDeclarations);
        }
    }
}