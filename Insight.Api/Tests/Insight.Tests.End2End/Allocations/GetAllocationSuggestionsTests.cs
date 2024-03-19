using FluentAssertions;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Allocations
{
    [Collection("End2End")]
    public class GetAllocationSuggestionsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;
        public GetAllocationSuggestionsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanGetAllocationSuggestions()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
          
            var filteringParameters = Any.IncomingFilteringParameters();
           
            const string country = "Denmark";
            const string location = "Jylland";
            
            const int seededDeclarationsToFind = 10;

            var seededCustomers = await CustomerTestData.SeedWithCustomer(1, webAppFixture);
            
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParameters, webAppFixture, IncomingDeclarationState.Reconciled, false, location, country);
            //Act
            var allocationSuggestionsResponse = await api.GetAllocationSuggestions(seededCustomers.First().CustomerId.Value, filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, country, location, 1 ,1 ,true, "Company");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            allocationSuggestionsResponse.Suggestions.Should().NotBeNull();
        }
    }
}